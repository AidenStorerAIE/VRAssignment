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
    public float spawnTime;
    float spawnTimer;


    private void Start()
    {
        playerInput.actionAssets[0].FindAction("FireL").performed += Spawn;
        playerInput.actionAssets[0].FindAction("FireR").performed += Spawn;
    }

    void Spawn(InputAction.CallbackContext context)
    {
        if (!active)
            return;

        Instantiate(magazinePrefab, spawnPos);
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
