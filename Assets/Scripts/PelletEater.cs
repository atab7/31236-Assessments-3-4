using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletEater : MonoBehaviour
{
    public int score { get; private set; }
    private const int scorePerPellet = 10;
    private const int scorePerCherry = 100;
    private PacStudentController pacStuController;
    private InGameUIController UIcontroller;
    private Animator ghost1Anim;
    private Animator ghost2Anim;
    private Animator ghost3Anim;
    private Animator ghost4Anim;
    private bool ghostsScared = false;
    private bool ghostsRecovering = false;
    private float timer = 10f;
    private AudioSource camAudio;
    [SerializeField] private AudioClip ghostsScaredMusic;
    private AudioClip normalBackground;

    // Start is called before the first frame update
    void Start()
    {
        UIcontroller = GameObject.FindGameObjectWithTag("HUD").GetComponent<InGameUIController>();
        pacStuController = GetComponent<PacStudentController>();
        score = 0;
        camAudio = Camera.main.GetComponent<AudioSource>();
        normalBackground = camAudio.clip;
        ghost1Anim = GameObject.FindGameObjectWithTag("Ghost1").GetComponent<Animator>();
        ghost2Anim = GameObject.FindGameObjectWithTag("Ghost2").GetComponent<Animator>();
        ghost3Anim = GameObject.FindGameObjectWithTag("Ghost3").GetComponent<Animator>();
        ghost4Anim = GameObject.FindGameObjectWithTag("Ghost4").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UIcontroller.ShowRemainingScaredGhostTime(timer);

        if (ghostsScared)
        {
            if (timer == 10f)
                ScareGhosts();

            timer -= Time.deltaTime;

            if (timer <= 3.0f)
            {
                RecoverGhosts();
                ghostsScared = false;
                ghostsRecovering = true;
            }
        }
        else if (ghostsRecovering)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                DescalateGhosts();
                timer = 10f;
                ghostsRecovering = false;
            }
        }

    }

    private void ScareGhosts()
    {
        camAudio.Stop();
        camAudio.clip = ghostsScaredMusic;
        camAudio.loop = true;
        camAudio.Play();
        ghost1Anim.SetBool("isScared", true);
        ghost2Anim.SetBool("isScared", true);
        ghost3Anim.SetBool("isScared", true);
        ghost4Anim.SetBool("isScared", true);
    }

    private void DescalateGhosts()
    {
        camAudio.Stop();
        camAudio.clip = normalBackground;
        camAudio.loop = true;
        camAudio.Play();
        ghost1Anim.SetBool("isRecovering", false);
        ghost2Anim.SetBool("isRecovering", false);
        ghost3Anim.SetBool("isRecovering", false);
        ghost4Anim.SetBool("isRecovering", false);
    }

    private void RecoverGhosts()
    {
        ghost1Anim.SetBool("isScared", false);
        ghost2Anim.SetBool("isScared", false);
        ghost3Anim.SetBool("isScared", false);
        ghost4Anim.SetBool("isScared", false);
        ghost1Anim.SetBool("isRecovering", true);
        ghost2Anim.SetBool("isRecovering", true);
        ghost3Anim.SetBool("isRecovering", true);
        ghost4Anim.SetBool("isRecovering", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pellet"))
        {
            score += scorePerPellet;
            Destroy(other.gameObject);
            UIcontroller.ChangeScore(score);
        }
        else if (other.CompareTag("Cherry"))
        {
            score += scorePerCherry;
            Destroy(other.gameObject);
            UIcontroller.ChangeScore(score);
            pacStuController.audioSource.Stop();
            pacStuController.audioSource.PlayOneShot(pacStuController.eatingSound);
        }
        else if (other.CompareTag("PowerPellet"))
        {
            ghostsScared = true;
            Destroy(other.gameObject);
            timer = 10f;
        }
    }
}
