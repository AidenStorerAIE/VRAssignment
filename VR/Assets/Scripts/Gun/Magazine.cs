using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    public int ammoCount;
    public Gun gun;
    Rigidbody rb;

    private void Start()
    {
        gun = FindObjectOfType<Gun>();
    }

    public void SelectL()
    {
        transform.parent = gun.attachPointL;
        transform.rotation = gun.attachPointL.rotation;
        rb.useGravity = false;
        gun.handModelL.SetActive(false);
        transform.localPosition = Vector3.zero;
    }

    public void SelectR()
    {
        transform.parent = gun.attachPointR;
        transform.rotation = gun.attachPointR.rotation;
        rb.useGravity = false;
        gun.handModelR.SetActive(false);
        transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if(transform.parent != null)
        transform.localPosition = Vector3.zero;
    }

    public void Drop()
    {
        rb.useGravity = true;
        transform.parent = null;
    }
}
