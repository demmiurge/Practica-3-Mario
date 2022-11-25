using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementWithRigidbody : MonoBehaviour
{
    public enum PunchType
    {
        Left_Hand = 0,
        Right_Hand,
        Foot
    }

    [Header("Animator")]
    public Animator m_Animator;

    [Header("Camera")]
    public Camera m_Camera;
    public float m_LerpRorationPct = 0.25f;
    public LayerMask m_FloorMask;
    public Transform m_FeetTransform;

    [Header("IK Foot")]
    [Range(0.0f, 1.0f)]
    public float m_DistanceToGround = 0f;
    public LayerMask m_PlayerLayerMask;

    [Header("Movement")]
    public float m_WalkSpeed = 2.5f;
    public float m_RunSpeed = 5.0f;

    [Header("Jump")]
    public float m_JumpForce = 5.0f;
    [Range(-2f, -0.0f)] 
    public float m_FallDetection = -1.0f;
    [Range(1f, 1.25f)] 
    public float m_AirJumpMultiplier = 1.05f;
    [Range(0.001f, 0.1f)] 
    public float m_RadiusSphereTolerance = 0.01f;
    int m_JumpsMade = 0;
    int m_MaximumNumberOfHops = 2;

    [Header("Punch")]
    public Collider m_LeftHandCollider;
    public Collider m_RightHandCollider;
    public Collider m_RightFootCollider;
    public float m_ComboPunchTime = 2.5f;
    float m_ComboPunchCurrentTime;
    PunchType m_CurrentComboPunch;
    bool m_IsPunchActive = false;

    [Header("Elevator")]
    public float m_ElevatorDotAngle = 0.95f;
    public Collider m_CurrentElevatorCollider = null;
    public float m_BridgeForce = 2.5f;

    // Private variables
    Vector3 m_PlayerMovementInput;

    Rigidbody m_PlayerRigidbody;

    bool m_IsDie;

    Vector3 m_StartPosition;
    Quaternion m_StartRotation;

    void Awake()
    {
        m_PlayerRigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_LeftHandCollider.gameObject.SetActive(false);
        m_RightHandCollider.gameObject.SetActive(false);
        m_RightFootCollider.gameObject.SetActive(false);

        m_ComboPunchCurrentTime -= m_ComboPunchTime;

        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;
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

        //Punch Input
        if (Input.GetButtonDown("Fire1") && CanPunch())
        {
            if (MustRestartComboPunch())
            {
                SetComboPucnch(PunchType.Right_Hand);
            }
            else
                NextComboPunch();
        }

        //Punch();

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

    /*void Punch()
    {
        if (CharacterTouchTheGround() && Input.GetButtonDown("Fire1"))
            m_Animator.SetTrigger("Punch");
    }*/
        
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

    void OnAnimatorIK(int l_LayerIndex)
    {
        if (m_Animator)
        {
            m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            m_Animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            // Left foot
            RaycastHit l_Hit;
            Ray l_Ray = new Ray(m_Animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(l_Ray, out l_Hit, m_DistanceToGround + 1f, m_PlayerLayerMask))
            {
                if (l_Hit.transform.tag == "Walkable") {
                    Vector3 l_FootPosition = l_Hit.point;
                    l_FootPosition.y += m_DistanceToGround;
                    m_Animator.SetIKPosition(AvatarIKGoal.LeftFoot, l_FootPosition);
                    m_Animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, l_Hit.normal));
                }
            }

            // Right foot
            l_Ray = new Ray(m_Animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(l_Ray, out l_Hit, m_DistanceToGround + 1f, m_PlayerLayerMask))
            {
                if (l_Hit.transform.tag == "Walkable")
                {
                    Vector3 l_FootPosition = l_Hit.point;
                    l_FootPosition.y += m_DistanceToGround;
                    m_Animator.SetIKPosition(AvatarIKGoal.RightFoot, l_FootPosition);
                    m_Animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, l_Hit.normal));
                }
            }
        }
    }

    //Punch
    public void SetPunchActiveType(PunchType PunchType, bool Active)
    {
        if (PunchType == PunchType.Left_Hand)
        {
            m_LeftHandCollider.gameObject.SetActive(Active);
        }
        else if (PunchType == PunchType.Right_Hand)
        {
            m_RightHandCollider.gameObject.SetActive(Active);
        }
        else if (PunchType == PunchType.Foot)
        {
            m_RightFootCollider.gameObject.SetActive(Active);
        }
    }

    bool CanPunch()
    {
        return !m_IsPunchActive;
    }

    bool MustRestartComboPunch()
    {
        return (Time.time - m_ComboPunchCurrentTime) > m_ComboPunchTime;
    }

    public void SetPunchActvie(bool PunchActive)
    {
        m_IsPunchActive = PunchActive;
    }

    void NextComboPunch()
    {
        if (m_CurrentComboPunch == PunchType.Right_Hand)
        {
            SetComboPucnch(PunchType.Left_Hand);
        }
        else if (m_CurrentComboPunch == PunchType.Left_Hand)
        {
            SetComboPucnch(PunchType.Foot);
        }
        else if (m_CurrentComboPunch == PunchType.Foot)
        {
            SetComboPucnch(PunchType.Right_Hand);
        }
    }

    void SetComboPucnch(PunchType PunchType)
    {
        m_CurrentComboPunch = PunchType;
        m_ComboPunchCurrentTime = Time.time;
        m_IsPunchActive = true;
        if (m_CurrentComboPunch == PunchType.Right_Hand)
        {
            m_Animator.SetTrigger("Right Hand");
        }
        else if (m_CurrentComboPunch == PunchType.Left_Hand)
        {
            m_Animator.SetTrigger("Left Hand");
        }
        else if (m_CurrentComboPunch == PunchType.Foot)
        {
            m_Animator.SetTrigger("Kick");
        }
    }

    //Restart
    public void RestartGame()
    {
        transform.position = m_StartPosition;
        transform.rotation = m_StartRotation;
    }

    //Platformers
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Elevator" && CanAttachToElevator(other))
        {
            AttachToElevator(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Elevator" && other == m_CurrentElevatorCollider)
        {
            DetachElevator();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Elevator")
        {
            if (m_CurrentElevatorCollider != null && Vector3.Dot(other.transform.up, Vector3.up) < m_ElevatorDotAngle)
            {
                DetachElevator();
            }
            if (CanAttachToElevator(other))
            {
                AttachToElevator(other);
            }
        }
    }

    bool CanAttachToElevator(Collider other)
    {
        return m_CurrentElevatorCollider == null && Vector3.Dot(other.transform.up, Vector3.up) >= m_ElevatorDotAngle;
    }

    void AttachToElevator(Collider other)
    {
        transform.SetParent(other.transform);
        m_CurrentElevatorCollider = other;
    }

    void DetachElevator()
    {
        transform.SetParent(null);
        m_CurrentElevatorCollider = null;
    }

    void LateUpdate()
    {
        if (m_CurrentElevatorCollider != null)
        {
            Vector3 l_EulerRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0.0f, l_EulerRotation.y, 0.0f);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Bridge")
        {
            hit.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal * m_BridgeForce, hit.point);
        }
    }

    // Utilities
    bool CharacterTouchTheGround() => Physics.CheckSphere(m_FeetTransform.position, m_RadiusSphereTolerance, m_FloorMask);
}
