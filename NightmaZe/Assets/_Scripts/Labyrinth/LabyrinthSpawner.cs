using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LabyrinthSpawner : NetworkBehaviour {

    private Labyrinth labyrinth;
    private GameObject[,] field;

    [Range(1, 9)]
    public int difficulty = 1;

    [Range(1, 5)]
    public int tileSize = 3;

    public GameObject free;
    public GameObject wall;
    public GameObject entry;
    public GameObject exit;
    public GameObject staticTrap;
    public GameObject dynamicTrap;
    public GameObject key;

    // Use this for initialization

    public override void OnStartServer()
    {
        labyrinth = new Labyrinth();
        Create();
    }
    
    //Initialization methods
    private void Create()
    {
        Labyrinth.Tile[,] tiles = labyrinth.GetTiles();
        field = new GameObject[tiles.GetLength(0), tiles.GetLength(1)];
        Labyrinth.Properties properties = labyrinth.GetProperties();

        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                GameObject tile;
                Transform transform = gameObject.transform;
                Vector3 position = new Vector3(x * tileSize, 0.4f, y * tileSize);
                switch (tiles[x, y])
                {
                    case Labyrinth.Tile.ENTRY:
                        tile = Instantiate(entry, transform);
                        break;
                    case Labyrinth.Tile.EXIT:
                        tile = Instantiate(exit, transform);
                        break;
                    case Labyrinth.Tile.DYNAMIC_TRAP:
                        tile = Instantiate(dynamicTrap, transform);
                        break;
                    case Labyrinth.Tile.STATIC_TRAP:
                        tile = Instantiate(staticTrap, transform);
                        break;
                    case Labyrinth.Tile.WALL:
                        tile = Instantiate(wall, transform);
                        break;
                    case Labyrinth.Tile.KEY:
                        tile = Instantiate(key, transform);
                        break;
                    default:
                        tile = Instantiate(free, transform);
                        position.y = 0;
                        break;
                }
                position.y *= tileSize;

                NetworkServer.Spawn(tile);
                tile.transform.position = position;
                tile.transform.localScale = Vector3.one * tileSize ;
                SyncScaleRotation t = tile.GetComponent<SyncScaleRotation>();
                t.rotation = tile.transform.rotation;
                t.scale = tile.transform.localScale;
               
                NetworkServer.Spawn(tile);
            }
        }
    }
}
