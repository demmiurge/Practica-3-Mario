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
    public float m_JumpSpeed = 5.0f;
    float m_VerticalSpeed = 0.0f;
    bool m_OnGround = true;

    Vector3 m_MoveVector;

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

        m_MoveVector = l_Movement * m_WalkSpeed;
        m_MoveVector.Normalize();
        m_PlayerRigidbody.velocity = new Vector3(m_MoveVector.x * m_WalkSpeed, m_PlayerRigidbody.velocity.y, m_MoveVector.z * m_WalkSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
            if (Physics.CheckSphere(m_FeetTransform.position, 0.1f, m_FloorMask))
                m_PlayerRigidbody.AddForce(Vector3.up * m_JumpSpeed, ForceMode.Impulse);
    }

    private void MovePlayer()
    {


        m_MoveVector = transform.TransformDirection(m_PlayerMovementInput) * m_WalkSpeed;
        m_PlayerRigidbody.velocity = new Vector3(m_MoveVector.x, m_PlayerRigidbody.velocity.y, m_MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space))
            if (Physics.CheckSphere(m_FeetTransform.position, 0.1f, m_FloorMask))
                m_PlayerRigidbody.AddForce(Vector3.up * m_JumpSpeed, ForceMode.Impulse);
    }
}
