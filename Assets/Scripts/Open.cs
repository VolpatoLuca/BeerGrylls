using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open : MonoBehaviour
{
    private bool resetTrigger = false;
    private InputManager controller;

    private List<Openable> openables = new List<Openable>();
    private void Awake()
    {
        controller = GetComponent<InputManager>();
    }

    private void Update()
    {
        if (controller.triggerBool)
        {
            if (resetTrigger)
            {
                resetTrigger = false;
                if (openables.Count > 0)
                {
                    openables[0].ToggleOpen();
                }
            }
        }
        else
        {
            resetTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Openable openable))
        {
            openables.Add(openable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Openable openable) && openables.Contains(openable))
        {
            openables.Remove(openable);
        }
    }
}
