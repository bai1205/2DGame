using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Events;
using System;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    /*    public int score = 0;
        public static ScoreManager instance { get; private set; }
        public Text scoreText; // Unity UI命名空间下的Text

        private string scoreFilePath;
        private void Awake()
        {
            // 确保可以在其他脚本中访问得分管理器
            if (instance == null)
            {
                instance = this;
                 scoreFilePath = Path.Combine(Application.persistentDataPath, "scores.txt"); // 设置得分文件的路径
                DontDestroyOnLoad(gameObject);
            }

        }
        public void AddScore(int points)
        {
            score += points;
            UpdateScoreDisplay();
        }

        private void UpdateScoreDisplay()
        {
            scoreText.text = "Score: " + score;
            // 这里更新得分显示，例如UI文本
            Debug.Log("Current Score: " + score);
        }
        public void ResetScore()
        {
            score = 0;
            UpdateScoreDisplay();
        }
        public void DestroyThisObject()
        {
            if (instance == this)
            {
                GameObject.Find("Point").SetActive(false); 
                instance = null;
            }
        }
        public void Endgame()
        {
            SaveScore();
            score = 0;
        }
        private void SaveScore()
        {
            using (StreamWriter w = new StreamWriter(scoreFilePath,true)) 
            {
                w.WriteLine(score);
            }
        }*/

    public int Score => score;
    int score;
    private void Start()
    {
        Debug.Log(score);
    }
    public void Reset()
    {
        //score = 0;
        ScoreDisplay.UpdateText(score);
    }
    public void AddScore(int scorepoint)
    {
        score += scorepoint;
        ScoreDisplay.UpdateText(score);
    }


    #region  High Score System
    [System.Serializable]
    public class PlayScore
    {
        public int score;
        public string name;

        public PlayScore(int score, string name)
        {
            this.score = score;
            this.name = name;
        }

    }
    [System.Serializable] public class PlayerScoreData
    {
        public List<PlayScore> list = new List<PlayScore>();

    }
    readonly string SaveFileName = "player_score.json";
    string playerName = "No Record";

    public void SetPlayerName()
    {
        playerName = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }

    public bool HasNewHighScore => score > LoadPlayerScoreData().list[9].score;
    public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();
        playerScoreData.list.Add(new PlayScore(score, playerName));
        playerScoreData.list.Sort((x, y) => y.score.CompareTo(x.score));//降序
        
        SaveSystem.Save(SaveFileName, playerScoreData);

    }


    public PlayerScoreData LoadPlayerScoreData()
    {
        var playerScoreData = new PlayerScoreData();

        if (SaveSystem.SaveFileExists(SaveFileName))
        {
            playerScoreData = SaveSystem.Load<PlayerScoreData>(SaveFileName);
        }
        else
        {
            while (playerScoreData.list.Count< 10)
            {
                playerScoreData.list.Add(new PlayScore(0, playerName));
            }
            SaveSystem.Save(SaveFileName, playerScoreData);
        }
        return playerScoreData;
    }


    #endregion


}

