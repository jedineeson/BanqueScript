
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    private Animator m_Animator;

    private Vector3 m_MoveDir = new Vector3();

    private RaycastHit m_Hit;

    [SerializeField]
    private GameObject m_PocketLight;

    [SerializeField]
    private GameObject m_Camera;

    [SerializeField]
    private float m_RotSpeed = 2.0f;
    [SerializeField]
    private float m_MoveSpeed = 5.0f;

    private float m_XValue = 0.0f;
    private float m_InputX;

    private bool m_LightIsOn = false;


    private void Start()
    {
        Cursor.visible = false;
        m_Animator = gameObject.GetComponent<Animator>();
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
        m_XValue = transform.localEulerAngles.x;
        UpdatePocketLight();
    }

    private void Update()
    {
        GetInput();
        RayCast();
        DebugFunc();
        Animate();
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
    }

    private void GetInput()
    {
        m_InputX = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.O))
        {
            m_LightIsOn = !m_LightIsOn;
            UpdatePocketLight();
        }
    }

    private void Move()
    {
        m_MoveDir = (m_InputX * transform.forward);
        m_MoveDir *= m_MoveSpeed;
        m_MoveDir.y = m_Rigidbody.velocity.y;
        m_Rigidbody.velocity = m_MoveDir;
    }
    private void Rotate()
    {
        m_XValue += m_RotSpeed * Input.GetAxis("Mouse X");
        transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, m_XValue, gameObject.transform.localEulerAngles.z);
    }

    private void RayCast()
    {
        if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out m_Hit, 1))
        {
            if (m_Hit.collider.gameObject.GetComponent<Interactable>())
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.I))
                {
                    m_Hit.collider.gameObject.GetComponent<Interactable>().Interact();
                }
            }
        }
        else
        {

        }
    }

    private void DebugFunc()
    {
        if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out m_Hit, 1))
        {
            Debug.DrawRay(m_Camera.transform.position, m_Camera.transform.forward * 1, Color.red);
        }
        else
        {
            Debug.DrawRay(m_Camera.transform.position, m_Camera.transform.forward * 1, Color.green);
        }
    }

    private void Animate()
    {
        m_Animator.SetInteger("Walk", (int)m_InputX);
    }

    private void UpdatePocketLight()
    {
        if(m_LightIsOn)
        {
            m_PocketLight.SetActive(true);
        }
        else
        {
            m_PocketLight.SetActive(false);
        }
    }
}