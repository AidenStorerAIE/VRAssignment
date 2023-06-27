using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class MouseRayForTesting : MonoBehaviour
{
    public InputActionManager playerInput;
    public Gun Gun;

    //test haptic
    public HapticInteractable haphap;
    public XRBaseController controller;

    void Start()
    {
        playerInput = GetComponent<InputActionManager>();
        playerInput.actionAssets[0].FindAction("FireL").performed += FireL;
        playerInput.actionAssets[0].FindAction("FireR").performed += FireR;
        playerInput.actionAssets[0].FindAction("Drop").performed += DropMagazine;
        playerInput.actionAssets[0].FindAction("SwapL").performed += Swap;
        playerInput.actionAssets[0].FindAction("SwapR").performed += Swap;

        //haphap = FindObjectOfType<HapticInteractable>();
    }
    void FireL(InputAction.CallbackContext context)
    {
        //haphap.TriggerHaptic(controller);


        if (Gun.curParent == Gun.attachPointL)
        {
            Gun.Fire();

        }

    }

    void FireR(InputAction.CallbackContext context)
    {
        haphap.TriggerHaptic(controller);



        if (Gun.curParent == Gun.attachPointR)
            Gun.Fire();
    }

    void DropMagazine(InputAction.CallbackContext context)
    {
        Gun.DropMagazine();
    }

    void Swap(InputAction.CallbackContext context)
    {
        Gun.Swap();
    }

    //public void Active()
    //{
    //    castHit = true;
    //}
    //public void Inactive()
    //{
    //    castHit = false;
    //}
}
