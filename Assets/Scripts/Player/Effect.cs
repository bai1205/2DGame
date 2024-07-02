using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 boxSize = new Vector2(3f, 3f); // 3x3范围的大小
    private Vector2 attackpoi;
    // Start is called before the first frame update
    private Animator animator; // 引用Animator组件
    public float interval = 3f; // 动画播放间隔，这里设为3秒
    private Collider2D[] colliders;
    public PlayerController Initiator { get; set; }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void MeleeAttackAnimEvent()
    {
        attackpoi = Initiator.transform.position;
        animator = GetComponent<Animator>();
        colliders = Physics2D.OverlapBoxAll(attackpoi, new Vector2(3f, 3f), 0);
        foreach (var collider in colliders)
        {
            // 输出控制台信
            OnColliderEnter2D(collider);
        }
    }
    void OnColliderEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("MeleeEnemy") || collider.gameObject.CompareTag("DestructibleObstacles") || collider.gameObject.CompareTag("Enemy"))
        {
            // 处理攻击逻辑
            Debug.Log("攻击到了敌人：" + collider.name);
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // 调用TakeDamage方法
                damageable.TakeDamage(2);
            }
        }
    }
}
