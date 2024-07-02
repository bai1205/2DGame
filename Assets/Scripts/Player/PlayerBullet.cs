using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2D1;
    void Awake()//�����ʼ������Ҫ��Awake����������Ϊ��Player��Ҫʵ�������������start�����δʵ�����Ĵ���
    {
        rigidbody2D1 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
    public void Move(Vector2 moveDirection, float moveForce)
    {
        rigidbody2D1.AddForce(moveDirection * moveForce);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject enemy = collision.gameObject;
        IDamageable damageable = collision.GetComponent<IDamageable>();
        //�������ж��ӵ��Ƿ���е���
        if (collision.gameObject.tag == "MeleeEnemy" || collision.gameObject.tag == "DestructibleObstacles" || collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            damageable.TakeDamage(1);
        }else if(collision.gameObject.tag == "IndestructibleObstacles")
        {
            Destroy(gameObject);
        }
    }
}

