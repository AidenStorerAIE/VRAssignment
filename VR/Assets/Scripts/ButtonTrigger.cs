using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public Timer timer;
    public TargetManager targetManager;
    Vector3 startPos;
    public GameObject buttonOBJ;

    private void Start()
    {
        startPos = buttonOBJ.transform.position;
    }

    private void FixedUpdate()
    {
        if(buttonOBJ.transform.position.y > startPos.y)
        {
            buttonOBJ.transform.position = startPos;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Button")
        {
            timer.Activate();
            targetManager.InitialTarget();
        }
    }
}
