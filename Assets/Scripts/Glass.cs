using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : Grabbable
{
    [SerializeField] private float fullness;
    [SerializeField] private Color emptyColor;
    [SerializeField] private Color fullColor;
    [SerializeField] private string trayTag;
    [SerializeField] private int scoreValue;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private GameObject glassTop;
    [SerializeField] private LayerMask defaultLayer;
    [SerializeField] private LayerMask glassLayer;
    [SerializeField] private MeshRenderer meshRenderer;

    public bool isFull = false;

    private Material mat;
    private float currentFullness;
    private float maxRotation;
    private float timeToVanish;
    private bool delivered;

    private void Start()
    {
        mat = meshRenderer.material;
    }

    private void Update()
    {
        if (GameManager.instance.hasGameEnded)
        {
            Destroy(gameObject);
        }

        if (delivered)
        {
            timeToVanish += Time.deltaTime;
            if (timeToVanish >= 2)
            {
                Destroy(gameObject);
            }
        }

        GetMaxRotation();

        SmoothFill();

        float fillLevel = currentFullness / 5.6f;
        fillLevel -= 0.07f;
        if (maxRotation > 90)
        {
            fillLevel -= 0.03f;
        }
        mat.SetFloat("_FillLevel", fillLevel);

        SpillFromRotation();
        CheckFullness();
    }

    private void SmoothFill()
    {
        if (currentFullness == fullness)
        {
            // :)
        }
        else if (currentFullness < fullness)
        {
            currentFullness += 0.4f * Time.deltaTime;
        }
        else
        {
            currentFullness -= 0.4f * Time.deltaTime;
        }
    }

    private void CheckFullness()
    {
        if (currentFullness >= 0.85f)
        {
            isFull = true;
            glassTop.layer = 0;
        }
        else
        {
            isFull = false;
            glassTop.layer = 10;
        }
    }

    private void SpillFromRotation()
    {
        if (maxRotation > 85f)
        {
            fullness -= 0.3f * Time.deltaTime;
            PlayParticle(10);
        }
        if (maxRotation > 50f)
        {
            if (fullness >= 0.4f)
            {
                fullness -= 0.3f * Time.deltaTime;
                PlayParticle(10);
            }
        }
        if (maxRotation > 35f)
        {
            if (fullness >= 0.8f)
            {
                fullness -= 0.3f * Time.deltaTime;
                PlayParticle(10);
            }
        }
        else if (maxRotation <= 35f)
        {
            if (particle != null)
            {
                particle.Pause();
            }
        }
        fullness = Mathf.Clamp(fullness, 0, 1);
    }

    private void GetMaxRotation()
    {
        float xRotation = Mathf.Abs(transform.eulerAngles.x);
        float zRotation = Mathf.Abs(transform.eulerAngles.z);
        if (xRotation > 180f)
        {
            xRotation = 360 - xRotation;
        }
        if (zRotation > 180)
        {
            zRotation = 360 - zRotation;
        }
        maxRotation = Mathf.Max(xRotation, zRotation);
    }

    public void AddBeer()
    {
        fullness += 0.1f;
        fullness = Mathf.Clamp(fullness, 0, 1);
    }

    public override void TriggerAction()
    {
        base.TriggerAction();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(trayTag) && isFull)
        {
            if (isGrabbed)
            {
                GetComponentInParent<Grab>().objectsToGrab.Remove(this);
            }
            GameManager.instance.AddScore(scoreValue);
            Delivered();
        }
    }

    private void PlayParticle(float amount)
    {
        if (particle != null)
        {
            particle.Play();
            var emission = particle.emission;
            emission.rateOverTime = amount;
        }
    }

    private void Delivered()
    {
        delivered = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }
}
