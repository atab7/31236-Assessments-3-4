using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreCollector : MonoBehaviour
{
    private TextMeshProUGUI highScore;
    private TextMeshProUGUI topTime;

    // Start is called before the first frame update
    void Start()
    {
        highScore = GameObject.FindGameObjectWithTag("HighScore").GetComponent<TextMeshProUGUI>();
        topTime = GameObject.FindGameObjectWithTag("TopTime").GetComponent<TextMeshProUGUI>();
        
        highScore.text = "HIGH SCORE\n" +  PlayerPrefs.GetInt("PacStu-HighScore", 0).ToString();
        
        float topFTime = PlayerPrefs.GetFloat("PacStu-TopTime", 0f);
        topTime.text = "TIME\n" + TimeSpan.FromSeconds(topFTime).ToString("mm':'ss':'fff");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
