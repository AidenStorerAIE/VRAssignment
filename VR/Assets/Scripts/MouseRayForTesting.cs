using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using TMPro;

public class MouseRayForTesting : MonoBehaviour
{
    public InputActionManager playerInput;
    public Gun Gun;

    void Start()
    {
        playerInput = GetComponent<InputActionManager>();
        playerInput.actionAssets[0].FindAction("Fire").performed += Fire;
        //playerInput.actionAssets[0].FindAction("Fire").performed += Fire;
        playerInput.actionAssets[0].FindAction("Drop").performed += DropMagazine;
        playerInput.actionAssets[0].FindAction("SwapL").performed += Swap;
        playerInput.actionAssets[0].FindAction("SwapR").performed += Swap;
    }
    void Fire(InputAction.CallbackContext context)
    {
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
