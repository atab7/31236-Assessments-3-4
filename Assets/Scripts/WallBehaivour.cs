using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaivour : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip collisionAudio;
    const float playAfter = 0.15f;
    private bool time = false;
    private float timer;
    private ParticleSystem particleEffect;

    // Start is called before the first frame update
    void Start()
    {
        particleEffect = GameObject.FindGameObjectWithTag("PacStuCollider").GetComponent<ParticleSystem>();
        audioSource = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (time)
            timer += Time.deltaTime;

        if (timer > playAfter)
            PlayCollisionEffects();
    }

    private void PlayAudio()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(collisionAudio);
    }

    private void PlayParticleEffect()
    {
        var main = particleEffect.main;
        main.simulationSpeed = 2.5f;
        particleEffect.Play();
    }

    private void PlayCollisionEffects()
    {
        PlayAudio();
        PlayParticleEffect();
        timer = 0f;
        time = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PacStuCollider"))
            time = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag( "PacStuCollider"))
            time = false;
    }

}
