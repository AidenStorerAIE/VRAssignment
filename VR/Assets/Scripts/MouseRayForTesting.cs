using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRayForTesting : MonoBehaviour
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
            targetManager.DropTarget(setTarget);
        }
    }
    private void Reset(InputAction.CallbackContext context)
    {
        targetManager.InitialTarget();
    }
}
