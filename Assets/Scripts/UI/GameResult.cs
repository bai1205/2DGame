using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    public GameObject Thanks;
    public GameObject Failed;
    // Start is called before the first frame update
    void Start()
    {
        Result();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Result()
    {
        if (PlayerManager.instance.playerCurrentHealth > 0)
        {
            Thanks.SetActive(true);

        }
        else
        {
            Failed.SetActive(true);
        }
    }
}
