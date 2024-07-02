using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    public static HealthManager instance {  get; private set; }
     // Start is called before the first frame update\
    public Image HealthBar;
    public Text HealthText;

    public Image Melee_cooldownMask; // 冷却遮罩的Image组件
    public float Melee_cooldownTime = 2f; // 冷却时间，假设为5秒

    private float Melee_cooldownTimer; // 当前冷却的计时器

    public Image Ranged_cooldownMask; // 冷却遮罩的Image组件
    public float Ranged_cooldownTime = 0.5f; // 冷却时间，假设为5秒
    private float Ranged_cooldownTimer; // 当前冷却的计时器

    void Awake()
    {
        instance = this; 
    }
    public void UpdateHealth(int currentHealth, int maxHealth )
    {
        HealthBar.fillAmount = (float)currentHealth / (float)maxHealth;
        HealthText.text = currentHealth + " / " + maxHealth;
    }

    void Update()
    {
        if (Melee_cooldownTimer > 0)
        {
            Melee_cooldownTimer -= Time.deltaTime;
            Melee_cooldownMask.fillAmount = Melee_cooldownTimer / Melee_cooldownTime;
        }
        if (Ranged_cooldownTimer > 0)
        {
            Ranged_cooldownTimer -= Time.deltaTime;
            Ranged_cooldownMask.fillAmount = Ranged_cooldownTimer / Ranged_cooldownTime;
        }
    }

    // 被外部调用，开始冷却计时
    public void Melee_Cooldown()
    {
        Melee_cooldownTimer = Melee_cooldownTime;
        Melee_cooldownMask.fillAmount = 1;
    }
    public void Ranged_Cooldown()
    {
        Ranged_cooldownTimer = Ranged_cooldownTime;
        Ranged_cooldownMask.fillAmount = 1;
    }
}
