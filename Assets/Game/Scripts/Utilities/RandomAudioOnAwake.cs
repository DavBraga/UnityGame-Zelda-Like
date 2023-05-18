using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioOnAwake : MonoBehaviour
{
    public List<AudioClip> audioClips;

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioClip choosenClip = audioClips[Random.Range(0,audioClips.Count)]; 
        audioSource.PlayOneShot(choosenClip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
