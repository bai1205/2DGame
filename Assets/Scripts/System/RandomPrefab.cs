using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;
using System.Security.Cryptography;

public class RandomPrefab : MonoBehaviour
{
    public Tilemap tilemap; // 场景中的Tilemap引用
    public GameObject[] prefabs; // 要生成的Prefabs数组
    public int numberOfPrefabsToSpawn = 15; // 生成Prefabs的数量
    private List<Vector3Int> tilePositions = new List<Vector3Int>();

    public float initial_MeleeEnemy = 15; // 开局时生成的敌人数量
    public float initial_RangedEnemy = 5;
    //public float spawnInterval; // 生成Prefabs的时间间隔，单位是秒\
    private int MeleeEnemy_count;
    private int RangedEnemy_count;

    public Transform playerTransform;

    void Start()
    {

        collection();
        SpawnInitialEnemy(numberOfPrefabsToSpawn, prefabs[0]);
        SpawnInitialEnemy(numberOfPrefabsToSpawn, prefabs[1]);
       SpawnInitialEnemy(initial_MeleeEnemy, prefabs[2]);
       SpawnInitialEnemy(initial_RangedEnemy, prefabs[3]);
        RescanPathfindingGraph();
    }
    public void RescanPathfindingGraph()
    {
        AstarPath.active.Scan();
    }
    void Update()
    {
        MeleeEnemy_count = GameObject.FindGameObjectsWithTag("MeleeEnemy").Length;
        RangedEnemy_count = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
    void collection()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 1; x < bounds.size.x; x++)
        {
            for (int y = 1; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    tilePositions.Add(new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0));
                }
            }

        }  
    }
    void SpawnInitialEnemy(float count,GameObject prefab)
    {
        for (int i = 0; i < count; i++)
        {
            RandomPrefabAroundPlayer(prefab);
            //Enemy(RangedEnemy);

        }
    }
    public void RandomPrefabAroundPlayer(GameObject prefab)
    {
        Vector3Int playerPositionOnTilemap = tilemap.WorldToCell(playerTransform.position);
        SpawnPrefabAvoidingPlayer(prefab, playerPositionOnTilemap, 2); // 2是检查半径，对应5x5范围
    }
    void SpawnPrefabAvoidingPlayer(GameObject prefab, Vector3Int playerPosition, int checkRadius)
    {
        List<Vector3Int> validPositions = new List<Vector3Int>(tilePositions);

        foreach (var position in tilePositions)
        {
            if (Mathf.Abs(position.x - playerPosition.x) <= checkRadius &&
                Mathf.Abs(position.y - playerPosition.y) <= checkRadius)
            {
                validPositions.Remove(position);
            }
        }

        if (validPositions.Count == 0) return;

        int randomIndex = Random.Range(0, validPositions.Count);
        Vector3Int randomPosition = validPositions[randomIndex];
        tilePositions.Remove(randomPosition); // 可选，如果你不想再次在这个位置生成

        Vector3 worldPosition = tilemap.CellToWorld(randomPosition) + new Vector3(0.5f, 0.5f, 0);
        Instantiate(prefab, worldPosition, Quaternion.identity);
    }
 /*   public void Random_Prefab(GameObject prefab)
    {
        if (tilePositions.Count == 0) return; // 如果没有位置了，就不再生成
        int randomIndex = Random.Range(0, tilePositions.Count);
        Vector3Int randomPosition = tilePositions[randomIndex];
        tilePositions.RemoveAt(randomIndex); // 移除已选位置，避免重复
        // 转换Tilemap坐标到世界坐标，并生成Prefab
        Vector3 worldPosition = tilemap.CellToWorld(randomPosition) + new Vector3(0.5f, 0.5f, 0);
        Instantiate(prefab, worldPosition, Quaternion.identity);
    }*/

    private void OnEnable()
    {
        EnemyManager.OnEnemyKilled += UpdateEnemyCount;
    }

    private void OnDisable()
    {
        EnemyManager.OnEnemyKilled -= UpdateEnemyCount;
    }

    void UpdateEnemyCount(EnemyType enemyType)
    {
        // 使用enemyType来区分是哪一种敌人被杀死，并相应地更新计数
        if (enemyType == EnemyType.Melee)
        {
            MeleeEnemy_count--;
            // 检查是否需要生成新的近战敌人
            StartCoroutine(MeleeEnemyCoroutine());
        }
        else if (enemyType == EnemyType.Ranged)
        {
            RangedEnemy_count--;
            StartCoroutine (RangedEnemyCoroutine());
            
            // 检查是否需要生成新的远程敌人
        }

        // ...生成逻辑...
    }
    IEnumerator MeleeEnemyCoroutine()
    {

        yield return new WaitForSeconds(2);
        RandomPrefabAroundPlayer(prefabs[2]);
        Debug.Log("生成近战敌人");
    }
    IEnumerator RangedEnemyCoroutine()
    {
        yield return new WaitForSeconds(4);
        RandomPrefabAroundPlayer(prefabs[3]);
        Debug.Log("生成远程敌人");
    }
}
