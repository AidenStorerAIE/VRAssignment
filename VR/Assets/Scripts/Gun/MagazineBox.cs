using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class MagazineBox : MonoBehaviour
{
    bool active;
    public GameObject magazinePrefab;
    public Transform spawnPos;
    public InputActionManager playerInput;
    public Gun gun;

    private void Start()
    {
        playerInput.actionAssets[0].FindAction("FireL").performed += SpawnL;
        playerInput.actionAssets[0].FindAction("FireR").performed += SpawnR;
    }

    void SpawnL(InputAction.CallbackContext context)
    {
        if (!active)
            return;

        if (gun.equippedL)
            return;

        //replace player hand instead
        Magazine magazine = Instantiate(magazinePrefab, spawnPos).GetComponent<Magazine>();
        magazine.gun = gun;
        magazine.SelectL();

        gun.equippedL = true;
        //set hand
    }
    void SpawnR(InputAction.CallbackContext context)
    {
        if (!active)
            return;

        if (gun.equippedR)
            return;

        //replace player hand instead
        Magazine magazine = Instantiate(magazinePrefab, spawnPos).GetComponent<Magazine>();
        magazine.gun = gun;
        magazine.SelectR();
        gun.equippedR = true;
        //set hand
    }

    public void Select()
    {
        active = true;
    }

    public void DeSelect()
    {
        active = false;
    }
}
