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
    [HideInInspector] public Transform curParent;
    public Transform attachPointL;
    public Transform attachPointR;
    public GameObject magPrefab;
    public Transform dropPoint;
    public Collider reloadSpace;
    public HapticInteractable gunHaptic;
    public XRBaseController controllerL;
    public XRBaseController controllerR;

    [Header("Stats")]
    bool equipped;
    public int maxAmmo;
    int curAmmo;
    bool loaded = true;

    [Header("Timers")]
    public float fireCooldown;
    float fireTimer;
    public float reloadCooldown;
    float reloadTimer;

    [Header("Debugging")]
    public GameObject lHand;
    public GameObject rHand;
    public XRRayInteractor interactorR;
    public XRRayInteractor interactorL;
    public bool unlimitedAmmo;

    public List<AudioClip> gunsShotSounds;
    public AudioClip emptySound;
    private AudioSource audioSource;


    void Start()
    {
        //references
        playerInput = GetComponent<InputActionManager>();
        targetManager = FindObjectOfType<TargetManager>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //ammo setting
        curAmmo = maxAmmo;
        UpdateUI();
        loaded = true;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, rHand.transform.position) < 1f 
            || Vector3.Distance(transform.position, lHand.transform.position) < 1f
            )
        {
            transform.parent = curParent;

            if (transform.parent != null)
            transform.localPosition = Vector3.zero;
            
            if (equipped)
                return;

            //inital equip
            curParent = attachPointR;
            ammoText.gameObject.SetActive(true);

            rb.useGravity = false;
            equipped = true;

            GetComponent<XRGrabInteractable>().enabled = false;
            interactorR.enabled = false;
            transform.rotation = curParent.rotation;
        }
        else
        {
            equipped = false;
            //anim.enabled = false;
            transform.parent = null;
        }
    }

    public void Swap()
    {
        if (!equipped)
            return;
        if (curParent == attachPointR)
        {
            curParent = attachPointL;
            interactorL.enabled = false;
            interactorR.enabled = true;
            transform.rotation = curParent.rotation;
        }
        else
        {
            curParent = attachPointR;
            interactorR.enabled = false;
            interactorL.enabled = true;
            transform.rotation = curParent.rotation;
        }
        transform.parent = curParent;
        //transform.localPosition = Vector3.zero;
    }
    public void Fire()
    {
        if (!equipped)
        return;

        if (Time.time - fireTimer < fireCooldown)
            return;

        if (curAmmo == 0)
        {
            PlayEmptySound();
            return;
        }

        if (!unlimitedAmmo)
            curAmmo--;

        if (curParent == attachPointR)
            gunHaptic.TriggerHaptic(controllerR);
        else
            gunHaptic.TriggerHaptic(controllerL);

        UpdateUI();
        //anim.SetTrigger("Fire");
        PlaySound();
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
    public void PlaySound()
    {
        int rando = Random.Range(0, gunsShotSounds.Count);
        audioSource.clip = gunsShotSounds[rando];
        audioSource.Play();
    }
    public void PlayEmptySound()
    {
        audioSource.clip = emptySound;
        audioSource.Play();
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
