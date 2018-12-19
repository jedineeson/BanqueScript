using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MarioController3 : MonoBehaviour
{
    [SerializeField]
    private float m_RunSpeed = 10f;
    [SerializeField]
    private float m_Acceleration = 5f;
    [SerializeField]
    private float m_TurnSpeed = 10f;
    [SerializeField]
    private float m_JumpForce = 650f;
    [SerializeField]
    private float m_FakeGravity = 20f; //Accélération lorsque le personnage tombe
    [SerializeField]
    private float m_MaxGroundAngle = 45f; //Angle maximum des pente
    [SerializeField]
    private float m_CharacterHeight = 1f;
    [SerializeField]
    private float m_HeightPadding = 0.5f; //De ou part mon raycast
    [SerializeField]
    private LayerMask m_Layer;

    [SerializeField]
    private bool m_ActiveDebug = true; //Temporaire pour le debug

    private Rigidbody m_Rigidbody;
    private float m_CurrentSpeed = 0f;
    private Vector2 m_Input = new Vector2();
    private float m_RotationAngle; // variable de travail pour les calculs
    private float m_GroundAngle;

    private Quaternion m_TargetRotation = new Quaternion();
    private Transform m_Camera;

    private Vector3 m_Forward = new Vector3();
    private Vector3 m_Gravity = new Vector3(); //État actuel de la gravité
    private RaycastHit m_HitInfo; //Est-ce que je touche le sol et quel est l'angle de celui-çi
    private bool m_IsGrounded = true;
    private Vector3 m_MoveDir = new Vector3();

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Start ()
    {
        m_Camera = Camera.main.transform; //On stock le transform de la camera
	}
	
	private void Update ()
    {
        GetInput();
        CalculateDirection();
        RaycastGround();
        CalculateGroundAngle();
        CalculateForward();

        if(m_ActiveDebug)
        {
            DrawDebugLines();
        }
     
        if(Input.GetButton("jump"))
        {
            Jump();
        }
	}

    private void FixedUpdate()
    {
        if(m_Input != Vector2.zero)
        {
            Rotate();
            SetSpeed();
        }
        else
        {
            ResetSpeed();
        }

        m_MoveDir = m_Forward * m_CurrentSpeed;
        m_MoveDir.y = m_Rigidbody.velocity.y;

        if(m_Rigidbody.velocity.y != 0)
        {
            m_MoveDir.y -= m_FakeGravity * Time.fixedDeltaTime;
        }

        m_Rigidbody.velocity = m_MoveDir;
    }

    private void GetInput()
    {
        m_Input.x = Input.GetAxis("Horizontal");
        m_Input.y = Input.GetAxis("Vertical");
    }

    private void CalculateDirection()
    {
        //Calcul l'angle en radiant selon l'input
        m_RotationAngle = Mathf.Atan2(m_Input.x, m_Input.y);
        //transformé l'angle en degré
        m_RotationAngle *= Mathf.Rad2Deg;
        //Rotation relative à la caméra
        m_RotationAngle += m_Camera.localEulerAngles.y;
    }

    private void CalculateForward()
    {
        if (m_IsGrounded)
        {
            //Retourne le vector "Forward" en considérant la pente sur laquelle nous sommes.
            //Nous voulons donc un "Forward" parallèle 
            //Produit vectoriel de 2 vectors
            m_Forward = Vector3.Cross(transform.right, m_HitInfo.normal);
        }
        else
        {
            m_Forward = transform.forward;
        }
    }

    private void CalculateGroundAngle()
    {
        if(m_IsGrounded)
        {
            //Içi on détermine l'angle du sol en utilisant la normal et notre forward
            m_GroundAngle = Vector3.Angle(m_HitInfo.normal, transform.forward);
        }
        else
        {
            m_GroundAngle = 90f;
        }
    }

    private void RaycastGround() //Avec 9 raycast il est possible qu'on ai 2 fonctions, une pour l'angle, l'autre pour s'il peut sauter
    {
        if(Physics.Raycast(transform.position, Vector3.down, out m_HitInfo, m_CharacterHeight + m_HeightPadding, m_Layer))
        {
            m_IsGrounded = true;
        }
        else
        {
            m_IsGrounded = false;
        }
    }

    private void Rotate()
    {
        //Convertis les eulerAngles en Quaternion
        m_TargetRotation = Quaternion.Euler(0f, m_RotationAngle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, m_TargetRotation, m_TurnSpeed * Time.fixedDeltaTime);
    }

    private void SetSpeed()
    {
        if(m_GroundAngle < m_MaxGroundAngle + 90f)
        {
            if(m_CurrentSpeed < m_RunSpeed)
            {
                m_CurrentSpeed += m_RunSpeed * (m_Acceleration * Time.fixedDeltaTime);
            }
        }
        else
        {
            ResetSpeed();
        }
    }

    private void Jump()
    {
        if(m_IsGrounded)
        {
            m_IsGrounded = false;
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.AddForce(transform.up * m_JumpForce);
        }
    }

    private void ResetSpeed()
    {
        m_CurrentSpeed = 0f;
    }

    private void DrawDebugLines() //temporaire
    {
        Debug.DrawLine(transform.position, transform.position + m_Forward * m_HeightPadding * 2f, Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * (m_CharacterHeight + m_HeightPadding), Color.green);
    }
}

