using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteraction : MonoBehaviour
{
    private InputManager controller;
    private bool menuInputReset;

    private void Awake()
    {
        controller = GetComponent<InputManager>();
    }

    private void Start()
    {
        menuInputReset = true;
    }

    private void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (controller.primaryButtonBool)
        {
            GameManager.instance.PrymaryButtonPress();
        }
        else
        {
            GameManager.instance.PrymaryButtonRelease();
        }

        if (controller.secondaryButtonBool)
        {
            GameManager.instance.SecondaryButtonPress();
        }
        else
        {
            GameManager.instance.SecondaryButtonRelease();
        }

        if (controller.primaryAxisBool)
        {
            GameManager.instance.PrymaryAxisPress();
        }
        else
        {
            GameManager.instance.PrymaryAxisRelease();
        }

        if (controller.menuBool)
        {
            if (menuInputReset)
            {
                menuInputReset = false;
                GameManager.instance.MenuButtonPress();
            }
        }
        else
        {
            menuInputReset = true;
        }
    }
}
