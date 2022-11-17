using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class MarioController : MonoBehaviour
{
    Animator m_AnimatorController;
    CharacterController m_CharacterController;
    public Camera m_Camera;
    public float m_LerpRotation = 0.85f;
    public float m_WalkSpeed = 2.5f;
    public float m_RunSpeed = 5f;

    float m_VerticalSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_AnimatorController = GetComponent<Animator>();
        m_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float l_Speed = 0.0f;
        /*if(Input.GetKey(KeyCode.UpArrow))
        {
            speed = 0.5f;
            if(Input.GetKey(KeyCode.LeftShift))
            {
                speed = 1;
            }
        }
        m_AnimatorController.SetFloat("Speed", speed);
        
        if(Input.GetKey(KeyCode.Space))
        {
            m_AnimatorController.SetTrigger("Punch");
        }*/

        Vector3 l_ForwardsCamera = m_Camera.transform.forward;
        Vector3 l_RightCamera = m_Camera.transform.right;
        l_ForwardsCamera.y = 0.0f;
        l_RightCamera.y = 0.0f;
        l_ForwardsCamera.Normalize();
        l_RightCamera.Normalize();

        bool l_HasMovement = false;

        Vector3 l_Movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            l_HasMovement = true;
            l_Movement = l_ForwardsCamera;
        }
        if (Input.GetKey(KeyCode.A))
        {
            l_HasMovement = true;
            l_Movement -= l_RightCamera;
        }
        if (Input.GetKey(KeyCode.S))
        {
            l_HasMovement = true;
            l_Movement = -l_ForwardsCamera;
        }
        if (Input.GetKey(KeyCode.D))
        {
            l_HasMovement = true;
            l_Movement += l_RightCamera;
        }

        l_Movement.Normalize();

        float l_MovementSpeed = 0.0f;

        if (l_HasMovement)
        {
            Quaternion l_lookAtRotation = Quaternion.LookRotation(l_Movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, l_lookAtRotation, m_LerpRotation);

            l_Speed = 0.5f;
            l_MovementSpeed = m_WalkSpeed;


            if (Input.GetKey(KeyCode.LeftShift))
            {
                l_Speed = 1;
                l_MovementSpeed = m_RunSpeed;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                m_AnimatorController.SetTrigger("Punch");
            }

            m_VerticalSpeed = 10f;
            m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;
            l_Movement.y = m_VerticalSpeed * Time.deltaTime;

            m_AnimatorController.SetFloat("Speed", l_Speed);

            l_Movement = l_Movement * l_MovementSpeed * Time.deltaTime;

            CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_Movement);

            if ((l_CollisionFlags & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f)
            {
                m_VerticalSpeed = 0.0f;
            }

            if ((l_CollisionFlags & CollisionFlags.Below) != 0)
            {
                m_VerticalSpeed = 0.0f;
            }

            // m_CharacterController.Move(l_Movement);
        }

    }
}
