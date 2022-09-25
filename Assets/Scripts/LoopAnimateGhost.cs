using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopAnimateGhost : MonoBehaviour
{

    private Animator animator;
    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        timer = 0.0f;
        animator.SetBool("isScared", false);
        animator.SetBool("isRecovering", false);
        animator.SetBool("isDead", false);
        animator.SetInteger("Direction", 3);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 7)
        {
            animator.SetBool("isScared", false);
            animator.SetBool("isRecovering", false);
            animator.SetBool("isDead", false);
            animator.SetInteger("Direction", 3);
            timer = 0.0f;

            
        }

        else if (timer >= 6)
        {
            animator.SetBool("isDead", true);
            
        }

        else if (timer >= 5)
        {
            animator.SetBool("isScared", false);
            animator.SetBool("isRecovering", true);
        }

        else if (timer >= 4)
        {
            animator.SetBool("isScared", true);
        }

        else if (timer >= 3)
        {
            animator.SetInteger("Direction", 12);
        }

        else if (timer >= 2)
        {
            animator.SetInteger("Direction", 9);
        }

        else if (timer >= 1)
        {
            animator.SetInteger("Direction", 6);
        }

    }
}
