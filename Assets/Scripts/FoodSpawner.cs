using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FoodSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile foodTile;
    public Vector3Int spawnAreaMin;
    public Vector3Int spawnAreaMax;
    public static FoodSpawner instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SpawnFood();
    }

    public void SpawnFood()
    {
        Vector3Int randomPosition = GetRandomPositionInBounds();

        if (CanSpawnAt(randomPosition))
        {
            tilemap.SetTile(randomPosition, foodTile);
        }
        else
        {
            SpawnFood();
        }
    }

    private Vector3Int GetRandomPositionInBounds()
    {
        int x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        int y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector3Int(x, y, 0);
    }

    private bool CanSpawnAt(Vector3Int position)
    {
        TileBase tile = tilemap.GetTile(position);

        return tile == null;
    }
}