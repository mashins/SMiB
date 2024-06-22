using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CarHandle : MonoBehaviour
{
    [SerializeField] private float carSpeed;
    [SerializeField] private LineRenderer passedPath;
    [SerializeField] private LineRenderer currentPath;

    private MapParam mParam;

    private List<Vector2Int> newPath;
    private int currentPathIndex = 0;

    private bool move;
    private Vector2 moveDirection;

    private int blockadeIndex;
    private List<GameObject> allBlockadeVisuals = new List<GameObject>();

    public void SetupBlockades(List<GameObject> blockades)
    {
        allBlockadeVisuals = blockades;
    }

    private bool NextStep(out bool blocked)
    {
        if (newPath == null)
            newPath = PathFinding.instance.FindPath(GetCurrentIndex());

        blocked = false;
        if (currentPathIndex >= newPath.Count - 1 || newPath.Count == 0)
            return true; //Found destination, or not found path

        float rand = Random.Range(0.0f, 1.0f);
        float p = mParam.BlockProbability;

        if (rand < p)
        {
            //Add visited roads
            if (PathFinding.instance.DisableConnection(newPath[currentPathIndex], newPath[currentPathIndex + 1]))
            {
                Debug.Log("Block");
                blocked = true;
               // Debug.DrawLine((Vector2)newPath[currentPathIndex] + new Vector2(0.5f, 0.5f), (Vector2)newPath[currentPathIndex + 1] + new Vector2(0.5f, 0.5f), Color.blue, 100);
                SetBlockade(newPath[currentPathIndex], newPath[currentPathIndex + 1]);
                newPath = PathFinding.instance.FindPath(newPath[currentPathIndex]);
                currentPathIndex = 0;
                return false;
            }

        }



        PathFinding.instance.SaveConnection(newPath[currentPathIndex], newPath[currentPathIndex + 1]);

        currentPath.positionCount = newPath.Count - currentPathIndex;
        for (int i = 0; i < currentPath.positionCount; i++)
        {
            currentPath.SetPosition(i, (Vector2)newPath[currentPathIndex + i] + new Vector2(0.5f, 0.5f));
        }
        currentPathIndex++;

        Debug.Log("dziala");


        Vector2 newTargetPostion = (Vector2)newPath[currentPathIndex] + new Vector2(0.5f, 0.5f);
        moveDirection = (newTargetPostion - (Vector2)transform.position).normalized;
        //transform.position = (Vector2)NewPathPostion + new Vector2(0.5f, 0.5f);
        return false;
    }

    private void SetBlockade(Vector2Int cell1, Vector2Int cell2)
    {
        Vector2 pos1 = cell1 + new Vector2(0.5f, 0.5f);
        Vector2 pos2 = cell2 + new Vector2(0.5f, 0.5f);
        Vector2 vec = pos2 - pos1;

        Vector2 blockadePos = (pos1 + pos2) / 2;

        float dot = Mathf.Abs(Vector2.Dot(vec.normalized, Vector2.up));

        allBlockadeVisuals[blockadeIndex].SetActive(true);
        allBlockadeVisuals[blockadeIndex].transform.position = blockadePos;
        allBlockadeVisuals[blockadeIndex].transform.localScale = dot > 0.5f ? new Vector2(0.3f, 0.8f) : new Vector2(0.8f, 0.3f);



       // allBlockadeVisuals[blockadeIndex].transform.localScale = 

        blockadeIndex++;
    }

    private Vector2Int GetCurrentIndex()
    {
        Vector2 newPos = transform.position - new Vector3(0.5f, 0.5f);
        return new Vector2Int((int)newPos.x, (int)newPos.y);
    }

    public void OnSetup(MapParam mParam)
    {
        this.mParam = mParam;
    }

    public void OnStart()
    {
        if (mParam == null) return;

        PathFinding.instance.InitEndDestination(mParam.EndIndex);
        transform.position = (Vector2)mParam.StartIndex + new Vector2(0.5f, 0.5f);

        newPath = null;
        currentPathIndex = 0;

        passedPath.positionCount = 2;
        passedPath.SetPosition(0, transform.position);

        CheckMoveDirection();
    }

    private void StartMove()
    {
        move = true;
    }
    private void StopMove()
    {
        move = false;

        if (newPath == null || newPath.Count == 0 || currentPathIndex >= newPath.Count) return;

        transform.position = (Vector2)newPath[currentPathIndex] + new Vector2(0.5f, 0.5f);
        passedPath.SetPosition(passedPath.positionCount - 1, transform.position);
        passedPath.positionCount++;
        passedPath.SetPosition(passedPath.positionCount - 1, transform.position);


    }

    private void CheckMoveDirection()
    {
        StopMove();

        bool isBlocked = true;
        while (isBlocked)
        {
            isBlocked = false;

            if (NextStep(out isBlocked)) return;
        }

        StartMove();
    }

    public void OnUpdate()
    {
        if (!move) return;

        Vector2 vec =  (Vector2)newPath[currentPathIndex] + new Vector2(0.5f, 0.5f) - (Vector2)transform.position ;
        Vector2 vecDir = vec.normalized;
        float vecDist = vec.magnitude;

        if (vecDist <= carSpeed * Time.deltaTime)
        {
            CheckMoveDirection();
            return;
        }

        transform.position += carSpeed * Time.deltaTime * (Vector3)vecDir;

        passedPath.SetPosition(passedPath.positionCount - 1, transform.position);

        currentPath.SetPosition(0, transform.position);
    }


}


