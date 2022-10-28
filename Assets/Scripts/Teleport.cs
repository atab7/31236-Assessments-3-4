using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    PacStudentController pacStuController;

    // Start is called before the first frame update
    void Start()
    {
        pacStuController = GameObject.FindGameObjectWithTag("PacStudent").GetComponent<PacStudentController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PacStuCollider"))
            pacStuController.setTeleportNow(true);
    }

    
}
