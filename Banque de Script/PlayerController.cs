using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum myState
    {
        explore,
        interact
    }

    [SerializeField]
    private Rigidbody m_Rigibody;
    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private float m_MoveSpeed = 10f;
    [SerializeField]
    private float m_RotSpeed = 10f;

    private Vector3 m_MoveDir;

    private GameObject m_Interactable;

    private myState m_MyState = myState.explore;

    private float m_InputX = 0;
    private float m_InputY = 0;

    private void Start()
    {
        InventoryManager.Instance.UpdateUI();
        if (GameManager.Instance.WasInFight || GameManager.Instance.IsMapTransition)
        {
            GameManager.Instance.WasInFight = false;
            GameManager.Instance.IsMapTransition = false;
            SetTransform();
        }
    }

    private void Update()
    {
        if (m_MyState == myState.explore)
        {
            SetInput();
        }
        else if (m_MyState == myState.interact)
        {
            LookAtInteractable();                                                                                                                    
        }
    }

    private void FixedUpdate()
    {
        if (m_MyState == myState.explore)
        {
            Rotate();
            m_Rigibody.velocity = m_MoveDir * m_MoveSpeed;
        }
    }

    private void SetInput()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            m_InputX = Input.GetAxisRaw("Horizontal");
            m_InputY = Input.GetAxisRaw("Vertical");
            m_Animator.SetInteger("walk", 1);
        }
        else
        {
            m_Animator.SetInteger("walk", 0);
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            InventoryManager.Instance.CallInventory();
        }
        if(Input.GetKeyDown(KeyCode.U))
        {
            InventoryManager.Instance.UpdateUI();
        }
    }

    private float SetDirection()
    {
        m_MoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));//Should not be in this function
        float rotationAngle;
        //Calcul l'angle en radiant selon l'input
        rotationAngle = Mathf.Atan2(m_InputX, m_InputY);
        //transformé l'angle en degré
        rotationAngle *= Mathf.Rad2Deg;
        return rotationAngle;
    }

    private void Rotate()
    {
        Quaternion targetRotation = new Quaternion();
        targetRotation = Quaternion.Euler(0f, SetDirection(), 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_RotSpeed * Time.fixedDeltaTime);
    }

    public void SetInteractable(GameObject interactable, bool onOrOff)
    {
        if (onOrOff)
        {
            m_Interactable = interactable;
        }
        else
        {
            m_Interactable = null;
            m_MyState = myState.explore;
            m_InputX = 0;
            m_InputY = -1;
        }
    }

    public void BeginInteraction()
    {
        m_MyState = myState.interact;
    }

    private void LookAtInteractable()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_Interactable.transform.position - transform.position), m_RotSpeed * Time.deltaTime);
    }

    private void SetTransform()
    {
        transform.position = GameManager.Instance.PlayerTransform.position;
        transform.rotation = GameManager.Instance.PlayerTransform.rotation;
    }
}
