using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScene : MonoBehaviour
{
    public GameObject info;
  public void StartMenu()
    {
        SceneLoader.instance.Map1();
    }
    public void ExitGame()
    {
        // 退出游戏
        Application.Quit();

        // 如果在Unity编辑器中，打印一条消息
#if UNITY_EDITOR
        Debug.Log("Exit button clicked. Game would quit if this were a build.");
#endif
    }
    public void About()
    {
        info.SetActive(true);
    }
    public void Close()
    {
        info.SetActive(false);
    }
    public void Ranking()
    {
        SceneLoader.instance.Rank();
    }
}
