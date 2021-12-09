using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : MonoBehaviour
{
    [SerializeField] private float startSpeed;
    [SerializeField] private string glassTag;
    [SerializeField] private string floorTag;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.up * startSpeed, ForceMode.VelocityChange);

        Destroy(gameObject, 10f);
    }


    private void Update()
    {
        if (!GameManager.instance.isPlaying)
        {
            Destroy(gameObject);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(glassTag))
        {
            float xRotation = Mathf.Abs(other.transform.eulerAngles.x);
            float zRotation = Mathf.Abs(other.transform.eulerAngles.z);
            if (xRotation > 180f)
            {
                xRotation = 360 - xRotation;
            }
            if (zRotation > 180)
            {
                zRotation = 360 - zRotation;
            }
            float maxRotation = Mathf.Max(xRotation, zRotation);
            maxRotation = Mathf.Abs(maxRotation);
            if (maxRotation >= 180f)
            {
                maxRotation = 360 - maxRotation;
            }
            if (maxRotation >= 45f)
            {
                return;
            }
            Glass glass = other.GetComponentInParent<Glass>();
            if (glass != null)
            {
                if (glass.isFull)
                {
                    return;
                }
                glass.AddBeer();
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag(floorTag))
        {
            GameManager.instance.AddSparedBeer();
            Destroy(gameObject);
        }
    }
}
