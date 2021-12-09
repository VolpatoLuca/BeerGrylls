using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private GameObject beerPrefab;
    [SerializeField] private float spillRate;
    [SerializeField] private ParticleSystem particle;
    private float spillTimer;

    private bool playParticleReset;

    private void Start()
    {
        playParticleReset = true;
    }

    private void Update()
    {
        if (GameManager.instance.isPlaying)
        {
            if (playParticleReset)
            {
                playParticleReset = false;
                if (particle != null)
                {
                    particle.Play();
                }
            }
            spillTimer += Time.deltaTime;
            if (spillTimer >= spillRate)
            {
                spillTimer = 0;
                Instantiate(beerPrefab, transform.position, transform.rotation);
            }
        }
        else
        {
            if (particle != null)
            {
                particle.Pause();
            }
            playParticleReset = true;
        }

        if (!GameManager.instance.hasGameStarted)
        {
            Destroy(gameObject);
        }
    }
}
