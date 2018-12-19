using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public Rigidbody m_RigidBody;
    public float m_Speed;
    public GameObject m_Explosion;

    void Start ()
    {
        m_RigidBody.velocity = transform.forward * m_Speed;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController m_Player = other.GetComponent<PlayerController>();
        if (m_Player != null)
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Box") || other.gameObject.CompareTag("Door"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Ennemy"))
        {
            Instantiate(m_Explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(other.gameObject);
          
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
        //redémarrer la scène actuel
        SceneManager.LoadScene("DiabloLike", LoadSceneMode.Single);
        }
    }

}
