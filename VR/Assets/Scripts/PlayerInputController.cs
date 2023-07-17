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
    public Gun gun;

    bool ParentedL()
    {
        return gun.curParent == gun.attachPointL;
    }
    bool ParentedR()
    {
        return gun.curParent == gun.attachPointR;
    }

    void Start()
    {
        playerInput = GetComponent<InputActionManager>();
        //inputs
        playerInput.actionAssets[0].FindAction("FireL").performed += FireL;
        playerInput.actionAssets[0].FindAction("FireR").performed += FireR;
        playerInput.actionAssets[0].FindAction("DropL").performed += DropMagazineL;
        playerInput.actionAssets[0].FindAction("DropR").performed += DropMagazineR;
        playerInput.actionAssets[0].FindAction("SwapL").performed += Swap;
        playerInput.actionAssets[0].FindAction("SwapR").performed += Swap;
    }

    //shooting, checks if gun in correct hand first
    void FireL(InputAction.CallbackContext context)
    {
        if (ParentedL())
            gun.Fire();
    }

    void FireR(InputAction.CallbackContext context)
    {
        if (ParentedR())
            gun.Fire();
    }

    //reloading, checks if gun in correct hand first
    void DropMagazineL(InputAction.CallbackContext context)
    {
        if (ParentedL())
            gun.DropMagazine();
        else if (gun.equippedL)
        {
            Magazine magazine = gun.attachPointL.GetChild(0).GetComponent<Magazine>();
            magazine.Drop();
            gun.equippedL = false;
        }
    }
    void DropMagazineR(InputAction.CallbackContext context)
    {
        if (ParentedR())
            gun.DropMagazine();
        else if (gun.equippedR)
        {
            Magazine magazine = gun.attachPointR.GetChild(0).GetComponent<Magazine>();
            magazine.Drop();
            gun.equippedR = false;
        }
    }

    void Swap(InputAction.CallbackContext context)
    {
        gun.Swap();
    }
}
