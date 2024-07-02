using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2D1;
    void Awake()//这里初始化必需要用Awake！！！！因为在Player中要实例化它，如果用start会出现未实例化的错误
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
        //这里是判断子弹是否击中敌人
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

