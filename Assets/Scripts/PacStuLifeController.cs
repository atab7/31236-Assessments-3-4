using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PacStuLifeController : MonoBehaviour
{
    // Start is called before the first frame update
    public int lives { get; private set; }
    private Animator pacStuAnimator;
    private InGameUIController uiController;
    private int pelletsLeft = 220;
    private AudioSource audioSource;
    public bool isGameOver = false;
    [SerializeField] AudioClip gameOverMusic;
    private float timer = 0;
    private PelletEater eater;
    private Vector3 targetPosition = new Vector3(1792f, 384f, 0);
    private Vector3 startPosition;
    private Vector3 startScale;
    private Vector3 targetScale = new Vector3(100, 100, 0);
    private float elapsedTime = 0f;
    private const float duration = 2f;
    private bool deathAnimationPlayed = false;
    private bool displayGameOver = false;

    void Start()
    {
        startScale = transform.localScale;
        lives = 4;
        pacStuAnimator = GetComponent<Animator>();
        uiController = GameObject.FindGameObjectWithTag("HUD").GetComponent<InGameUIController>();
        audioSource = Camera.main.GetComponent<AudioSource>();
        eater = GetComponent<PelletEater>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pelletsLeft <= 0 || lives <= 0)
            GameOver();
    }

    public void takeLife()
    {
        lives -= 1;
        // in game ui health thing
    }

    public void registerPellet()
    {
        pelletsLeft -= 1;
    }

    void LerpPacStu()
    {
        elapsedTime += Time.deltaTime;
        float ratio = elapsedTime / duration;
        transform.position = Vector3.Lerp(startPosition, targetPosition, ratio);
        transform.localScale = Vector3.Lerp(startScale, targetScale, ratio);
    }

    void SaveHighScore()
    {
        int prevHighScore = PlayerPrefs.GetInt("PacStu-HighScore", 0);
        float prevTime = PlayerPrefs.GetInt("PacStu-TopTime", 0);
        if (eater.score > prevHighScore)
        {
            PlayerPrefs.SetInt("PacStu-HighScore", eater.score);
            PlayerPrefs.SetFloat("PacStu-TopTime", uiController.timer);
            PlayerPrefs.Save();
        }
        else if (eater.score == prevHighScore)
        {
            if (uiController.timer < prevTime)
            {
                PlayerPrefs.SetFloat("PacStu-TopTime", uiController.timer);
                PlayerPrefs.Save();
            }
        }
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            audioSource.Stop();
            audioSource.loop = false;
            uiController.StopTimer();
            if (lives <= 0)
            {
                audioSource.clip = gameOverMusic;
                audioSource.Play();
            }
            GameObject.FindGameObjectWithTag("PacStuCollider").GetComponent<BoxCollider>().enabled = false;
            SaveHighScore();
            startPosition = transform.position;
            
        }
        else
        {
            if (lives <= 0)
            {
                if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    LerpPacStu();
                    return;
                }
                else if (!deathAnimationPlayed)
                {
                    pacStuAnimator.SetBool("isDead", true);
                    deathAnimationPlayed = true;
                }
            }
            else
            {
                pacStuAnimator.SetBool("gameOverWon", true);
            }

            if (pacStuAnimator.GetBool("gameOverWon") || pacStuAnimator.GetCurrentAnimatorStateInfo(0).IsName("GameOverLost"))
                displayGameOver = true;
            
            if (displayGameOver)
            {
                if (timer <= 3f)
                {
                    timer += Time.deltaTime;
                    uiController.ShowGameOver();
                }
                else
                    SceneManager.LoadScene(0);
            }
        }

    }
}
