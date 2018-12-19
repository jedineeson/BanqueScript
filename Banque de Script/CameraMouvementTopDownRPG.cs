using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class CameraMouvement : MonoBehaviour
{
    private Vector3 m_Destination = new Vector3();
    public Boundary m_Boundary;
    public Rigidbody m_Camera;
    public Rigidbody m_Player;

    private void Start()
    {
        //Desination = position actuel - position désiré
        m_Destination = transform.position - m_Player.transform.position;
    }

    private void Update()
    {
        if (m_Player == null)
        {
            return;
        }

        //si à l'intérieur des limites
        if (m_Player.position.x > m_Boundary.xMin && m_Player.position.x < m_Boundary.xMax && m_Player.position.z > m_Boundary.zMin && m_Player.position.z < m_Boundary.zMax)
        {
            transform.position = m_Player.transform.position + m_Destination;
        }
        //sinon si à l'extérieur des limites en x ET en z
        else if ((m_Player.position.x < m_Boundary.xMin || m_Player.position.x > m_Boundary.xMax) && (m_Player.position.z < m_Boundary.zMin || m_Player.position.z > m_Boundary.zMax))
        {
            //Vector3 (float x, float y, float z) pas sur une ligne seulement pour la visibilité
            m_Camera.position = new Vector3
                (
                //librairie Mathf / Clamp(objet à bouger, float min, float max)
                Mathf.Clamp(m_Camera.position.x, m_Boundary.xMin, m_Boundary.xMax),
                0.0f,
                Mathf.Clamp(m_Camera.position.z, m_Boundary.zMin, m_Boundary.zMax)
                );
        }
        //sinon si à l'extérieur des limites en x
        else if (m_Player.position.x < m_Boundary.xMin || m_Player.position.x > m_Boundary.xMax)
        {
            m_Camera.position = new Vector3
                (
                Mathf.Clamp(m_Camera.position.x, m_Boundary.xMin, m_Boundary.xMax),
                0.0f,
                Mathf.Clamp(m_Camera.position.z, m_Player.position.z, m_Player.position.z)
                );
        }
        //sinon si à l'extérieur des limites en y
        else if (m_Player.position.z < m_Boundary.zMin || m_Player.position.z > m_Boundary.zMax)
        {
            m_Camera.position = new Vector3
                (
                Mathf.Clamp(m_Camera.position.x, m_Player.position.x, m_Player.position.x),
                0.0f,
                Mathf.Clamp(m_Camera.position.z, m_Boundary.zMin, m_Boundary.zMax)
                );
        }
    }
}

