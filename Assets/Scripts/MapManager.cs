using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] maps;

    public void CreateMap(GameState state)
    {
        if(state.curMap == -1)
            state.curMap = Random.Range(0, maps.Length);

        Instantiate(maps[state.curMap]);
        state.curMap = (state.curMap + 1) % maps.Length;
    }
}
