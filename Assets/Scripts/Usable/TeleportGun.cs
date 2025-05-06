using UnityEngine;

public class TeleportGun : AmmoUsable
{
    public GunLine line;
    public LayerMask ignoreMask = 1 << 2;
    public Transform shootSpot;

    public float range = 25f;

    struct PlayerHitInfo
    {
        public PlayerBody player;
        public float hitDist;

        public PlayerHitInfo(PlayerBody player, float hitDist)
        {
            this.player = player;
            this.hitDist = hitDist;
        }
    }

    protected override bool UseObject(Player user)
    {
        PlayerHitInfo playerHit = Shoot(user);

        GunLine gl = Instantiate(line.gameObject).GetComponent<GunLine>();
        gl.CreateLine(new GunBullet.ShotData[] { new GunBullet.ShotData(playerHit.hitDist, 1f, 1f - playerHit.hitDist / range) }, shootSpot.position, shootSpot.up);

        if (playerHit.player != null)
            user.TeleportPlayer(playerHit.player);

        return true;
    }

    PlayerHitInfo Shoot(Player user)
    {
        RaycastHit2D[] rcs = Physics2D.RaycastAll(shootSpot.position, shootSpot.up, range, ~ignoreMask);
        PlayerBody ourBody = user.GetComponent<PlayerBody>();

        foreach (RaycastHit2D hit in rcs)
        {
            PlayerBody hitPlayer = hit.transform.GetComponent<PlayerBody>();

            if (hitPlayer == null)
                return new PlayerHitInfo(null, hit.distance);

            if (hitPlayer.Equals(ourBody))
                continue;

            return new PlayerHitInfo(hitPlayer, hit.distance);
        }

        return new PlayerHitInfo(null, range);
    }
}
