using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pausePanel; // 指向暂停界面Panel的引用
    private bool isPaused = true; // 当前暂停状态

    private void Start()
    {
       //pausePanel.SetActive(false); // 初始时隐藏暂停界面
    }

    private void Update()
    {

        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pausePanel.SetActive(true);
                isPaused = false;
                Time.timeScale = 0;
            }
        }else if (Input.GetKeyDown(KeyCode.Space))
        {
            pausePanel.SetActive(false);
            isPaused = true;
            Time.timeScale = 1;
        }
    }


    // 如果需要，添加其他UI控件调用的方法，比如继续游戏或退出游戏
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        isPaused = true;
        Time.timeScale = 1;
    }
    public void Retart()
    {
       SceneLoader.instance.Map1();
       Time.timeScale = 1;
        
    }
    public void Home()
    {
        SceneLoader.instance.Home();
    }
}
