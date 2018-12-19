using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneActivation : MonoBehaviour
{
    public List<EnnemyController> m_Ennemies;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (EnnemyController enemy in m_Ennemies)
            {
                if (enemy != null)
                {
                    enemy.m_CanShoot = true;
                }
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (EnnemyController enemy in m_Ennemies)
            {
                if (enemy != null)
                {
                    enemy.m_CanShoot = false;
                }
            }

        }
    }
}
