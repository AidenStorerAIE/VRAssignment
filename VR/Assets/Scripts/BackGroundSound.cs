using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSound : MonoBehaviour
{
    public AudioSource Music;
    public AudioSource Voices;
    private float shotTime;
    public float volume;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        volume = ((shotTime + (Time.time / 10)) / shotTime) - 2;
    }
    public void SetSound()
    {
        shotTime = Time.time;
    }
}
