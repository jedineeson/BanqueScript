using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody m_Player;

    Vector3 m_TargetPosition;
    Vector3 m_LookAtTarget;
    Quaternion m_PlayerRot;
    public float m_RotSpeed;
    public float m_Speed;
    private bool m_Moving = false;

    private bool m_BoxParenting = false;
    private Transform m_ToTake;

    public GameObject m_Shot;
    public GameObject m_ShotSpawn;
    private float m_NextFire;
    public float m_FireRate;

    void Start()
    {

    }


    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            ActionChoice();
        }

        if (m_Moving)
        {
            Move();
        }
        else
        {
            m_Player.velocity = Vector3.zero;
        }

        if (Input.GetMouseButtonDown(1) && m_BoxParenting)
        {
            m_ToTake.SetParent(null);
            m_BoxParenting = false;
            Debug.Log("Lache la boite");
        }
        else if (Input.GetMouseButtonDown(1))
        {
            m_ToTake.parent = transform;
            m_BoxParenting = true;
            Debug.Log("Pogne la boite");
        }
    }

    void ActionChoice()
    {
        //Déclare un ray à la position de la souris
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Déclare le point de contact du ray
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2000))
        {
            //Destination = Contact du ray
            m_TargetPosition = hit.point;
            //Pour ne pas caller
            m_TargetPosition.y = transform.position.y;
            //Rotation vers un point précis
            this.transform.LookAt(m_TargetPosition);
            m_LookAtTarget = new Vector3(m_TargetPosition.x - transform.position.x, 00f, m_TargetPosition.z - transform.position.z);
            m_PlayerRot = Quaternion.LookRotation(m_LookAtTarget);

            if (hit.transform.tag == "Ennemy" && Time.time > m_NextFire)
            {
                m_Moving = false;
                m_NextFire = Time.time + m_FireRate;
                //Instancier un objet
                Instantiate(m_Shot, m_ShotSpawn.transform.position, m_ShotSpawn.transform.rotation);
            }
            else if (hit.transform.tag != "Ennemy")
            {
                m_Moving = true;
            }
        }
    }

    void Move()
    {
        //rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, m_PlayerRot, m_RotSpeed * Time.deltaTime);
        //vélocité
        m_Player.velocity = (m_TargetPosition - transform.position).normalized * m_Speed;
        //<= 1.5f pour éviter que ça bug
        if (Vector3.Distance(transform.position, m_TargetPosition) <= 1.5f)
        {
            m_Moving = false;
        }
    }

    //Pour Arrêter de bouger au contact de mur ou de boîtes
    private void OnCollisionEnter(Collision other)
    {
        //arrêter de bouger lors d'une collision
        if (other.gameObject.CompareTag("Box") || other.gameObject.CompareTag("Wall"))
        {
            m_Moving = false;
        }
    }

    //Peux activer le parenting
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            m_Moving = false;
            m_ToTake = other.transform;
     
        }
    }

    //Pour éviter le méga-bug de parenting
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Box"))
    //    {
    //        m_ToTake = null;
    //    }
    //}
}
