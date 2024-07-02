using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    //public static PlayerController instance;
    //public int CurrentHealth => currentHealth;
    public float speed = 5f; // 角色移动速度
    private Rigidbody2D player; // Rigidbody2D组件的引用
    private Animator animator; // Animator组件的引用
    private Vector2 direction;
    private int role_direction = 1;
    //Melee_Attack
    public float cooldownTime = 2.0f; // 冷却时间，单位为秒
    private float nextAllowedTime = 0.0f; // 下一次允许操作的时间
    public float attackRangeX = 3f; // 攻击范围的宽度
    public float attackRangeY = 3f; // 攻击范围的高度
    public GameObject attackEffectPrefab;// 攻击的特效
    //Ranged_Attack
    public GameObject bulletPrefab; // 子弹的Prefab
    public Transform bulletSpawnPoint; // 子弹生成的位置
    public float bulletSpeed = 2f; // 子弹的速度
    public float shootingCooldown = 0.5f; // 射击的冷却时间
    private float nextShootTime = 0f; // 下一次射击的时间点
    //角色血量
    private int maxHealth = 30;
    private int currentHealth;



    void Start()
    {
        // 获取附加在同一个GameObject上的Rigidbody2D组件
        player = GetComponent<Rigidbody2D>();
        // 获取Animator组件
        animator = GetComponent<Animator>();
        //将当前血量设置为最大血量
        currentHealth = maxHealth;

    }

    void Update()
    {
        Moving();
        Attack();
    }
    void Moving()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 创建并更新角色的当前位置向量
        Vector2 movement = new Vector2(moveX, moveY);
        if (movement.x != 0 || movement.y != 0)
        {
            direction = movement;
        }
        Vector2 moveVelocity = Vector2.zero;
        animator.SetBool("isRun", false);

        // 根据玩家输入来决定角色移动和动画
        if (moveX<0 || moveY < 0)
        {
            role_direction = -1;
            moveVelocity = Vector2.left;
            transform.localScale = new Vector3(role_direction, 1, 1);
            animator.SetBool("isRun", true);

        }
        if (moveX > 0 || moveY > 0)
        {
            role_direction = 1;
            moveVelocity = Vector2.right;
            transform.localScale = new Vector3(role_direction, 1, 1);
            animator.SetBool("isRun", true);
        }

        Vector2 position = player.position;
        position += movement * speed * Time.fixedDeltaTime;
        player.MovePosition(position);
    }

    void Attack()
    {

        if (Input.GetMouseButtonDown(1) && Time.time >= nextAllowedTime)
        {

            animator.SetTrigger("attack");
            MeleeAttack();
            MeleeAttackAudio();
            HealthManager.instance.Melee_Cooldown();
            nextAllowedTime = Time.time + cooldownTime;
        }
        if (Input.GetMouseButtonDown(0) && Time.time >= nextShootTime)
        {
            animator.SetTrigger("attack");
            RangedAttack();
            RangedAttackAudio();
            HealthManager.instance.Ranged_Cooldown();
            nextShootTime = Time.time + shootingCooldown;
        }
    }
    void MeleeAttack()
    {
        GameObject effect = Instantiate(attackEffectPrefab, gameObject.transform.position+ new Vector3(0,0.5f,0), Quaternion.identity);
        effect.transform.SetParent(transform);
        Animator effect_ani = effect.GetComponent<Animator>();
        Effect attack = effect.GetComponent<Effect>();
        if (attack != null)
        {
            attack.Initiator = this;
        }
        if (effect_ani != null)
        {
            // 计算动画时长
            float animationLength = effect_ani.GetCurrentAnimatorStateInfo(0).length;

            // 动画播放完毕后销毁特效
            Destroy(effect, animationLength);
        }
    }
    void RangedAttack()
    {
        Vector2 shootingDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;
        if (shootingDirection == Vector2.zero) // 如果没有水平输入，使用角色最后的朝向
        {
            shootingDirection = new Vector2(role_direction, 0);
        }

        // 设置子弹的生成位置在角色前方
        Vector3 spawnPosition = transform.position + new Vector3(shootingDirection.x, 0.5f, 0); // 设置子弹在角色前方的位置

        // 创建子弹实例
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        // 设置子弹图像朝向
        bullet.transform.localScale = new Vector3(role_direction, 1, 1);

        // 给子弹一个初速度
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.velocity = shootingDirection * bulletSpeed; // 子弹沿最后的水平方向发射
        }

        // 可选：设置一定时间后销毁子弹
       
    }
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        PlayerManager.instance.UpdateHealth(currentHealth);
        HealthManager.instance.UpdateHealth(currentHealth, maxHealth);
        animator.SetTrigger("hurt");
        Debug.Log("玩家受到伤害，剩余血量：" + currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("玩家死亡。");
           
            StartCoroutine( Die());
            // 此处添加玩家死亡的处理逻辑
        }
    }
    IEnumerator Die()
    {
        animator.SetTrigger("die");
        yield return new WaitForSeconds(1);
        SceneLoader.instance.GameOverPage();

    }

    void OnColliderEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("MeleeEnemy") || collider.gameObject.CompareTag("DestructibleObstacles")|| collider.gameObject.CompareTag("Enemy"))
        {
            // 处理攻击逻辑
            Debug.Log("攻击到了敌人：" + collider.name);
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // 调用TakeDamage方法
                damageable.TakeDamage(1);
            }
        }
    }

    #region Audio System

    public AudioClip RangedAttackSound1; // 第一种攻击的音效
    public AudioClip MeleeAttackSound2; // 第二种攻击的音效
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    public void RangedAttackAudio()
    {
        audioSource.clip = RangedAttackSound1;
        audioSource.Play();
    }

    public void MeleeAttackAudio()
    {
        audioSource.clip = MeleeAttackSound2;
        audioSource.Play();
    }
    #endregion


}
