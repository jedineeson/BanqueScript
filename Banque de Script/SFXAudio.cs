using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXAudio : MonoBehaviour
{
    private float m_Counter = 0f;
    private float m_Duration = 3f;

    private void Update()
    {
        m_Counter += Time.deltaTime;
        if (m_Counter >= m_Duration)
        {
            Destroy(gameObject);
        }
    }
}


