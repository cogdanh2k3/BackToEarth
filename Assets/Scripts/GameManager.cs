using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private static int levelNumber = 1;
    [SerializeField] private List<GameLevel> gameLevelList;

    private int score;
    private float time;
    private bool isStarted;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Lander.instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.instance.OnLanded += Lander_OnLanded;
        Lander.instance.OnStateChanged += Lander_OnStateChanged;

        LoadCurrentLevel();
    }
    private void Update()
    {
        if (isStarted)
        {
            time += Time.deltaTime;
        }
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isStarted = e.state == Lander.State.Normal;
    }

    
    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AddScore(500);
    }

    public void LoadCurrentLevel()
    {
        foreach(GameLevel level in gameLevelList)
        {
            if (level.GetLevel() == levelNumber)
            {
                GameLevel spawnedGameLevel = Instantiate(level, Vector3.zero, Quaternion.identity);
                Lander.instance.transform.position = spawnedGameLevel.GetLanderSpawnPosition();
            }
        }
    }

    public void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;
        Debug.Log(score);
    }

    public int GetScore()
    {
        return score;
    }

    public float GetTime()
    {
        return time;
    }

    public void GoToNextLevel()
    {
        levelNumber++;
        SceneManager.LoadScene(0);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(0);
    }
    
    public int GetLevelNumber()
    {
        return levelNumber;
    }
}
