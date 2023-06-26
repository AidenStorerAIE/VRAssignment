using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObj : MonoBehaviour
{
    public int score;
    public List<AudioClip> sounds;
    private AudioSource AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        if (sounds.Count > 0)
        {
            int rando = Random.Range(0, sounds.Count);
            AudioSource.clip = sounds[rando];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void Drop()
    {

    }
}
