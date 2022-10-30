using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStateController : MonoBehaviour
{
    private Animator ghost1Anim;
    private Animator ghost2Anim;
    private Animator ghost3Anim;
    private Animator ghost4Anim;
    private bool ghostsScared = false;
    private bool ghostsRecovering = false;
    public bool scareSeqPlaying = false;
    private float timer = 10f;
    private float G1deathTimer = 5f;
    private float G2deathTimer = 5f;
    private float G3deathTimer = 5f;
    private float G4deathTimer = 5f;

    private InGameUIController UIcontroller;
    private AudioSource camAudio;
    [SerializeField] private AudioClip ghostsScaredMusic;
    [SerializeField] private AudioClip ghostDeadMusic;
    private AudioClip normalBackground;
    private PacStuLifeController lifeController;
    private GhostController ghost1Controller, ghost2Controller, ghost3Controller, ghost4Controller;

    // Start is called before the first frame update
    void Start()
    {
        UIcontroller = GameObject.FindGameObjectWithTag("HUD").GetComponent<InGameUIController>();
        
        GameObject ghost1 = GameObject.FindGameObjectWithTag("Ghost1");
        ghost1Anim = ghost1.GetComponent<Animator>();
        ghost1Controller = ghost1.GetComponent<GhostController>();
        
        GameObject ghost2 = GameObject.FindGameObjectWithTag("Ghost2");
        ghost2Anim = ghost2.GetComponent<Animator>();
        ghost2Controller = ghost2.GetComponent<GhostController>();

        GameObject ghost3 = GameObject.FindGameObjectWithTag("Ghost3");
        ghost3Anim = ghost3.GetComponent<Animator>();
        ghost3Controller = ghost3.GetComponent<GhostController>();

        GameObject ghost4 = GameObject.FindGameObjectWithTag("Ghost4");
        ghost4Anim = ghost4.GetComponent<Animator>();
        ghost4Controller = ghost4.GetComponent<GhostController>();

        camAudio = Camera.main.GetComponent<AudioSource>();
        normalBackground = camAudio.clip;

        lifeController = GameObject.FindGameObjectWithTag("PacStudent").GetComponent<PacStuLifeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeController.isGameOver)
            return;

        if (scareSeqPlaying)
            PlayScaredSequence();

        IfDeadPlayDeathSequence();
    }


    private void IfDeadPlayDeathSequence()
    {
        if (ghost1Anim.GetBool("isDead"))
        {
            G1deathTimer -= Time.deltaTime;
            if (G1deathTimer <= 0)
                ReviveGhost(1);
        }

        if (ghost2Anim.GetBool("isDead"))
        {
            G2deathTimer -= Time.deltaTime;
            if (G2deathTimer <= 0)
                ReviveGhost(2);
        }

        if (ghost3Anim.GetBool("isDead"))
        {
            G3deathTimer -= Time.deltaTime;
            if (G3deathTimer <= 0)
                ReviveGhost(3);
        }

        if (ghost4Anim.GetBool("isDead"))
        {
            G4deathTimer -= Time.deltaTime;
            if (G4deathTimer <= 0)
                ReviveGhost(4);
        }
    }


    public void StartScaredSequence()
    {
        scareSeqPlaying = true;
        ghostsScared = true;
        timer = 10f;
    }

    private void PlayScaredSequence()
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
                scareSeqPlaying = false;
                UIcontroller.ShowRemainingScaredGhostTime(0f);
            }
        }
    }

    private void ChangeBackgroundMusicToGhostDeath()
    {
        camAudio.Stop();
        camAudio.clip = ghostDeadMusic;
        camAudio.Play();
    }


    public bool IsGhostDeadly(int ghostID)
    {
        if (ghostID == 1)
        {
            if (!ghost1Anim.GetBool("isDead") && !(ghost1Anim.GetBool("isScared") || ghost1Anim.GetBool("isRecovering")))
                return true;
        }
        else if (ghostID == 2)
        {
            if (!ghost2Anim.GetBool("isDead") && !(ghost2Anim.GetBool("isScared") || ghost2Anim.GetBool("isRecovering")))
                return true;
        }
        else if (ghostID == 3)
        {
            if (!ghost3Anim.GetBool("isDead") && !(ghost3Anim.GetBool("isScared") || ghost3Anim.GetBool("isRecovering")))
                return true;
        }
        else if (ghostID == 4)
        {
            if (!ghost4Anim.GetBool("isDead") && !(ghost4Anim.GetBool("isScared") || ghost4Anim.GetBool("isRecovering")))
                return true;
        }

        return false;
    }

    public bool IsGhostKillable(int ghostID)
    {
        if (ghostID == 1)
        {
            if (!ghost1Anim.GetBool("isDead") && (ghost1Anim.GetBool("isScared") || ghost1Anim.GetBool("isRecovering")))
                return true;
        }
        else if (ghostID == 2)
        {
            if (!ghost2Anim.GetBool("isDead") && (ghost2Anim.GetBool("isScared") || ghost2Anim.GetBool("isRecovering")))
                return true;
        }
        else if (ghostID == 3)
        {
            if (!ghost3Anim.GetBool("isDead") && (ghost3Anim.GetBool("isScared") || ghost3Anim.GetBool("isRecovering")))
                return true;
        }
        else if (ghostID == 4)
        {
            if (!ghost4Anim.GetBool("isDead") && (ghost4Anim.GetBool("isScared") || ghost4Anim.GetBool("isRecovering")))
                return true;
        }

        return false;
    }


    public void KillGhost(int ghostID)
    {
        if (ghostID == 1 && IsGhostKillable(1))
        {
            ghost1Anim.SetBool("isDead", true);
            ghost1Anim.SetBool("isScared", false);
            ghost1Anim.SetBool("isRecovering", false);
            ghost1Controller.isDead = true;
            ghost1Controller.ResetGhost();
            ChangeBackgroundMusicToGhostDeath();
        }
        else if (ghostID == 2 && IsGhostKillable(2))
        {
            ghost2Anim.SetBool("isDead", true);
            ghost2Anim.SetBool("isScared", false);
            ghost2Anim.SetBool("isRecovering", false);
            ghost2Controller.isDead = true;
            ghost2Controller.ResetGhost();
            ChangeBackgroundMusicToGhostDeath();
        }
        else if (ghostID == 3 && IsGhostKillable(3))
        {
            ghost3Anim.SetBool("isDead", true);
            ghost3Anim.SetBool("isScared", false);
            ghost3Anim.SetBool("isRecovering", false);
            ghost3Controller.isDead = true;
            ghost3Controller.ResetGhost();
            ChangeBackgroundMusicToGhostDeath();
        }
        else if (ghostID == 4 && IsGhostKillable(4))
        {
            ghost4Anim.SetBool("isDead", true);
            ghost4Anim.SetBool("isScared", false);
            ghost4Anim.SetBool("isRecovering", false);
            ghost4Controller.isDead = true;
            ghost4Controller.ResetGhost();
            ChangeBackgroundMusicToGhostDeath();
        }
    }

    private void ReviveGhost(int ghostID)
    {
        if (ghostID == 1)
        {
            G1deathTimer = 5;
            ghost1Anim.SetBool("isDead", false);
            ghost1Controller.isDead = false;
        }
        else if (ghostID == 2)
        {
            G2deathTimer = 5;
            ghost2Anim.SetBool("isDead", false);
            ghost2Controller.isDead = false;
        }
        else if (ghostID == 3)
        {
            G3deathTimer = 5;
            ghost3Anim.SetBool("isDead", false);
            ghost3Controller.isDead = false;
        }
        else if (ghostID == 4)
        {
            G4deathTimer = 5;
            ghost4Anim.SetBool("isDead", false);
            ghost4Controller.isDead = false;
        }

        if (G1deathTimer + G2deathTimer + G3deathTimer + G4deathTimer == 20f)
        {
            if (camAudio.clip == ghostDeadMusic)
            {
                camAudio.Stop();
                if (scareSeqPlaying)
                {
                    camAudio.clip = ghostsScaredMusic;
                }
                else
                {
                    camAudio.clip = normalBackground;
                }
                camAudio.Play();
            }
        }
    }


    private void ScareGhosts()
    {
        if (!ghost1Anim.GetBool("isDead") && !ghost2Anim.GetBool("isDead") && !ghost3Anim.GetBool("isDead") && !ghost4Anim.GetBool("isDead"))
        {
            camAudio.Stop();
            camAudio.clip = ghostsScaredMusic;
            camAudio.Play();
        }

        ghost1Anim.SetBool("isScared", true);
        ghost2Anim.SetBool("isScared", true);
        ghost3Anim.SetBool("isScared", true);
        ghost4Anim.SetBool("isScared", true);
    }

    private void DescalateGhosts()
    {

        bool ghost1Dead = ghost1Anim.GetBool("isDead");
        bool ghost2Dead = ghost2Anim.GetBool("isDead");
        bool ghost3Dead = ghost3Anim.GetBool("isDead");
        bool ghost4Dead = ghost4Anim.GetBool("isDead");

        if (!ghost1Dead && !ghost2Dead && !ghost3Dead && !ghost4Dead)
        {
            camAudio.Stop();
            camAudio.clip = normalBackground;
            camAudio.loop = true;
            camAudio.Play();
        }

        if (!ghost1Dead)
            ghost1Anim.SetBool("isRecovering", false);
        if (!ghost2Dead)
            ghost2Anim.SetBool("isRecovering", false);
        if (!ghost3Dead)
            ghost3Anim.SetBool("isRecovering", false);
        if (!ghost4Dead)
            ghost4Anim.SetBool("isRecovering", false);
    }

    private void RecoverGhosts()
    {
        ghost1Anim.SetBool("isScared", false);
        ghost2Anim.SetBool("isScared", false);
        ghost3Anim.SetBool("isScared", false);
        ghost4Anim.SetBool("isScared", false);

        if (!ghost1Anim.GetBool("isDead"))
            ghost1Anim.SetBool("isRecovering", true);
        if (!ghost2Anim.GetBool("isDead"))
            ghost2Anim.SetBool("isRecovering", true);
        if (!ghost3Anim.GetBool("isDead"))
            ghost3Anim.SetBool("isRecovering", true);
        if (!ghost4Anim.GetBool("isDead"))
            ghost4Anim.SetBool("isRecovering", true);
    }
}
