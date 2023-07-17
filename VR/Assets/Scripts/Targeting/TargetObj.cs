using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObj : MonoBehaviour
{
    public int score;
    public List<AudioClip> sounds;
    public AudioSource audioSourceOne;
    public AudioSource audioSourceTwo;
    public List<GameObject> targets;

    // Start is called before the first frame update
    void Start()
    {
        int rando1 = Random.Range(0, targets.Count);
        targets[rando1].SetActive(true);
        audioSourceOne = GetComponent<AudioSource>();
        if (sounds.Count > 0)
        {
            int rando = Random.Range(0, sounds.Count);
            audioSourceOne.clip = sounds[rando];
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
