using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(InputManager))]

public class Grab : MonoBehaviour
{
    private GameObject closestObject;
    private Rigidbody heldRB;
    private GameObject heldObj;
    private Grabbable heldGrabbable;
    private Transform grabArea;
    private InputManager controller;

    [SerializeField] Material defaultMaterial;
    [SerializeField] Material highlightedMaterial;
    [SerializeField] float followSpeed;
    [SerializeField] float launchPower;

    public List<Grabbable> objectsToGrab = new List<Grabbable>();
    public bool isGrabbing;

    private void Awake()
    {
        controller = GetComponent<InputManager>();
    }

    private void Update()
    {
        CheckGrabbableObjects();

        FollowHand();

        //if (heldObj != null && controller.triggerFloat > .8f)
        //{
        //    if (heldGrabbable == null)
        //    {
        //        heldGrabbable = heldObj.GetComponent<Grabbable>();
        //    }
        //    heldGrabbable.TriggerAction();

        //}

        //if (heldObj != null && controller.triggerFloat <= .8f)
        //{
        //    if (heldGrabbable == null)
        //    {
        //        heldGrabbable = heldObj.GetComponent<Grabbable>();
        //    }
        //    heldGrabbable.ReleaseTriggerAction();

        //}

        //if (heldObj != null && controller.primaryButtonBool && resetPrimaryButton)
        //{
        //    if (heldGrabbable == null)
        //    {
        //        heldGrabbable = heldObj.GetComponent<Grabbable>();
        //    }
        //    heldGrabbable.PrimaryButtonAction();
        //    resetPrimaryButton = false;
        //}
        //if (!controller.primaryButtonBool)
        //{
        //    resetPrimaryButton = true;
        //}
    }

    private void CheckGrabbableObjects()
    {
        if (objectsToGrab.Count > 0)
        {
            if (controller.gripBool)
            {
                if (!isGrabbing)
                {
                    GrabObject();
                }
            }
            else
            {
                if (isGrabbing)
                {
                    ReleaseObject();
                }

                closestObject = GetClosestObjectInList();

                ChangeShader();
            }
        }
        else if (heldObj != null)
        {

        }
        else
        {
            isGrabbing = false;
            closestObject = null;
        }
    }

    private void FollowHand()
    {
        if (heldObj != null)
        {
            if (heldGrabbable.turnBasedOnHand)
            {
                if (gameObject.name == "LeftHandController")
                {
                    heldObj.transform.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    heldObj.transform.rotation = transform.rotation * Quaternion.Euler(0, -90, 0);
                }
            }


            if (Vector3.Distance(heldObj.transform.position, transform.position) <= .02f)
            {
                heldObj.transform.position = transform.position;
            }
            else
            {
                heldObj.transform.position = Vector3.MoveTowards(heldObj.transform.position, transform.position + (heldObj.transform.position - grabArea.position), followSpeed * Time.deltaTime);
            }
        }
    }

    private void GrabObject()
    {
        if (closestObject == null)
        {
            closestObject = GetClosestObjectInList();
        }
        if (closestObject == null)
        {
            return;
        }
        isGrabbing = true;
        heldObj = closestObject;
        //heldObj.GetComponent<MeshRenderer>().material = defaultMaterial;
        heldRB = heldObj.GetComponent<Rigidbody>();
        heldGrabbable = heldObj.GetComponent<Grabbable>();
        heldGrabbable.isGrabbed = true;
        heldRB.isKinematic = true;

        if (heldGrabbable.specificGrabArea)
        {
            grabArea = heldGrabbable.alternativeGrab;
        }
        else
        {
            grabArea = heldGrabbable.transform;
        }
    }

    private void ReleaseObject()
    {
        isGrabbing = false;
        if (heldObj != null)
        {
            heldObj.GetComponent<Grabbable>().isGrabbed = false;
            heldObj = null;
            heldGrabbable = null;
        }
        if (heldRB != null)
        {
            heldRB.isKinematic = false;
            Vector3 forceDirection = controller.velocity;
            forceDirection *= launchPower;
            heldRB.AddForce(forceDirection, ForceMode.VelocityChange);
            heldRB.angularVelocity = controller.angularVelocity;
        }
    }

    private void ChangeShader()
    {
        if (closestObject != null)
        {
            //closestObject.GetComponent<MeshRenderer>().material = highlightedMaterial;
        }
        foreach (var grabbable in objectsToGrab)
        {
            if (grabbable != null)
            {
                if (grabbable.gameObject != closestObject)
                {
                    //grabbable.gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
                }
            }
        }
    }

    private GameObject GetClosestObjectInList()
    {
        float minDistance = Mathf.Infinity;
        GameObject closestGameObject = null;
        float thisDistance;
        foreach (var grabbable in objectsToGrab)
        {
            if (grabbable != null)
            {
                thisDistance = Vector3.Distance(transform.position, grabbable.transform.position);
                if (thisDistance < minDistance && !grabbable.isGrabbed)
                {
                    minDistance = thisDistance;
                    closestGameObject = grabbable.gameObject;
                }
            }
        }
        return closestGameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.transform.parent == null)
        //{
        if (other.TryGetComponent(out Grabbable grabbable))
        {
            objectsToGrab.Add(grabbable);
        }
        //}
        //else
        //{
        //if (other.transform.parent.TryGetComponent(out Grabbable grabbable))
        //{
        //    objectsToGrab.Add(grabbable);
        //}
        //}

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Grabbable grabbable) && objectsToGrab.Contains(grabbable))
        {
            if (heldObj != grabbable.gameObject)
            {
                objectsToGrab.Remove(grabbable);
                if (closestObject != null)
                {
                    //closestObject.GetComponent<MeshRenderer>().material = defaultMaterial;
                }
            }
            else if (controller.gripBool)
            {
                objectsToGrab.Remove(grabbable);
                if (closestObject != null)
                {
                    //closestObject.GetComponent<MeshRenderer>().material = defaultMaterial;
                }
            }
        }
    }
}
