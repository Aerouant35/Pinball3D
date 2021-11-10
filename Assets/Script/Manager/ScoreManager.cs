using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    public Text scoreDisplayer;

    [SerializeField] 
    private Text[] leaderboard;

    private int CurrentScore;

    private int HighScore1;
    private int HighScore2;
    private int HighScore3;
    
    private void Awake()
    {
        if (!ReferenceEquals(Instance, null))
            DestroyImmediate(this);
        else
            Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (leaderboard.Length <= 0) return;
        
        LoadHighScore();
    }
    
    private void LoadHighScore()
    {
        HighScore1 = PlayerPrefs.GetInt("HighScore1");
        HighScore2 = PlayerPrefs.GetInt("HighScore2");
        HighScore3 = PlayerPrefs.GetInt("HighScore3");


        leaderboard[0].text = HighScore1.ToString();
        leaderboard[1].text = HighScore2.ToString();
        leaderboard[2].text = HighScore3.ToString();
    }

    public void SaveHighScore()
    {
        if(CurrentScore > HighScore3)
        {
            HighScore3 = CurrentScore;

            if(CurrentScore > HighScore2)
            {
                HighScore3 = HighScore2;
                HighScore2 = CurrentScore;

                if(CurrentScore > HighScore1)
                {
                    HighScore2 = HighScore1;
                    HighScore1 = CurrentScore;
                }
            }
        }
        PlayerPrefs.SetInt("HighScore1", HighScore1);
        PlayerPrefs.SetInt("HighScore2", HighScore2);
        PlayerPrefs.SetInt("HighScore3", HighScore3);

        LoadHighScore();
    }

    /// <summary>
    /// Add score
    /// </summary>
    /// <param name="score">Score to add</param>
    public void AddScore(int score)
    {
        CurrentScore += score;
        GameManager.Instance.ScoreDisplayer = CurrentScore.ToString();
    }
}
