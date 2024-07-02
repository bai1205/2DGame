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
        // �˳���Ϸ
        Application.Quit();

        // �����Unity�༭���У���ӡһ����Ϣ
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
