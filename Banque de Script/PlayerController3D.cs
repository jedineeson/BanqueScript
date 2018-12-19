using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //variable editor
    [SerializeField]
    private float m_RotationSpeed = 20f;
    [SerializeField]
    private float m_RunSpeed = 10f;
    [SerializeField]
    private float m_RunAcceleration = 5f;
    [SerializeField]
    private float m_JumpForce = 650f;
    [SerializeField]
    private float m_FakeGravity = 20f;
    [SerializeField]
    private Animator m_Animator;
    //[SerializeField]
    //private Transform m_RaycastParent;

    //variable input et mouvement
    private float m_CurrentSpeed = 0f;
    private float m_InputX = 0f;
    private float m_InputZ = 0f;
    private bool m_IsGrounded = true;

    //vector
    private Vector3 m_LookRotation = new Vector3();
    private Vector3 m_MoveDir = new Vector3();

    //Component
    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        //Initialiser mes variables;
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        m_MoveDir.y = m_Rigidbody.velocity.y;
        if (m_Rigidbody.velocity.y != 0f)
        {
            m_MoveDir.y -= m_FakeGravity * Time.fixedDeltaTime;
        }
        m_Rigidbody.velocity = m_MoveDir;
        //Modifier la velocity;
    }

    private void Update()
    {
        CheckInputAxis();
        //Valider les input utilisateurs;
        if (m_InputZ != 0)
        {
            if (m_CurrentSpeed < m_RunSpeed)
            {
                m_CurrentSpeed += m_RunSpeed * (m_RunAcceleration * Time.deltaTime);
            }
            m_MoveDir = transform.forward * m_CurrentSpeed * m_InputZ;
            m_Animator.SetBool("Walk", true);
            
        }
        else
        {
            m_MoveDir = Vector3.zero;
            m_CurrentSpeed = 0f;
            m_Animator.SetBool("Walk", false);

        }

        if (m_InputX != 0)
        {
            transform.Rotate(Vector3.up * m_RotationSpeed * m_InputX);
        }

        if (Input.GetButtonDown("Jump") && m_IsGrounded)
        {
            Jump();
        }

        //UpdateRotation();
    }


    private void Jump()
    {
        if (m_IsGrounded)
        {
            m_IsGrounded = false;
            //içi on annule la velocity courant en Y;
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, 0f, m_Rigidbody.velocity.z);
            //içi on lui donne une force en Y;
            m_Rigidbody.AddForce(transform.up * m_JumpForce);
        }
        //On ajoute une force en Y au joueur;
    }

    /*private void UpdateRotation()
    {
        //Update la rotation du joueur;
        m_LookRotation.x = m_InputX;
        //m_LookRotation.z = m_InputZ;
        m_LookRotation *= m_RunSpeed;

        if (CheckInputAxis())
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_LookRotation), m_RotationSpeed * Time.deltaTime);
        }
    }*/

   /* private void RaycastGround()
    {
        if (m_Rigidbody.velocity.y < 0f)
        {
            for (int i = 0; i < m_RaycastParent.childCount; i++)
            {
                Debug.DrawRay(m_RaycastParent.GetChild(i).transform.position, -transform.up);
                if (Physics.Raycast(m_RaycastParent.GetChild(i).transform.position, -transform.up, 0.6f))
                {
                    m_IsGrounded = true;
                    return;
                }
            }
        }
        //Raycast le sol 9 fois;
    }
    */
    private void OnTriggerEnter(Collider aOther)
    {
        if(aOther.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            m_IsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider aOther)
    {
        if (aOther.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            m_IsGrounded = false;
        }
    }

    private bool CheckInputAxis()
    {
        //On vérifie les Input et on retourne vrai si il y a un input;
        m_InputX = Input.GetAxis("Horizontal");
        m_InputZ = Input.GetAxis("Vertical");
        return m_InputX != 0f || m_InputZ != 0f;
    }
}