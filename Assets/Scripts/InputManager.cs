using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    public InputDeviceCharacteristics controller;
    [HideInInspector] public InputDevice thisDevice;

    [HideInInspector] public float gripFloat;
    [HideInInspector] public float triggerFloat;
    [HideInInspector] public Vector2 primaryAxisValue;
    [HideInInspector] public bool primaryAxisBool;
    [HideInInspector] public bool triggerBool;
    [HideInInspector] public bool gripBool;
    [HideInInspector] public bool primaryButtonBool;
    [HideInInspector] public bool secondaryButtonBool;
    [HideInInspector] public Vector3 angularVelocity;
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public bool menuBool;


    [SerializeField] private float followSpeed;
    [SerializeField] private LayerMask grabLayer;
    [SerializeField] private GameObject handPrefab;
    [SerializeField] private GameObject cubePrefab;

    public bool isUsingHand = false;

    private Animator handAnimator;


    private void Awake()
    {
        handAnimator = handPrefab.GetComponent<Animator>();
    }

    void Start()
    {
        TryGetController();

        if (isUsingHand)
        {
            handPrefab.SetActive(true);
            cubePrefab.SetActive(false);
        }
        else
        {
            handPrefab.SetActive(false);
            cubePrefab.SetActive(true);
        }
    }


    void Update()
    {
        if (!thisDevice.isValid)
        {
            TryGetController();
            return;
        }

        GetInputs();

        if (isUsingHand)
        {
            UpdateAnimations(gripFloat, triggerFloat);
        }
    }

    private void TryGetController()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controller, devices);

        if (devices.Count > 0)
        {
            thisDevice = devices[0];
        }
    }

    private void GetInputs()
    {
        thisDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out primaryAxisValue);
        thisDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out primaryAxisBool);
        thisDevice.TryGetFeatureValue(CommonUsages.grip, out gripFloat);
        thisDevice.TryGetFeatureValue(CommonUsages.gripButton, out gripBool);
        thisDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerFloat);
        thisDevice.TryGetFeatureValue(CommonUsages.triggerButton, out triggerBool);
        thisDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out velocity);
        thisDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out angularVelocity);
        thisDevice.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonBool);
        thisDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonBool);
        thisDevice.TryGetFeatureValue(CommonUsages.menuButton, out menuBool);
    }

    private void UpdateAnimations(float gripValue, float pinchValue)
    {
        handAnimator.SetFloat("Grip", gripValue);
        handAnimator.SetFloat("Pinch", pinchValue);
    }
}