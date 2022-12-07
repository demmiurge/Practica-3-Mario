using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    Animator m_Animator;
    CharacterController m_CharacterController;

    [Header("Camera")]
    public Camera m_Camera;
    public float m_LerpRorationPct = 0.25f;

    [Header("Movement")]
    public float m_WalkSpeed = 2.5f;
    public float m_RunSpeed = 5.0f;

    [Header("Jump")]
    public float m_JumpSpeed = 5.0f;
    float m_VerticalSpeed = 0.0f;
    bool m_OnGround = true;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_CharacterController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float l_Speed = 0.0f;

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
        if (Input.GetKeyDown(KeyCode.Space) && m_OnGround)
        {
            l_HasMovement = true;
            m_VerticalSpeed = m_JumpSpeed;
        }

        l_Movement.Normalize();

        float l_MovementSpeed = 0.0f;

        if (l_HasMovement)
        {
            Quaternion l_lookAtRotation = Quaternion.LookRotation(l_Movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, l_lookAtRotation, m_LerpRorationPct);

            l_Speed = 0.5f;
            l_MovementSpeed = m_WalkSpeed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                l_Speed = 1;
                l_MovementSpeed = m_RunSpeed;
            }
        }

        m_Animator.SetFloat("Speed", l_Speed);
        l_Movement = l_Movement * l_MovementSpeed * Time.deltaTime;

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;

        CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_Movement);

        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f)
            m_VerticalSpeed = 0.0f;
        if ((l_CollisionFlags & CollisionFlags.Below) != 0)
        {
            m_VerticalSpeed = 0.0f;
            m_OnGround = true;
        }
        else
            m_OnGround = false;

        m_CharacterController.Move(l_Movement);
    }
}
