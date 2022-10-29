using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    private TextMeshProUGUI scoreKeeper;
    private TextMeshProUGUI ghostTimer;
    private TextMeshProUGUI gameTimer;
    private TextMeshProUGUI counter;
    public float timer = 0f;
    private bool keepTime = true;
    public bool go = false;
    private AudioSource camAudio;
    public Sprite[] lifeIndicatorSprites;
    private Image[] indicatorImageRefs;
    int indicatorIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        counter = GameObject.FindGameObjectWithTag("Counter").GetComponent<TextMeshProUGUI>();
        counter.text = "3";
        scoreKeeper = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TextMeshProUGUI>();
        ghostTimer = GameObject.FindGameObjectWithTag("GhostTimer").GetComponent<TextMeshProUGUI>();
        gameTimer = GameObject.FindGameObjectWithTag("GameTimer").GetComponent<TextMeshProUGUI>();
        camAudio = Camera.main.GetComponent<AudioSource>();
        
        GameObject[] lifeIndicators = GameObject.FindGameObjectsWithTag("LifeIndicator");
        indicatorImageRefs = new Image[lifeIndicators.Length];
        for (int i = 0; i < lifeIndicators.Length; i++)
        {
            indicatorImageRefs[i] = lifeIndicators[i].GetComponent<Image>();
        }
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

    public void UpdateLifeIndicator()
    {
        if (indicatorIndex < lifeIndicatorSprites.Length)
        {
            for (int i = 0; i < indicatorImageRefs.Length; i++)
            {
                indicatorImageRefs[i].sprite = lifeIndicatorSprites[indicatorIndex];
            }
            indicatorIndex++;
        }
        else if (indicatorIndex == lifeIndicatorSprites.Length)
        {
            for (int i = 0; i < indicatorImageRefs.Length; i++)
            {
                indicatorImageRefs[i].enabled = false;
            }
            indicatorIndex++;
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

}
