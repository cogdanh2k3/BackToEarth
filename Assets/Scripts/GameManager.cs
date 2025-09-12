using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

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
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isStarted = e.state == Lander.State.Normal;
    }

    private void Update()
    {
        if (isStarted)
        {
            time += Time.deltaTime;
        }
    }
    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AddScore(500);
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

}
