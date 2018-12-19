using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineTrigger : MonoBehaviour
{
    public PlayableDirector m_Timeline;
    public bool m_UseOneTimeOnly = false;
    private bool m_HasBeenUsed = false;

    private void OnTriggerEnter(Collider aOther)
    {
        if (m_UseOneTimeOnly && !m_HasBeenUsed)
        {
            m_Timeline.Play();
            m_HasBeenUsed = true;
        }
        else if(!m_UseOneTimeOnly)
        {
            m_Timeline.Play();
        }

    }
}
