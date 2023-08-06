using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
public class PostTimeline : MonoBehaviour
{
    public UnityEvent postEvents; 
    [SerializeField] PlayableDirector director;

    private void OnEnable() {
        director.stopped+= PlayEvents;
    }
     private void OnDisable() {
        director.stopped-= PlayEvents;
    }

    public void PlayEvents(PlayableDirector director)
    {
        postEvents?.Invoke();

    }
}
