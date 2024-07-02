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
    // �޸��¼��Ա�������һ��EnemyType����
    public static event Action<EnemyType> OnEnemyKilled;

    public static void EnemyKilled(EnemyType enemyType)
    {
        // ʹ�ô����EnemyType���������¼�
        OnEnemyKilled?.Invoke(enemyType);
    }
}

