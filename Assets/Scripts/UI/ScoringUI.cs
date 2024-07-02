using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoringUI : MonoBehaviour
{
    [SerializeField] Transform highScoreLeaderContainer;
    void Start()
    {
        if (ScoreManager.instance.HasNewHighScore && PlayerManager.instance.playerCurrentHealth>0)
        {
            ScoreManager.instance.SetPlayerName();
            ScoreManager.instance.SavePlayerScoreData();
            ShowingScoringScreen();
        }
        else
        {
            ShowingScoringScreen();
        }
    }


    void ShowingScoringScreen()
    {
        //playerScoreText.text = ScoreManager.instance.Score.ToString();
        UpdateHighScoreLeaderboard();

    }
    void UpdateHighScoreLeaderboard()
    {
        var playerScoreList = ScoreManager.instance.LoadPlayerScoreData().list;
        for (int i = 0; i < highScoreLeaderContainer.childCount; i++)
        {
            var child = highScoreLeaderContainer.GetChild(i);
            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].name;
        }

    }
    public void Back()
    {
        SceneLoader.instance.Home();
    }
}
