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
        rb = GetComponent<Rigidbody>();
    }

    public void SelectL()
    {
        transform.parent = gun.attachPointL;
        transform.rotation = gun.attachPointL.rotation;
        if(rb)
        rb.useGravity = false;
        gun.lHand.transform.GetChild(gun.lHand.transform.childCount - 1).gameObject.SetActive(false);
        gun.handModelL.SetActive(false);
        transform.localPosition = Vector3.zero;
    }

    public void SelectR()
    {
        transform.parent = gun.attachPointR;
        transform.rotation = gun.attachPointR.rotation;
        if(rb)
        rb.useGravity = false;
        gun.rHand.transform.GetChild(gun.rHand.transform.childCount - 1).gameObject.SetActive(false);
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

        if (gun.equippedL)
            gun.lHand.transform.GetChild(gun.lHand.transform.childCount - 1).gameObject.SetActive(true);
        else
            gun.rHand.transform.GetChild(gun.rHand.transform.childCount - 1).gameObject.SetActive(true);
        transform.parent = null;
    }

    public void Reenable()
    {

        if (gun.equippedL)
            gun.lHand.transform.GetChild(gun.lHand.transform.childCount - 1).gameObject.SetActive(true);
        else
            gun.rHand.transform.GetChild(gun.rHand.transform.childCount - 1).gameObject.SetActive(true);
        transform.parent = null;
    }
}
