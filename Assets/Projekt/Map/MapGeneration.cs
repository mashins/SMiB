using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private int cameraOffset = 300;

    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Tile crossTile;
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject end;

    private int mapSize;
    private Vector2Int startIndex;
    private Vector2Int endIndex;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void SetupStart(Vector2Int index, bool state)
    {
        startIndex = index;
        start.transform.position = index + new Vector2(0.5f, 0.5f);
        start.SetActive(state);
    }

    public void SetupEnd(Vector2Int index, bool state)
    {
        endIndex = index;
        end.transform.position = index + new Vector2(0.5f, 0.5f);
        end.SetActive(state);
    }

    public void SetupMap(int mapSize)
    {
        this.mapSize = mapSize;

        tileMap.ClearAllTiles();

        BoundsInt bounds = new BoundsInt(0, 0, 0, this.mapSize, this.mapSize, 1);
        TileBase[] tileArray = Enumerable.Repeat<TileBase>(crossTile, this.mapSize * this.mapSize).ToArray();
        tileMap.SetTilesBlock(bounds, tileArray);

        if (!bounds.Contains((Vector3Int)startIndex)) start.SetActive(false);
        if (!bounds.Contains((Vector3Int)endIndex)) end.SetActive(false);

        //Vector2Int endPos = new Vector2Int(this.mapSize - 1, 2);
        PathFinding.instance.InitSize(this.mapSize);

        if (mapSize == 0) return;

        mainCamera.orthographicSize = (float)this.mapSize / 2;
        float width = mainCamera.aspect * mainCamera.orthographicSize * 2;
        float offsetValue = cameraOffset * width / Screen.width;

        mainCamera.transform.position = new Vector3((float)this.mapSize / 2 - offsetValue / 2, (float)this.mapSize / 2, -10);


    }

}


