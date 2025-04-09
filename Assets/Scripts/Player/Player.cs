using UnityEngine;

public class Player : MonoBehaviour
{
    private int id;

    public int GetPlayerID()
    {
        return id;
    }

    public void SetPlayerID(int id)
    {
        this.id = id;
    }
}
