using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PathFinding
{
    private int size = -1;
    private Vector2Int endDestination;
    private Cell[,] cells;
    private bool[,] toVisit;

    private static PathFinding _instance;
    public static PathFinding instance
    {
        get
        {
            if( _instance == null )
                _instance = new PathFinding();
            return _instance;
        }
    }

    public void InitSize(int Size)
    {
        size = Size;
        cells = new Cell[size, size];
        toVisit = new bool[size, size];

        //Init cells
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                //Define distance
                Vector2Int index = new Vector2Int(i, j);
                float dist = Vector2Int.Distance(endDestination, index);
                cells[i, j] = new Cell() { distanceToEnd = dist, index = index };
            }
        }

        //Set basic Connections
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i != 0)
                    cells[i, j].neighbours[(int)Direction.Left] = cells[i - 1, j];
                if (i != size - 1)
                    cells[i, j].neighbours[(int)Direction.Right] = cells[i + 1, j];

                if (j != 0)
                    cells[i, j].neighbours[(int)Direction.Down] = cells[i, j - 1];
                if (j != size - 1)
                    cells[i, j].neighbours[(int)Direction.Up] = cells[i, j + 1];

            }
        }
    }

    public void InitEndDestination(Vector2Int EndDestination)
    {
        if (size == -1)
            return;
        endDestination = EndDestination;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Vector2Int index = new Vector2Int(i, j);
                float dist = Vector2Int.Distance(endDestination, index);
                cells[i, j].distanceToEnd = dist;
            }
        }
    }

    public List<Vector2Int> FindPath(Vector2Int carPosition)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cells[i, j].cameFrom = null;
                toVisit[i, j] = false;
            }
        }

        List<Cell> CellsToCheck = new List<Cell>();

        Cell CellToFind = cells[endDestination.x, endDestination.y];
        Cell StartingCell = cells[carPosition.x, carPosition.y];

        Cell LastCell = StartingCell;
        toVisit[LastCell.index.x, LastCell.index.y] = true;

        do
        {
            if (LastCell != null) //Not found any!
            {
                Cell[] LastCellNeighbours = LastCell.neighbours.Where(e => e != null && !toVisit[e.index.x, e.index.y]).ToArray();

                if (LastCellNeighbours.Contains(CellToFind)) //FoundCell
                {
                    CellToFind.cameFrom = LastCell;
                    break;
                }

                foreach (Cell cell in LastCellNeighbours) //Mark to visit
                {
                    toVisit[cell.index.x, cell.index.y] = true;
                    cell.cameFrom = LastCell;
                }

                CellsToCheck.AddRange(LastCellNeighbours);
                CellsToCheck.Remove(LastCell);
            }
            int NewIndex = GetIndexOfLowestValue(CellsToCheck);

            if (NewIndex != -1)
                LastCell = CellsToCheck[NewIndex];

        }
        while (CellsToCheck.Count > 0);

        //Return
        List<Vector2Int> outList = new List<Vector2Int>();
        if (CellToFind.cameFrom != null)
        {
            Cell cell = CellToFind;
            outList.Add(cell.index); //Last index;

            do
            {
                if (cell.cameFrom != null)
                    Debug.DrawLine(
                        new Vector3(cell.index.x + 0.5f, cell.index.y + 0.5f),
                        new Vector3(cell.cameFrom.index.x + 0.5f, cell.cameFrom.index.y + 0.5f), Color.red, 2);

                outList.Add(cell.cameFrom.index);
                cell = cell.cameFrom;
            }
            while (cell.cameFrom != null);
            outList.Reverse();
        }

        return outList;
    }

    public bool DisableConnection(Vector2Int cell1, Vector2Int cell2)
    {
        return DisableConnection(cells[cell1.x, cell1.y], cells[cell2.x, cell2.y]);
    }

    private bool DisableConnection(Cell cell1, Cell cell2)
    {
        bool result = false;
        for (int i = 0; i < 4; i++)
        {
            if (cell1.neighbours[i] == cell2 && !cell1.savedNeighbours[i])
            {
                result = true;
                cell1.neighbours[i] = null;
            }
            if (cell2.neighbours[i] == cell1 && !cell2.savedNeighbours[i])
            {
                result = true;
                cell2.neighbours[i] = null;
            }
        }
        return result;
    }
    public void SaveConnection(Vector2Int cell1, Vector2Int cell2)
    {
        SaveConnection(cells[cell1.x, cell1.y], cells[cell2.x, cell2.y]);
    }

    private void SaveConnection(Cell cell1, Cell cell2)
    {
        for (int i = 0; i < 4; i++)
        {
            if (cell1.neighbours[i] == cell2 && !cell1.savedNeighbours[i])
            {

                Debug.DrawRay((Vector2)cell1.index + new Vector2(0.5f, 0.5f), (Vector2)(cell2.index - cell1.index) * 0.5f, Color.cyan, 100);
                cell1.savedNeighbours[i] = true;
            }
            if (cell2.neighbours[i] == cell1 && !cell2.savedNeighbours[i])
            {
                Debug.DrawRay((Vector2)cell2.index + new Vector2(0.5f, 0.5f), (Vector2)(cell1.index - cell2.index) * 0.5f, Color.magenta, 100);
                cell2.savedNeighbours[i] = true;
            }
        }
    }

    private static int GetIndexOfLowestValue(List<Cell> arr)
    {
        float value = float.PositiveInfinity;
        int index = -1;
        for (int i = 0; i < arr.Count; i++)
        {
            if (arr[i].distanceToEnd < value)
            {
                index = i;
                value = arr[i].distanceToEnd;
            }
        }
        return index;
    }

    private class Cell
    {
        public Cell[] neighbours = new Cell[4];
        public bool[] savedNeighbours = new bool[4];
       
        public Cell cameFrom;
        public Vector2Int index;
        public float distanceToEnd;

    }

    public enum Direction : int
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}
