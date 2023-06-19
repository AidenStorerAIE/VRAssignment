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
    public TextMeshPro text;
    int shots = 0;
    bool castHit;

    void Start()
    {
        playerInput = GetComponent<InputActionManager>();
        targetManager = FindObjectOfType<TargetManager>();
        playerInput.actionAssets[0].FindAction("Test").performed += Fire;
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
                targetManager.DropTarget(hit.collider.transform.parent.gameObject);
            }
            shots++;
            text.text = ("You hit button no hit " + shots);
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
