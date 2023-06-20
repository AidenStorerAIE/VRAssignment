using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using TMPro;

public class MouseRayForTesting : MonoBehaviour
{
    public InputActionManager playerInput;
    //public GameObject setTarget;
    private TargetManager targetManager;
    public GameObject rHand;
    public TextMeshPro testText;
    public LayerMask LM;
    bool castHit;
    public Gun Gun;

    void Start()
    {
        playerInput = GetComponent<InputActionManager>();
        targetManager = FindObjectOfType<TargetManager>();
        playerInput.actionAssets[0].FindAction("Fire").performed += Fire;
    }
    void Fire(InputAction.CallbackContext context)
    {
        Gun.Fire();
    }

    public void Active()
    {
        castHit = true;
    }
    public void Inactive()
    {
        castHit = false;
    }
}
