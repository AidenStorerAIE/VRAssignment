using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerInputController : MonoBehaviour
{
    public InputActionManager playerInput;
    public Gun Gun;

    void Start()
    {
        playerInput = GetComponent<InputActionManager>();
        playerInput.actionAssets[0].FindAction("FireL").performed += FireL;
        playerInput.actionAssets[0].FindAction("FireR").performed += FireR;
        playerInput.actionAssets[0].FindAction("DropL").performed += DropMagazineL;
        playerInput.actionAssets[0].FindAction("DropR").performed += DropMagazineR;
        playerInput.actionAssets[0].FindAction("SwapL").performed += Swap;
        playerInput.actionAssets[0].FindAction("SwapR").performed += Swap;
    }
    void FireL(InputAction.CallbackContext context)
    {
        if (Gun.curParent == Gun.attachPointL)
            Gun.Fire();
    }

    void FireR(InputAction.CallbackContext context)
    {
        if (Gun.curParent == Gun.attachPointR)
            Gun.Fire();
    }

    void DropMagazineL(InputAction.CallbackContext context)
    {
        if (Gun.curParent == Gun.attachPointL)
            Gun.DropMagazine();
    }
    void DropMagazineR(InputAction.CallbackContext context)
    {
        if (Gun.curParent == Gun.attachPointR)
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
