using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pausePanel; // ָ����ͣ����Panel������
    private bool isPaused = true; // ��ǰ��ͣ״̬

    private void Start()
    {
       //pausePanel.SetActive(false); // ��ʼʱ������ͣ����
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


    // �����Ҫ���������UI�ؼ����õķ��������������Ϸ���˳���Ϸ
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
