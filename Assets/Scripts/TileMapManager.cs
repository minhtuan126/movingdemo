using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class TileMapManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    public float zSpawn = 1;
    public float tileLength = 30;
    public int numberOfTiles = 5;

    public Transform target;
    private List<GameObject> activeTiles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == 0)
                SpawnTile(0);
            else
                SpawnTile(Random.Range(1, tilePrefabs.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((target.position.z - 35) > (zSpawn - (5 * tileLength)))
        {
            SpawnTile(Random.Range(1, tilePrefabs.Length));
            DeleteTile();
        }
    }

    void SpawnTile(int tileIndex)
    {
        GameObject tile =  Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(tile);
        zSpawn += tileLength;
    }
    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
