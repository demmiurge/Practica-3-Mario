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
    [Range(-1f, -0.0f)] public float m_FallDetection = -0.25f;
    [Range(1f, 1.25f)] public float m_AirJumpMultiplier = 1.05f;
    int m_JumpsMade = 0;
    int m_MaximumNumberOfHops = 2;

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

        if (CheckCharacterIsFall()) 
            m_Animator.SetFloat("Speed", l_Speed);
        else
            m_Animator.SetFloat("Speed", 0);

        l_Movement += l_Movement * l_MovementSpeed;
        m_PlayerRigidbody.velocity = new Vector3(l_Movement.x, m_PlayerRigidbody.velocity.y, l_Movement.z);

        Jump();

        CheckMarioIsFall();
    }

        
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CheckCharacterIsFall())
            {
                m_Animator.SetTrigger("Jump");
                m_PlayerRigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
            }

            if (!CheckCharacterIsFall() && m_JumpsMade < m_MaximumNumberOfHops)
            {
                m_JumpsMade++;
                m_Animator.SetTrigger("Jump");
                m_Animator.SetInteger("JumpNumber", m_JumpsMade);
                m_PlayerRigidbody.velocity = Vector3.zero;
                m_PlayerRigidbody.AddForce(Vector3.up * m_JumpForce * m_AirJumpMultiplier, ForceMode.Impulse);
            }
        }

        // Restart jumps when touching ground
        if (CheckCharacterIsFall())
            m_JumpsMade = 0;
    }

    void CheckMarioIsFall()
    {
        if (CheckCharacterIsFall()) 
            m_Animator.SetTrigger("OnGround");
        if (!CheckCharacterIsFall() && m_PlayerRigidbody.velocity.y < m_FallDetection)
            m_Animator.SetBool("Falling", true);
        else
            m_Animator.SetBool("Falling", false);
    }

    void FixedUpdate()
    {
        
    }

    // Utilities
    bool CheckCharacterIsFall() => Physics.CheckSphere(m_FeetTransform.position, 0.1f, m_FloorMask);
}
