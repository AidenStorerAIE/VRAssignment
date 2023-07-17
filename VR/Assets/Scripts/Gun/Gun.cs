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
    [Header("Player/Manager References")]
    public InputActionManager playerInput;
    public TargetManager targetManager;
    Rigidbody rb;

    [Header("Controller References")]
    public GameObject lHand; public GameObject rHand;
    public XRRayInteractor interactorL; public XRRayInteractor interactorR;
    public GameObject handModelL; public GameObject handModelR;
    [HideInInspector] public Transform curParent;
    public XRBaseController controllerL; public XRBaseController controllerR;
    public Transform attachPointL; public Transform attachPointR;
    public HapticInteractable gunHaptic;

    [Header("Magazine References")]
    public GameObject magPrefab;
    public Transform dropPoint;
    public GameObject magInGun;

    [Header("Effects/UI References")]
    public ParticleSystem hitParticle;
    public ParticleSystem muzzleFlashParticle;
    public Light muzzleFlashLight;
    public TextMeshPro ammoText;
    public Animator gunAnimator;

    [Header("Stats")]
    public bool unlimitedAmmo; //used for testing
    public int maxAmmo;
    bool equippedL; bool equippedR;
    bool equipped;
    int curAmmo;
    bool loaded = true;

    [Header("Timers")]
    public float fireCooldown;
    float fireTimer;
    public float reloadCooldown;
    float reloadTimer;
    public float lightCooldown;
    float lightTimer;

    [Header("Sounds")]
    public List<AudioClip> gunsShotSounds;
    public AudioClip emptySound;
    private AudioSource audioSource;
    public AudioClip loadSound;
    public AudioClip unloadSound;

    void Start()
    {
        //references
        playerInput = GetComponent<InputActionManager>();
        targetManager = FindObjectOfType<TargetManager>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        //ammo setting
        curAmmo = maxAmmo;
        UpdateUI();
        loaded = true;
    }

    private void Update()
    {
        if (Time.time - lightTimer > lightCooldown)
        {
            //print("Light off");
            muzzleFlashLight.gameObject.SetActive(false);
        }


        //close to either hand. defaults to right
        if (Vector3.Distance(transform.position, rHand.transform.position) < 1f 
            || Vector3.Distance(transform.position, lHand.transform.position) < 1f)
        {
            //set parent then keep updating localPos
            transform.parent = curParent;

            if (transform.parent != null)
            transform.localPosition = Vector3.zero;
            
            //if already equipped, no need to run intial equip
            if (equipped)
                return;

            if (equippedR)
            {
                Magazine magazine = attachPointR.GetChild(0).GetComponent<Magazine>();
                magazine.Drop();
                equippedR = false;
            }

            //inital equip
            curParent = attachPointR;
            //handModelR.SetActive(false);
            rHand.transform.GetChild(rHand.transform.childCount - 1).gameObject.SetActive(false);
            handModelR.SetActive(false);
            ammoText.gameObject.SetActive(true);
            //sets
            rb.useGravity = false;
            equipped = true;
            GetComponent<XRGrabInteractable>().enabled = false;
            interactorR.enabled = false;
            transform.rotation = curParent.rotation;
        }
        else
        {
            //unequip and unparent
            equippedL = false; equippedR = false; equipped = false;
            transform.parent = null;
        }
        //disabling muzzleflash light
    }


    public void Fire()
    {
        //unable to fire checks
        if (!equipped)
        return;
        if (Time.time - fireTimer < fireCooldown)
            return;
        if (curAmmo == 0)
        {
            PlayEmptySound();
            return;
        }

        //used for testing
        if (!unlimitedAmmo)
            curAmmo--;

        //controller feedback, vibration
        if (curParent == attachPointR)
            gunHaptic.TriggerHaptic(controllerR);
        else
            gunHaptic.TriggerHaptic(controllerL);

        UpdateUI();
        PlaySound();
        gunAnimator.SetTrigger("Fire");
        fireTimer = Time.time;

        //particle system
        //muzzleFlashParticle.gameObject.SetActive(true);
        muzzleFlashLight.gameObject.SetActive(true);
        muzzleFlashParticle.Play();

        //print("Light on");
        lightTimer = Time.time; 

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000))
        {
            Vector3 point = hit.point;
            hitParticle.transform.position = point;
            hitParticle.Play();

            if (hit.collider.gameObject.tag == "TargetCollider")
            {
                targetManager.DropTarget(hit.collider.transform.parent.gameObject, true);
            }
        }
    }

    //magazine functions
    public void DropMagazine()
    {
        if (!loaded || (!equipped))
            return;

        loaded = false;
        magInGun.SetActive(false);
        audioSource.clip = unloadSound;
        audioSource.Play();
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

        if (curAmmo != maxAmmo)
        {
            if (Time.time - reloadTimer > reloadCooldown)
            {
                curAmmo = mag.ammoCount;
                UpdateUI();
                magInGun.SetActive(true);
                audioSource.clip = loadSound;
                audioSource.Play();
                loaded = true;
                Destroy(mag.gameObject);
            }
        }
    }
    public void Swap()
    {
        if (!equipped)
            return;

        //if equipped in right hand, change to left hand
        if (curParent == attachPointR)
        {
            if (equippedL)
            {
                Magazine magazine = attachPointL.GetChild(0).GetComponent<Magazine>();
                magazine.Drop();
            }

            curParent = attachPointL;
            interactorL.enabled = false;
            interactorR.enabled = true;
            transform.rotation = curParent.rotation;
            handModelR.SetActive(true);
            handModelL.SetActive(false);

            equippedL = true; equippedR = false;

            lHand.transform.GetChild(lHand.transform.childCount - 1).gameObject.SetActive(false);

        }
        else
        {
            if (equippedR)
            {
                Magazine magazine = attachPointR.GetChild(0).GetComponent<Magazine>();
                magazine.Drop();
            }

            curParent = attachPointR;
            interactorR.enabled = false;
            interactorL.enabled = true;
            transform.rotation = curParent.rotation;
            handModelL.SetActive(true);
            handModelR.SetActive(false);

            equippedL = false; equippedR = true;

            rHand.transform.GetChild(rHand.transform.childCount - 1).gameObject.SetActive(false);

        }
        //set parenting
        transform.parent = curParent;
    }


    //sound functions
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

    //set UI
    void UpdateUI()
    {
        ammoText.text = (curAmmo + "/" + maxAmmo);
    }
}
