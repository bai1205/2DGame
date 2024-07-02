using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        // 检查碰撞的对象是否是玩家
        if (collider.gameObject.CompareTag("Player")|| collider.gameObject.CompareTag("DestructibleObstacles"))
        {
            Debug.Log("名字是" + collider.name);
            // 假设玩家的GameObject有一个标签叫"Player"
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(1); // 玩家扣除一滴血
            }
            Destroy(gameObject); // 销毁子弹
        }else if (collider.gameObject.CompareTag("IndestructibleObstacles"))
        {
            Destroy(gameObject);
        }
    }
}
