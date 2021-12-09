using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : MonoBehaviour
{
    public bool inverted;

    private bool isOpen;
    private float closeAngle;
    private float openAngle;
    private float currentAngle;
    private float t;

    private void Start()
    {
        closeAngle = 0;
        if (inverted)
        {
            openAngle = -70;
        }
        else
        {
            openAngle = 70;
        }
    }

    private void Update()
    {
        if (isOpen)
        {
            t += Time.deltaTime * 2;
        }
        else
        {
            t -= Time.deltaTime * 2;
        }
        t = Mathf.Clamp(t, 0, 1);

        currentAngle = Mathf.Lerp(closeAngle, openAngle, t);

        transform.eulerAngles = new Vector3(0, currentAngle, 0);
    }
    public void ToggleOpen()
    {
        isOpen = !isOpen;
    }
}
