using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CarHandle : MonoBehaviour
{
    [SerializeField] private float carSpeed;

    private Tilemap map;
    private MapParam mParam;

    private Vector2Int currentIndex;

    private Dictionary<Vector2Int, bool> passedNodes = new Dictionary<Vector2Int, bool>();
    private Dictionary<Vector2Int, bool> blockedNodes = new Dictionary<Vector2Int, bool>();

    public void OnSetup(MapParam mParam)
    {
        this.mParam = mParam;
        transform.localScale = Vector3.one / 3 / mParam.Size;
    }

    public void OnStart()
    {
        transform.position = (Vector2)mParam.StartIndex;
    }


    private void OnIndexChange()
    {
        
    }

    private Vector2Int GetNextNode()
    {
        return Vector2Int.zero;
    }

    private List<Vector2Int> GetNeighbours(Vector2Int index)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        Vector2Int neighbour = index + Vector2Int.up;
        if (neighbour.x >= 0 && neighbour.x <= mParam.Size && neighbour.y >= 0 && neighbour.y <= mParam.Size)
        {

        }



        return neighbours;
    }

    public void OnUpdate()
    {

    }


}


