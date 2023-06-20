using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using TMPro;

public class MouseInput : MonoBehaviour
{
    private PlayerInput playerInput;
    public GameObject setTarget;
    private TargetManager targetManager;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        targetManager = FindObjectOfType<TargetManager>();
        playerInput.actions["Click"].performed += TestFire;
        playerInput.actions["Reset"].performed += Reset;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider.gameObject.tag == "TargetCollider")
            {
                setTarget = hit.collider.transform.parent.gameObject;
            }
            else
            {
                setTarget = null;
            }
        }
        else
        {
            setTarget = null;
        }
    }
    private void TestFire(InputAction.CallbackContext context)
    {
        if (setTarget != null && targetManager.targets[targetManager.gameObjects.IndexOf(setTarget.gameObject)].active == true)
        {
            targetManager.DropTarget(setTarget, 100);
        }
    }
    private void Reset(InputAction.CallbackContext context)
    {
        targetManager.Stop();
    }
}