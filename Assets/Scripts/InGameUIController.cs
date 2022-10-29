using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InGameUIController : MonoBehaviour
{
    private TextMeshProUGUI scoreKeeper;
    private TextMeshProUGUI ghostTimer;
    private TextMeshProUGUI gameTimer;
    private TextMeshProUGUI counter;
    private float timer;
    private bool keepTime = true;
    public bool go = false;
    private AudioSource camAudio;

    // Start is called before the first frame update
    void Start()
    {
        counter = GameObject.FindGameObjectWithTag("Counter").GetComponent<TextMeshProUGUI>();
        counter.text = "3";
        scoreKeeper = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TextMeshProUGUI>();
        ghostTimer = GameObject.FindGameObjectWithTag("GhostTimer").GetComponent<TextMeshProUGUI>();
        gameTimer = GameObject.FindGameObjectWithTag("GameTimer").GetComponent<TextMeshProUGUI>();
        camAudio = Camera.main.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (keepTime)
            timer += Time.deltaTime;

        if (!go)
            CountDown();
        else
        {
            gameTimer.text = "TIME\n" + TimeSpan.FromSeconds(timer).ToString("mm':'ss':'fff");
        }
    }

    void CountDown()
    {
        if (timer >= 4f)
        {
            counter.text = "";
            counter.enabled = false;
            timer = 0;
            go = true;
            camAudio.Play();
        }
        else if (timer >= 3f)
            counter.text = "GO!";
        else if (timer >= 2f)
            counter.text = "1";
        else if (timer >= 1f)
            counter.text = "2";
    }


    public void StopTimer()
    {
        keepTime = false;
    }

    public void ChangeScore(int score)
    {
        scoreKeeper.text = "SCORE\n" + score.ToString(); 
    }

    public void ShowRemainingScaredGhostTime(float time)
    {
        ghostTimer.text = "SCARED\n" + TimeSpan.FromSeconds(time).ToString("ss':'fff");
    }

    public void ShowGameOver()
    {
        counter.enabled = true;
        counter.text = "GAME OVER!";
    }

    public void UpdateTopTime()
    {
        float prevTopTime = PlayerPrefs.GetFloat("PacStu-TopTime", 0f);
        if (timer > prevTopTime)
        {
            PlayerPrefs.SetFloat("PacStu-TopTime", timer);
            PlayerPrefs.Save();
        }
    }
}
