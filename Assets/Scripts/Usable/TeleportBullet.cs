using System;
using UnityEngine;

public class TeleportBullet : MonoBehaviour, Bullet
{
    public LayerMask ignoreMask = 1 << 2;

    public float range = 25f;

    public event Action<Bullet.Data> OnFire;

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

    PlayerHitInfo Shoot(Vector2 pos, Vector2 dir, Player owner)
    {
        RaycastHit2D[] rcs = Physics2D.RaycastAll(pos, dir, range, ~ignoreMask);
        PlayerBody ourBody = owner.GetComponent<PlayerBody>();

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

    public void Fire(Vector2 pos, Vector2 dir, Player owner)
    {
        PlayerHitInfo playerHit = Shoot(pos, dir, owner);
        Bullet.LineData[] lineData = new Bullet.LineData[] { 
            new Bullet.LineData(playerHit.hitDist, 1f, 1f - playerHit.hitDist / range, (playerHit.player == null ? null : playerHit.player.transform)) 
        };
        Bullet.Data data = new Bullet.Data (pos, dir, lineData);
        
        OnFire?.Invoke(data);

        if (playerHit.player != null)
            owner.TeleportPlayer(playerHit.player);
    }
}
