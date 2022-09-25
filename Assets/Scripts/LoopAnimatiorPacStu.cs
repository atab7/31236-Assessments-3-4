using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopAnimatiorPacStu : MonoBehaviour
{
    private Animator animator;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        timer = 0.0f;
        animator.SetInteger("Direction", 0);
        animator.SetBool("isDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 6)
        {
            animator.SetInteger("Direction", 0);
            animator.SetBool("isDead", false);
            timer = 0.0f;


        }

        else if (timer >= 5)
        {
            animator.SetInteger("Direction", -1);
            animator.SetBool("isDead", true);

        }

        else if (timer >= 4)
        {

            animator.SetInteger("Direction", 12);
        }

        else if (timer >= 3)
        {
            animator.SetInteger("Direction", 9);

        }

        else if (timer >= 2)
        {
            animator.SetInteger("Direction", 6);
        }

        else if (timer >= 1)
        {
            animator.SetInteger("Direction", 3);
        }
    }
}
