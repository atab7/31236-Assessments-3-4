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
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        scoreKeeper = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TextMeshProUGUI>();
        ghostTimer = GameObject.FindGameObjectWithTag("GhostTimer").GetComponent<TextMeshProUGUI>();
        gameTimer = GameObject.FindGameObjectWithTag("GameTimer").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        gameTimer.text = "TIME\n" + TimeSpan.FromSeconds(timer).ToString("mm':'ss':'fff");


    }

    public void ChangeScore(int score)
    {
        scoreKeeper.text = "SCORE\n" + score.ToString(); 
    }

    public void ShowRemainingScaredGhostTime(float time)
    {
        ghostTimer.text = "SCARED\n" + TimeSpan.FromSeconds(time).ToString("ss':'fff");
    }
}
