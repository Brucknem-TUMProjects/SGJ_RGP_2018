using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthSpawner : MonoBehaviour {


    private Labyrinth labyrinth;
    private GameObject[,] field;

    public GameObject free;
    public GameObject wall;
    public GameObject entry;
    public GameObject exit;
    public GameObject staticTrap;
    public GameObject dynamicTrap;
    public GameObject key;

    //private int seed = new System.Random().Next(0, 10000);

    public bool randomize = false;
    [Range(1, 9)]
    public int difficulty = 1;
    [Range(1, 5)]
    public int tileSize = 3;
    [Range(0, 10000)]
    public int seed = new System.Random().Next(0, 10000);
    [Range(10, 150)]
    public int width = 50;
    [Range(10, 150)]
    public int height = 50;
    [Range(0, 10)]
    public int inNr = 0;
    [Range(1, 10)]
    public int outNr = 5;
    [Range(0, 100)]
    public int staticNr = 25;
    [Range(0, 100)]
    public int dynamicNr = 25;
    [Range(0, 5)]
    public int keyNr = 2;
    [Range(1, 10)]
    public int density = 1;

    // Use this for initialization

    //public override void OnStartServer()
    private void Start()
    {
        //    DontDestroyOnLoad(gameObject);

        labyrinth = new Labyrinth(seed, width,height,inNr,outNr,staticNr,dynamicNr,keyNr,density);
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
                Vector3 position = new Vector3(x * tileSize, 0, y * tileSize);
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

                tile.transform.position = position;
                tile.transform.localScale = Vector3.one * tileSize ;
                //SyncScaleRotation t = tile.GetComponent<SyncScaleRotation>();
                //t.rotation = tile.transform.rotation;
                //t.scale = tile.transform.localScale;
               
                //NetworkServer.Spawn(tile);
            }
        }
    }
}
