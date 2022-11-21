using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementWithRigidbody : MonoBehaviour
{
    public Animator m_Animator;

    Rigidbody m_PlayerRigidbody;
    private Vector3 m_PlayerMovementInput;

    [Header("Camera")]
    [SerializeField] private Camera m_Camera;
    [SerializeField] private float m_LerpRorationPct = 0.25f;

    [Header("Camera")]
    [SerializeField] private LayerMask m_FloorMask;
    [SerializeField] private Transform m_FeetTransform;

    [Header("Movement")]
    public float m_WalkSpeed = 2.5f;
    public float m_RunSpeed = 5.0f;

    [Header("Jump")]
    public float m_JumpForce = 5.0f;
    [Range(-2f, -0.0f)] public float m_FallDetection = -1.0f;
    [Range(1f, 1.25f)] public float m_AirJumpMultiplier = 1.05f;
    [Range(0.001f, 0.1f)] public float m_RadiusSphereTolerance = 0.01f;
    int m_JumpsMade = 0;
    int m_MaximumNumberOfHops = 2;

    // Utilities
    bool m_IsDie;

    void Awake()
    {
        m_PlayerRigidbody = GetComponent<Rigidbody>();
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

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            l_HasMovement = true;
        if (Input.GetKey(KeyCode.W))
            l_Movement = l_ForwardsCamera;
        if (Input.GetKey(KeyCode.A))
            l_Movement -= l_RightCamera;
        if (Input.GetKey(KeyCode.S))
            l_Movement = -l_ForwardsCamera;
        if (Input.GetKey(KeyCode.D))
            l_Movement += l_RightCamera;

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

        l_Movement += l_Movement * l_MovementSpeed;
        m_PlayerRigidbody.velocity = new Vector3(l_Movement.x, m_PlayerRigidbody.velocity.y, l_Movement.z);

        Punch();

        Jump();

        Die();

        CheckMarioIsFall();

        m_Animator.SetFloat("Speed", l_Speed);
    }

    void Die()
    {
        if (Input.GetKeyDown(KeyCode.N)) 
        {
            m_Animator.SetBool("Die", true);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            m_Animator.SetBool("Die", false);
        }
    }

    void Punch()
    {
        if (CharacterTouchTheGround() && Input.GetButtonDown("Fire1"))
            m_Animator.SetTrigger("Punch");
    }
        
    void Jump()
    {
        // Restart jumps when touching ground
        if (CharacterTouchTheGround())
        {
            m_JumpsMade = 0;
            m_Animator.SetInteger("JumpNumber", 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CharacterTouchTheGround())
            {
                m_Animator.SetTrigger("Jump");
                m_PlayerRigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
            }

            if (!CharacterTouchTheGround() && m_JumpsMade < m_MaximumNumberOfHops)
            {
                m_JumpsMade++;
                m_Animator.SetTrigger("Jump");
                m_Animator.SetInteger("JumpNumber", m_JumpsMade);
                m_PlayerRigidbody.velocity = Vector3.zero;
                m_PlayerRigidbody.AddForce(Vector3.up * m_JumpForce * m_AirJumpMultiplier, ForceMode.Impulse);
            }
        }
    }

    void CheckMarioIsFall()
    {
        m_Animator.SetBool("OnGround", CharacterTouchTheGround());
        if (!CharacterTouchTheGround() && m_PlayerRigidbody.velocity.y < m_FallDetection)
            m_Animator.SetBool("Falling", true);
        else
            m_Animator.SetBool("Falling", false);
    }

    void FixedUpdate()
    {
        
    }

    // Utilities
    bool CharacterTouchTheGround() => Physics.CheckSphere(m_FeetTransform.position, m_RadiusSphereTolerance, m_FloorMask);
}
