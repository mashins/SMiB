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

    private List<Vector2Int> newPath;
    private int currentPathIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextStep();
        }
    }

    private void NextStep()
    {
        if (newPath == null)
            newPath = PathFinding.instance.FindPath(GetCurrentIndex());

        if (currentPathIndex >= newPath.Count - 1 || newPath.Count == 0)
            return; //Found destination, or not found path

        float rand = Random.Range(0.0f, 1.0f);
        float p = 0.4f;

        if (rand < p)
        {
            //Add visited roads
            PathFinding.instance.DisableConnection(newPath[currentPathIndex], newPath[currentPathIndex + 1]);
            newPath = PathFinding.instance.FindPath(newPath[currentPathIndex]);
            currentPathIndex = 0;
        }
        else
        {
            currentPathIndex++;
            Vector2Int NewPathPostion = newPath[currentPathIndex];
            transform.position = (Vector2)NewPathPostion + new Vector2(0.5f, 0.5f);
        }
    }

    private Vector2Int GetCurrentIndex()
    {
        Vector2 newPos = transform.position - new Vector3(0.5f, 0.5f);
        return new Vector2Int((int)newPos.x, (int)newPos.y);
    }

    public void OnSetup(MapParam mParam)
    {
        this.mParam = mParam;
        transform.localScale = Vector3.one / 3 / mParam.Size;
    }

    public void OnStart()
    {
        transform.position = (Vector2)mParam.StartIndex + new Vector2(0.5f, 0.5f);
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


