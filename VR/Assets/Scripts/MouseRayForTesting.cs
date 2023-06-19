using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using TMPro;

public class MouseRayForTesting : MonoBehaviour
{
    public InputActionManager playerInput;
    public GameObject setTarget;
    private TargetManager targetManager;
    public GameObject gun;
    public LayerMask LM;
    public TextMeshPro text;
    int shots = 0;
    bool castHit;
    


    void Start()
    {
        playerInput = GetComponent<InputActionManager>();
        targetManager = FindObjectOfType<TargetManager>();
        //playerInput.actionAssets["Select"].performed += TestFire;
        playerInput.actionAssets[0].FindAction("Test").performed += Fire;
        //testAction.performed += DebugFire;
        //playerInput.
    }

    // Update is called once per frame
    void Update()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, 1000))
        //{
        //    if (hit.collider.gameObject.tag == "TargetCollider")
        //    {
        //        setTarget = hit.collider.transform.parent.gameObject;
        //    }
        //    else
        //    {
        //        setTarget = null;
        //    }
        //}
        //else
        //{
        //    setTarget = null;
        //}



    }
    private void TestFire(InputAction.CallbackContext context)
    {
        if (setTarget != null && targetManager.targets[targetManager.gameObjects.IndexOf(setTarget.gameObject)].active == true)
        {
            targetManager.DropTarget(setTarget);
        }
    }

    void Fire(InputAction.CallbackContext context)
    {
        if (!castHit)
            return;

        targetManager.DropTarget(setTarget);


        shots++;

        text.text = ("You hit button no hit " + shots);

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
