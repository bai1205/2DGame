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
    public Tilemap tilemap; // �����е�Tilemap����
    public GameObject[] prefabs; // Ҫ���ɵ�Prefabs����
    public int numberOfPrefabsToSpawn = 15; // ����Prefabs������
    private List<Vector3Int> tilePositions = new List<Vector3Int>();

    public float initial_MeleeEnemy = 15; // ����ʱ���ɵĵ�������
    public float initial_RangedEnemy = 5;
    //public float spawnInterval; // ����Prefabs��ʱ��������λ����\
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
        SpawnPrefabAvoidingPlayer(prefab, playerPositionOnTilemap, 2); // 2�Ǽ��뾶����Ӧ5x5��Χ
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
        tilePositions.Remove(randomPosition); // ��ѡ������㲻���ٴ������λ������

        Vector3 worldPosition = tilemap.CellToWorld(randomPosition) + new Vector3(0.5f, 0.5f, 0);
        Instantiate(prefab, worldPosition, Quaternion.identity);
    }
 /*   public void Random_Prefab(GameObject prefab)
    {
        if (tilePositions.Count == 0) return; // ���û��λ���ˣ��Ͳ�������
        int randomIndex = Random.Range(0, tilePositions.Count);
        Vector3Int randomPosition = tilePositions[randomIndex];
        tilePositions.RemoveAt(randomIndex); // �Ƴ���ѡλ�ã������ظ�
        // ת��Tilemap���굽�������꣬������Prefab
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
        // ʹ��enemyType����������һ�ֵ��˱�ɱ��������Ӧ�ظ��¼���
        if (enemyType == EnemyType.Melee)
        {
            MeleeEnemy_count--;
            // ����Ƿ���Ҫ�����µĽ�ս����
            StartCoroutine(MeleeEnemyCoroutine());
        }
        else if (enemyType == EnemyType.Ranged)
        {
            RangedEnemy_count--;
            StartCoroutine (RangedEnemyCoroutine());
            
            // ����Ƿ���Ҫ�����µ�Զ�̵���
        }

        // ...�����߼�...
    }
    IEnumerator MeleeEnemyCoroutine()
    {

        yield return new WaitForSeconds(2);
        RandomPrefabAroundPlayer(prefabs[2]);
        Debug.Log("���ɽ�ս����");
    }
    IEnumerator RangedEnemyCoroutine()
    {
        yield return new WaitForSeconds(4);
        RandomPrefabAroundPlayer(prefabs[3]);
        Debug.Log("����Զ�̵���");
    }
}
