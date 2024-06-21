using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private int cameraOffset = 300;
    [SerializeField] private int maxMapSize = 500;

    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Tile crossTile;

    private int mapSize;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void Setup(string mapSize)
    {
        if (!int.TryParse(mapSize, out int result)) return;

        this.mapSize = Mathf.Clamp(Mathf.Abs(result), 0, maxMapSize);

        tileMap.ClearAllTiles();

        BoundsInt bounds = new BoundsInt(0, 0, 0, this.mapSize, this.mapSize, 1);
        TileBase[] tileArray = Enumerable.Repeat<TileBase>(crossTile, this.mapSize * this.mapSize).ToArray();
        tileMap.SetTilesBlock(bounds, tileArray);
        mainCamera.orthographicSize = (float)this.mapSize / 2;
        float width = mainCamera.aspect * mainCamera.orthographicSize * 2;
        float offsetValue = cameraOffset * width / Screen.width;
        
        mainCamera.transform.position = new Vector3((float)this.mapSize / 2 - offsetValue / 2, (float)this.mapSize / 2, -10);

    }

}

public struct MapParam
{
    public int Size;
    public Vector2Int StartIndex;
    public Vector2Int EndIndex;
    public float BlockProbability;
}
