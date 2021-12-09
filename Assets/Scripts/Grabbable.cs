using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
    public bool isGrabbed = false;
    public bool specificGrabArea;
    public bool turnBasedOnHand;

    public Transform turnPivot;
    public Transform alternativeGrab;

    public virtual void TriggerAction()
    {
        //roba da tutti
    }

    public virtual void PrimaryButtonAction()
    {
        //roba da tutti
    }

    public virtual void ReleaseTriggerAction()
    {

    }

}
