using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using TMPro;
using Unity.VisualScripting;

public class MouseInput : MonoBehaviour
{
    private PlayerInput playerInput;
    public GameObject setTarget;
    private TargetManagerVarient targetManagerVarient;
    public List<AudioClip> gunshots;
    private AudioSource audioSource;
    public Transform gun;
    public Transform shellPos;
    public GameObject shell;
    private float lastFire;
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        targetManagerVarient = FindObjectOfType<TargetManagerVarient>();
        audioSource = GetComponent<AudioSource>();
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
            gun.LookAt(hit.point);
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
        if (Time.time > lastFire + delay)
        {
            lastFire = Time.time;
            gun.GetComponentInChildren<Animator>().SetTrigger("Shoot");
            var newShell = Instantiate(shell, shellPos.position, Quaternion.Euler(new Vector3(shellPos.rotation.x + 180, shellPos.rotation.y + Random.Range(-15, 15), shellPos.rotation.z + Random.Range(-15, 15))));
            newShell.GetComponent<Rigidbody>().AddForce(shellPos.transform.up * 100);
            StartCoroutine(DestroyShell(newShell));
            int rando = Random.Range(0, gunshots.Count);
            audioSource.clip = gunshots[rando];
            audioSource.Play();
            if (setTarget != null)
            {
                targetManagerVarient.DropTarget(setTarget, setTarget.GetComponent<TargetObj>().score);
            }
        }
    }
    private void Reset(InputAction.CallbackContext context)
    {
        //targetManagerVarient.Stop();
        targetManagerVarient.InitialTarget();
    }
    IEnumerator DestroyShell(GameObject shell)
    {
        yield return new WaitForSeconds(5);
        Destroy(shell);
    }
}