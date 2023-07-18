using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class Magazine : MonoBehaviour
{
    public int ammoCount;
    public Gun gun;
    Rigidbody rb;
    bool empty;
    public float despawnTime = 4f;
    float despawnTimer;
    //public InputActionManager playerInput;

    private void Start()
    {
        gun = FindObjectOfType<Gun>();
        rb = GetComponent<Rigidbody>();
        //playerInput.actionAssets[0].FindAction("FireR").performed += SelectL;
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


        if (!empty && ammoCount <= 0)
        {
            empty = true;
            despawnTimer = Time.time;
        }
        if (empty)
        {
            //if(gun.attachPointL.GetChild(0).GetComponent<Magazine>() ||
                //gun.attachPointR.GetChild(0).GetComponent<Magazine>() ||
                //gun.attachPointL.GetChild(0).GetComponent<Magazine>() != this 
                //|| gun.attachPointR.GetChild(0).GetComponent<Magazine>())
            {
                if (Time.time - despawnTimer > despawnTime)
                    Destroy(gameObject);
            }
        }
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
        else if(gun.equippedR)
            gun.rHand.transform.GetChild(gun.rHand.transform.childCount - 1).gameObject.SetActive(true);
        else if(gun.curParent == gun.attachPointL)
            gun.rHand.transform.GetChild(gun.rHand.transform.childCount - 1).gameObject.SetActive(true);
        else if(gun.curParent == gun.attachPointR)
            gun.lHand.transform.GetChild(gun.lHand.transform.childCount - 1).gameObject.SetActive(true);
        transform.parent = null;
    }
}
