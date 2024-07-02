using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    //public static PlayerController instance;
    //public int CurrentHealth => currentHealth;
    public float speed = 5f; // ��ɫ�ƶ��ٶ�
    private Rigidbody2D player; // Rigidbody2D���������
    private Animator animator; // Animator���������
    private Vector2 direction;
    private int role_direction = 1;
    //Melee_Attack
    public float cooldownTime = 2.0f; // ��ȴʱ�䣬��λΪ��
    private float nextAllowedTime = 0.0f; // ��һ�����������ʱ��
    public float attackRangeX = 3f; // ������Χ�Ŀ��
    public float attackRangeY = 3f; // ������Χ�ĸ߶�
    public GameObject attackEffectPrefab;// ��������Ч
    //Ranged_Attack
    public GameObject bulletPrefab; // �ӵ���Prefab
    public Transform bulletSpawnPoint; // �ӵ����ɵ�λ��
    public float bulletSpeed = 2f; // �ӵ����ٶ�
    public float shootingCooldown = 0.5f; // �������ȴʱ��
    private float nextShootTime = 0f; // ��һ�������ʱ���
    //��ɫѪ��
    private int maxHealth = 30;
    private int currentHealth;



    void Start()
    {
        // ��ȡ������ͬһ��GameObject�ϵ�Rigidbody2D���
        player = GetComponent<Rigidbody2D>();
        // ��ȡAnimator���
        animator = GetComponent<Animator>();
        //����ǰѪ������Ϊ���Ѫ��
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

        // ���������½�ɫ�ĵ�ǰλ������
        Vector2 movement = new Vector2(moveX, moveY);
        if (movement.x != 0 || movement.y != 0)
        {
            direction = movement;
        }
        Vector2 moveVelocity = Vector2.zero;
        animator.SetBool("isRun", false);

        // �������������������ɫ�ƶ��Ͷ���
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
            // ���㶯��ʱ��
            float animationLength = effect_ani.GetCurrentAnimatorStateInfo(0).length;

            // ����������Ϻ�������Ч
            Destroy(effect, animationLength);
        }
    }
    void RangedAttack()
    {
        Vector2 shootingDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;
        if (shootingDirection == Vector2.zero) // ���û��ˮƽ���룬ʹ�ý�ɫ���ĳ���
        {
            shootingDirection = new Vector2(role_direction, 0);
        }

        // �����ӵ�������λ���ڽ�ɫǰ��
        Vector3 spawnPosition = transform.position + new Vector3(shootingDirection.x, 0.5f, 0); // �����ӵ��ڽ�ɫǰ����λ��

        // �����ӵ�ʵ��
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        // �����ӵ�ͼ����
        bullet.transform.localScale = new Vector3(role_direction, 1, 1);

        // ���ӵ�һ�����ٶ�
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.velocity = shootingDirection * bulletSpeed; // �ӵ�������ˮƽ������
        }

        // ��ѡ������һ��ʱ��������ӵ�
       
    }
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        PlayerManager.instance.UpdateHealth(currentHealth);
        HealthManager.instance.UpdateHealth(currentHealth, maxHealth);
        animator.SetTrigger("hurt");
        Debug.Log("����ܵ��˺���ʣ��Ѫ����" + currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("���������");
           
            StartCoroutine( Die());
            // �˴������������Ĵ����߼�
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
            // �������߼�
            Debug.Log("�������˵��ˣ�" + collider.name);
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // ����TakeDamage����
                damageable.TakeDamage(1);
            }
        }
    }

    #region Audio System

    public AudioClip RangedAttackSound1; // ��һ�ֹ�������Ч
    public AudioClip MeleeAttackSound2; // �ڶ��ֹ�������Ч
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
