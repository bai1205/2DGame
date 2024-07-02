using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : PersistentSingleton<PlayerManager>
{
    public int playerCurrentHealth;
    public void UpdateHealth(int currentHealth)
    {
        playerCurrentHealth = currentHealth;
    }
}
