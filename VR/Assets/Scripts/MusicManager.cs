using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource music;
    public void Play()
    {
        music.Play();
    }
    public void Stop()
    {
        music.Stop();
    }
}
