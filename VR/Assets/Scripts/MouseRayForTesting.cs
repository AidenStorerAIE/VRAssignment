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
    public LayerMask LM;
    bool castHit;

    void Start()
    {
        playerInput = GetComponent<InputActionManager>();
        targetManager = FindObjectOfType<TargetManager>();
        playerInput.actionAssets[0].FindAction("Fire").performed += Fire;
    }
    void Fire(InputAction.CallbackContext context)
    {
        if (!castHit)
            return;
        //Aiden wrote this
        RaycastHit hit;
        if(Physics.Raycast(rHand.transform.position, rHand.transform.forward, out hit, 1000))
        {
            if(hit.collider.gameObject.tag == "TargetCollider")
            {
                targetManager.DropTarget(hit.collider.transform.parent.gameObject, 100);
            }
            //targetManager.DropTarget(setTarget);
        }
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
