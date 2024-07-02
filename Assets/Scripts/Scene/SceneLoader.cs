using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    // ���ڱ�ǵ�ǰ�����Ƿ��ǵڶ�������
    public bool isSecondScene = false;
    public void Home()
    {
        SceneManager.LoadSceneAsync("Home");
    }
    public void Rank()
    {
        SceneManager.LoadSceneAsync("Rank");
    }
    public void End()
    {
        SceneManager.LoadSceneAsync("End");
    }
    public void Map1()
    {
        SceneManager.LoadSceneAsync("Map1");
    }
    public void Map2()
    {
        SceneManager.LoadSceneAsync("Map2");
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void GameOverPage()
    {
        SceneManager.LoadSceneAsync("Over");
    }
}
