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
        // �����ײ�Ķ����Ƿ������
        if (collider.gameObject.CompareTag("Player")|| collider.gameObject.CompareTag("DestructibleObstacles"))
        {
            Debug.Log("������" + collider.name);
            // ������ҵ�GameObject��һ����ǩ��"Player"
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(1); // ��ҿ۳�һ��Ѫ
            }
            Destroy(gameObject); // �����ӵ�
        }else if (collider.gameObject.CompareTag("IndestructibleObstacles"))
        {
            Destroy(gameObject);
        }
    }
}
