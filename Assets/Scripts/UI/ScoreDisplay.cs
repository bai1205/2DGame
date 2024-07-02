using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    // Start is called before the first frame update
      static Text scoreText;
     void Awake()
    {
        scoreText = GetComponent<Text>();
    }
     void Start()
    {
        ScoreManager.instance.Reset();
    }
    public static void UpdateText(int score)
    {
        scoreText.text=score.ToString();
    }
    public void BacktoMenu()
    {
        SceneLoader.instance.Home();
    }
    public void Restart()
    {
        SceneLoader.instance.Map1();
    }
}
