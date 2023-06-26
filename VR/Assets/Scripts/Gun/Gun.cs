using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
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
    public TextMeshPro ammoText;
    public Transform attachPoint;
    public GameObject magPrefab;
    public Transform dropPoint;
    public Collider reloadSpace;

    [Header("Stats")]
    bool equipped;
    public int maxAmmo;
    int curAmmo;
    bool loaded;

    [Header("Timers")]
    public float fireCooldown;
    float fireTimer;
    public float reloadCooldown;
    float reloadTimer;

    [Header("Debugging")]
    public GameObject rHand;
    public XRRayInteractor interactor;
    public bool unlimitedAmmo;
    //public GameObject lHand;

    void Start()
    {
        //gets
        playerInput = GetComponent<InputActionManager>();
        targetManager = FindObjectOfType<TargetManager>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //ammo setting
        curAmmo = maxAmmo;
        UpdateUI();
        loaded = true;
    }

    private void Update()
    {

        if(Vector3.Distance(transform.position, rHand.transform.position) < 1f /*|| 
            Vector3.Distance(transform.position, lHand.transform.position) < 0.5f*/)
        {
            transform.parent = attachPoint;

            if (transform.parent != null)
            transform.localPosition = Vector3.zero;
            
            if (equipped)
                return;

            rb.useGravity = false;
;
            equipped = true;
            ammoText.gameObject.SetActive(true);

            GetComponent<XRGrabInteractable>().enabled = false;
            interactor.enabled = false;
            //transform.localRotation = new Quaternion(0,0,0,0);
        }
        else
        {
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

        if (curAmmo == 0)
        {
            return;
        }

        if (!unlimitedAmmo)
            curAmmo--;

        UpdateUI();
        anim.SetTrigger("Fire");
        fireTimer = Time.time;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000))
        {

            if (hit.collider.gameObject.tag == "TargetCollider")
            {
                targetManager.DropTarget(hit.collider.transform.parent.gameObject, true);
            }
        }
    }

    public void DropMagazine()
    {

        if (!loaded || !equipped)
            return;

        loaded = false;
        Magazine mag = Instantiate(magPrefab, dropPoint).GetComponent<Magazine>();
        mag.transform.parent = null;
        mag.ammoCount = curAmmo;

        curAmmo = 0;
        UpdateUI();

        reloadTimer = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        Magazine mag = other.GetComponent<Magazine>();
        if (!mag || loaded)
        {
            return;
        }

        if(curAmmo != maxAmmo)
        {
            if (Time.time - reloadTimer > reloadCooldown)
            {
                curAmmo = mag.ammoCount;
                UpdateUI();
                loaded = true;
                Destroy(mag.gameObject);
            }
        }
    }

    void UpdateUI()
    {
        ammoText.text = (curAmmo + "/" + maxAmmo);
    }
}
