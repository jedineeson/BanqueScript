using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualRotation : MonoBehaviour
{	
    [SerializeField]
    private float m_RotSpeed;
    [SerializeField]
    private string m_Axis;


	private void Update ()
    {
        transform.Rotate(Vector3.m_Axis, m_RotSpeed * Time.deltaTime);
	}
}