using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public Timer timer;
    public TargetManager targetManager;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        if(transform.position.y > startPos.y)
        {
            transform.position = startPos;
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
