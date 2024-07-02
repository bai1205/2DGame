using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapChangeTimer : MonoBehaviour
{
    public float delayInSeconds = 30f; // �ӳ�ʱ�䣨�룩
    public Text countdownText; // ����ʱ�ı�������
    // Start is called before the first frame update
    private void Start()
    {
        // ��⵱ǰ���صĳ���������ǵڶ�����������ôisSecondSceneΪtrue
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneLoader.instance.isSecondScene = true;
        }

        // ������ڵ���ʱText����ʼ����ʱ
        if (countdownText != null)
            StartCoroutine(UpdateCountdown());

        // ��ʼ���س�����Э��
        StartCoroutine(LoadSceneAfterDelay(delayInSeconds));
    }
    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ���ݵ�ǰ���ĸ����������������ĸ�����
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
            countdownText.text =countdown.ToString(); // ��ʾ����ʱ��������λС��
            yield return new WaitForSeconds(1f); // �ȴ�һ��
            countdown -= 1f;
        }
    }
}
