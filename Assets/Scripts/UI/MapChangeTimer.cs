using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapChangeTimer : MonoBehaviour
{
    public float delayInSeconds = 30f; // 延迟时间（秒）
    public Text countdownText; // 倒计时文本的引用
    // Start is called before the first frame update
    private void Start()
    {
        // 检测当前加载的场景，如果是第二个场景，那么isSecondScene为true
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneLoader.instance.isSecondScene = true;
        }

        // 如果存在倒计时Text，开始倒计时
        if (countdownText != null)
            StartCoroutine(UpdateCountdown());

        // 开始加载场景的协程
        StartCoroutine(LoadSceneAfterDelay(delayInSeconds));
    }
    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 根据当前是哪个场景来决定加载哪个场景
        if (SceneLoader.instance.isSecondScene)
        {
            SceneLoader.instance.GameOverPage();
        }
        else
        {
            SceneLoader.instance.Map2();
        }
    }

    private IEnumerator UpdateCountdown()
    {
        float countdown = delayInSeconds;
        while (countdown > 0)
        {
            countdownText.text =countdown.ToString(); // 显示倒计时，保留两位小数
            yield return new WaitForSeconds(1f); // 等待一秒
            countdown -= 1f;
        }
    }
}
