using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;
public class destroyobstacles : MonoBehaviour, IDamageable
{
    private float maxHealth=3;
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //ani.SetTrigger("Hit");
        Debug.Log("destructible obstacles，剩余血量：" + currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("destructible obstacle损坏。");
            //EnemyManager.EnemyKilled();
            //StartCoroutine(die_ani());
            Destroy(gameObject);
            RescanPathfindingGraph();
            // 此处添加玩家死亡的处理逻辑

        }
    }
    public void RescanPathfindingGraph()
    {
        AstarPath.active.Scan();
    }
}
