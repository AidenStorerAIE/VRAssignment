using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

//debugging
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public InputActionManager playerInput;
    public TargetManager targetManager;
    Rigidbody rb;
    Animator anim;

    [Header("Stats")]
    public float fireCooldown;
    float fireTimer;

    //debugging
    public GameObject rHand;
    //public GameObject lHand;
    public Transform attachPoint;
    public TextMeshPro testText;
    bool equipped;


    void Start()
    {
        playerInput = GetComponent<InputActionManager>();
        targetManager = FindObjectOfType<TargetManager>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        //playerInput.actionAssets[0].FindAction("Fire").performed += Fire;

        //playerInput.actionAssets[0].FindAction("Test").performed += Fire;
    }

    private void Update()
    {

        if(Vector3.Distance(transform.position, rHand.transform.position) < 1.2 /*|| 
            Vector3.Distance(transform.position, lHand.transform.position) < 0.5f*/)
        {
            transform.parent = attachPoint;


            if (transform.parent != null)
            transform.localPosition = Vector3.zero;


            if (equipped)
                return;



            //transform.position = transform.parent.transform.position;
            rb.useGravity = false;
            //anim.enabled = true;

            testText.text = ("Grab");
            equipped = true;
        }
        else
        {
            //testText.text = ("NoGrab");
            equipped = false;
            //anim.enabled = false;
            transform.parent = null;
        }
    }

    public void Fire()
    {
        if (!equipped)
        return;

        if (Time.time - fireTimer < fireCooldown)
            return;

        testText.text = ("FIre");
        anim.SetTrigger("Fire");
        fireTimer = Time.time;

        


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000))
        {
            if (hit.collider.gameObject.tag == "TargetCollider")
            {
                targetManager.DropTarget(hit.collider.transform.parent.gameObject, 100);

            }
            testText.text = ("FIreHit");
        }
    }
}
