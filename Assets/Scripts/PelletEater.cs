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
    private GhostStateController ghostStateController;
    private GameObject resetEffectObj;
    private ParticleSystem resetEffect;
    private PacStuLifeController lifeController;
    private AudioSource audioSource;
    public AudioClip[] deathSoundClips;
    private int deathSoundClipsIndex = 0;



    // Start is called before the first frame update
    void Start()
    {
        UIcontroller = GameObject.FindGameObjectWithTag("HUD").GetComponent<InGameUIController>();
        ghostStateController = GameObject.FindGameObjectWithTag("GhostManager").GetComponent<GhostStateController>();
        pacStuController = GetComponent<PacStudentController>();
        score = 0;
        resetEffectObj = GameObject.FindGameObjectWithTag("ResetEffect");
        resetEffect = resetEffectObj.GetComponent<ParticleSystem>();
        lifeController = GetComponent<PacStuLifeController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!resetEffect.isPlaying)
            resetEffectObj.transform.position = transform.position;
    }

    void PlayDeathSound()
    {
        if (deathSoundClipsIndex < deathSoundClips.Length)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(deathSoundClips[deathSoundClipsIndex], 0.3f);
            deathSoundClipsIndex++;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pellet"))
        {
            score += scorePerPellet;
            Destroy(other.gameObject);
            UIcontroller.ChangeScore(score);
            lifeController.registerPellet();
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
            ghostStateController.StartScaredSequence();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Ghost1"))
        {
            if (ghostStateController.IsGhostDeadly(1))
            {
                UIcontroller.UpdateLifeIndicator();
                PlayDeathSound();
                resetEffect.Play();
                lifeController.takeLife();
                pacStuController.ResetPacStu();
            }
            else if (ghostStateController.IsGhostKillable(1))
            {
                ghostStateController.KillGhost(1);
                score += 300;
                UIcontroller.ChangeScore(score);
            }
        }
        else if (other.CompareTag("Ghost2"))
        {
            if (ghostStateController.IsGhostDeadly(2))
            {
                UIcontroller.UpdateLifeIndicator();
                PlayDeathSound();
                resetEffect.Play();
                lifeController.takeLife();
                pacStuController.ResetPacStu();
            }
            else if (ghostStateController.IsGhostKillable(2))
            {
                ghostStateController.KillGhost(2);
                score += 300;
                UIcontroller.ChangeScore(score);
            }
        }
        else if (other.CompareTag("Ghost3"))
        {
            if (ghostStateController.IsGhostDeadly(3))
            {
                UIcontroller.UpdateLifeIndicator();
                PlayDeathSound();
                resetEffect.Play();
                lifeController.takeLife();
                pacStuController.ResetPacStu();
            }
            else if (ghostStateController.IsGhostKillable(3))
            {
                ghostStateController.KillGhost(3);
                score += 300;
                UIcontroller.ChangeScore(score);
            }
        }
        else if (other.CompareTag("Ghost4"))
        {
            if (ghostStateController.IsGhostDeadly(4))
            {
                UIcontroller.UpdateLifeIndicator();
                PlayDeathSound();
                resetEffect.Play();
                lifeController.takeLife();
                pacStuController.ResetPacStu();
            }
            else if (ghostStateController.IsGhostKillable(4))
            {
                ghostStateController.KillGhost(4);
                score += 300;
                UIcontroller.ChangeScore(score);
            }
        }
    }
}
