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
        Debug.Log("destructible obstacles��ʣ��Ѫ����" + currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("destructible obstacle�𻵡�");
            //EnemyManager.EnemyKilled();
            //StartCoroutine(die_ani());
            Destroy(gameObject);
            RescanPathfindingGraph();
            // �˴������������Ĵ����߼�

        }
    }
    public void RescanPathfindingGraph()
    {
        AstarPath.active.Scan();
    }
}
