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


    // Start is called before the first frame update
    void Start()
    {
        UIcontroller = GameObject.FindGameObjectWithTag("HUD").GetComponent<InGameUIController>();
        ghostStateController = GameObject.FindGameObjectWithTag("GhostManager").GetComponent<GhostStateController>();
        pacStuController = GetComponent<PacStudentController>();
        score = 0;
        resetEffectObj = GameObject.FindGameObjectWithTag("ResetEffect");
        resetEffect = resetEffectObj.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!resetEffect.isPlaying)
            resetEffectObj.transform.position = transform.position;
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
            ghostStateController.StartScaredSequence();
            Destroy(other.gameObject);
        }

        if (ghostStateController.scareSeqPlaying)
        {
            if (other.CompareTag("Ghost1"))
            {
                ghostStateController.KillGhost(1);
                score += 300;
            }
            else if (other.CompareTag("Ghost2"))
            {
                ghostStateController.KillGhost(2);
                score += 300;
            }
            else if (other.CompareTag("Ghost3"))
            {
                ghostStateController.KillGhost(3);
                score += 300;
            }
            else if (other.CompareTag("Ghost4"))
            {
                ghostStateController.KillGhost(4);
                score += 300;
            }
        }
        else
        {
            if (other.CompareTag("Ghost1") || other.CompareTag("Ghost2") || other.CompareTag("Ghost3") || other.CompareTag("Ghost4"))
            {
                resetEffect.Play();
                pacStuController.ResetPacStu();
            }
        }
    }
}
