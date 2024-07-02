using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Melee,
    Ranged
}

public class EnemyManager : MonoBehaviour
{
    // 修改事件以便它接受一个EnemyType参数
    public static event Action<EnemyType> OnEnemyKilled;

    public static void EnemyKilled(EnemyType enemyType)
    {
        // 使用传入的EnemyType参数调用事件
        OnEnemyKilled?.Invoke(enemyType);
    }
}

