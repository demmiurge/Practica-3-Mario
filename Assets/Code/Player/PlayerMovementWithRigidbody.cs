using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementWithRigidbody : MonoBehaviour, IRestartGame
{
    public enum Turtle
    {
        Take = 0,
        Throw
    }
    public enum PunchType
    {
        Left_Hand = 0,
        Right_Hand,
        Foot
    }

    public enum JumpType
    {
        Single = 0,
        Double,
        Triple,
        Long,
        Wall
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
    public float m_FirstJumpForce = 3.0f;
    public float m_SecondJumpForce = 4.0f;
    public float m_ThirdJumpForce = 6.0f;
    [Range(-2f, -0.0f)] 
    public float m_FallDetection = -1.0f;
    [Range(1f, 1.25f)] 
    public float m_AirJumpMultiplier = 1.05f;
    [Range(0.001f, 0.1f)] 
    public float m_RadiusSphereTolerance = 0.01f;
    bool m_IsJumpActive = false;
    public float m_ComboJumpTime = 2.5f;
    float m_ComboJumpCurrentTime;
    JumpType m_CurrentJumpType;
    bool m_Falling = false;
    public LayerMask m_WallJumpLayer;
    public Transform m_EyesHeight;


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
    float m_IdleTime = 0;
    float m_CameraRepos = 0;

    Vector3 m_StartPosition;
    Quaternion m_StartRotation;

    public Checkpoint m_Checkpoint = null;
    [Header("Kills")]
    public float m_MaxKillAngle = 45.0f;
    public float m_KillerJumpSpeed = 5.0f;

    [Header("Bouncing")] 
    public float m_TimeToBouncing = 2;
    float m_TimeToBounce = 4;
    float m_CurrentBouncing = 0;
    bool m_IsBouncing;
    bool m_IsWallJumping = false;

    bool m_CanMove = true;
    bool m_IsCrouch = false;
    bool m_HasJumped = false;

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
        m_ComboJumpCurrentTime -= m_ComboJumpTime;

        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;

        GameController.GetGameController().AddRestartGameElements(this);
        GameController.GetGameController().SetPlayer(this);
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
        if (m_CanMove)
        {
            /*if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                l_HasMovement = true;*/

            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
                l_HasMovement = true;
            if(Input.GetButton("Horizontal"))
            {
                l_Movement = Input.GetAxis("Horizontal") * l_RightCamera;
            }
            if (Input.GetButton("Vertical"))
            {
                l_Movement = Input.GetAxis("Vertical") * l_ForwardsCamera;
            }
            /*if (Input.GetKey(KeyCode.W))
                l_Movement = l_ForwardsCamera;
            if (Input.GetKey(KeyCode.A))
                l_Movement -= l_RightCamera;
            if (Input.GetKey(KeyCode.S))
                l_Movement = -l_ForwardsCamera;
            if (Input.GetKey(KeyCode.D))
                l_Movement += l_RightCamera;*/
        }
        l_Movement.Normalize();

        float l_MovementSpeed = 0.0f;

        if (l_HasMovement)
        {
            m_IdleTime = 0;
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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (m_IsCrouch == false)
            {
                m_Animator.SetBool("Crouching", true);
                m_IsCrouch = true;
                m_CanMove = false;
            }
            else
            {
                m_IsCrouch = false;
                m_Animator.SetBool("Crouching", false);
                m_CanMove = true;
            }
        }

        if (l_HasMovement == false && m_HasJumped == false)
        {
            m_IdleTime += Time.deltaTime;
            m_CameraRepos += Time.deltaTime;
        }

        if(m_CameraRepos >= 5)
        {
           m_Camera.GetComponent<CameraController>().m_HasToReset = true;
            m_CameraRepos = 0;
        }

        if (m_IdleTime >= 10)
        {
            m_PlayerRigidbody.AddForce(Vector3.up * m_ThirdJumpForce, ForceMode.Impulse);
            m_Animator.SetTrigger("Special");
            m_IdleTime = 0;
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

        //Jump input
        if (Input.GetButtonDown("Jump") && CanJump() && m_Falling == false)
        {
            m_HasJumped = true;
            if (m_IsCrouch == false)
            {
                if (MustRestartJumpCombo())
                {
                    SetJumpType(JumpType.Single);
                    // m_PlayerRigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse); // DEPRECATED
                }
                else
                {
                    NextJump();
                    m_PlayerRigidbody.velocity = Vector3.zero;
                    // m_PlayerRigidbody.AddForce(Vector3.up * m_JumpForce * m_AirJumpMultiplier, ForceMode.Impulse); // DEPRECATED
                }
            }
            else
            {
                SetJumpType(JumpType.Long);
                m_PlayerRigidbody.velocity = Vector3.zero;
            }
        }

        //Wall jump
        Ray l_Ray = new Ray(m_EyesHeight.position, m_PlayerRigidbody.transform.forward);
        Debug.DrawRay(l_Ray.origin, l_Ray.direction * 0.25f, Color.red);
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(l_Ray, out l_RaycastHit, 0.25f, m_WallJumpLayer.value))
        {
            if (m_IsJumpActive == true)
            {
                //Debug.DrawRay(l_RaycastHit.normal, l_Ray.direction * 0.25f, Color.red);
                //m_PlayerRigidbody.rotation = Quaternion.Euler(l_RaycastHit.normal);
                transform.rotation = Quaternion.FromToRotation(transform.forward, l_RaycastHit.normal) * transform.rotation;
                Debug.DrawRay(l_RaycastHit.point, l_RaycastHit.normal * 2f, Color.blue);
                //Debug.Break();
                m_CanMove = false;
                m_IsWallJumping = true;
                m_CurrentBouncing = m_TimeToBouncing;
                StartCoroutine(EnableMovement(1));
            }
        }
        //Debug.DrawLine(m_EyesHeight.position, l_RaycastHit.point,  Color.red);       

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
       
    // DEPRECATED
    /*void Jump()
    {
        // Restart jumps when touching ground
        if (CharacterTouchTheGround())
        {
            Debug.Log("Jumps restarted");
            m_JumpsMade = 0;
            m_Animator.SetInteger("JumpNumber", 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CharacterTouchTheGround())
            {
                m_Animator.SetTrigger("Single");
                SetJumpType(JumpType.Single);
                m_PlayerRigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
            }

            if (!CharacterTouchTheGround() && m_JumpsMade < m_MaximumNumberOfHops)
            {
                m_JumpsMade++;
                //m_Animator.SetTrigger("Jump");
                NextJump();
                m_Animator.SetInteger("JumpNumber", m_JumpsMade);
                m_PlayerRigidbody.velocity = Vector3.zero;
                m_PlayerRigidbody.AddForce(Vector3.up * m_JumpForce * m_AirJumpMultiplier, ForceMode.Impulse);
            }
        }
    }*/

    void CheckMarioIsFall()
    {
        m_Animator.SetBool("OnGround", CharacterTouchTheGround());
        if (!CharacterTouchTheGround() && m_PlayerRigidbody.velocity.y < m_FallDetection)
        {
            m_Animator.SetBool("Falling", true);
            m_Falling = true;
        }
        else
        {
            m_Falling = false;
            m_Animator.SetBool("Falling", false);
            m_HasJumped = false;
        }
    }

    void FixedUpdate()
    {
        if (m_IsBouncing)
        {
            m_CurrentBouncing -= Time.deltaTime;
            m_PlayerRigidbody.AddForce(-m_PlayerRigidbody.transform.forward * m_CurrentBouncing, ForceMode.Impulse);
            
        }
        if(m_IsWallJumping)
        {
            //float l_NewRotation = -m_PlayerRigidbody.transform.rotation.y;
            //m_PlayerRigidbody.transform.rotation = Quaternion.Euler(0.0f, -m_PlayerRigidbody.transform.rotation.eulerAngles.y, 0.0f);
            m_CurrentBouncing -= Time.deltaTime;
            m_PlayerRigidbody.AddForce(m_PlayerRigidbody.transform.forward * 5, ForceMode.Impulse);
        }
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

    // Jumps

    public void SetJumpActiveType(JumpType jumpType)
    {
        m_CurrentJumpType = jumpType;
    }

    public void SetJumpType(JumpType JumpType)
    {
        m_CurrentJumpType = JumpType;
        m_ComboJumpCurrentTime = Time.time;
        m_IsJumpActive = true;
        if (JumpType == JumpType.Single)
        {
            m_PlayerRigidbody.AddForce(Vector3.up * m_FirstJumpForce, ForceMode.Impulse);
            m_Animator.SetTrigger("SingleJump");
        }
        else if (JumpType == JumpType.Double)
        {
            m_PlayerRigidbody.AddForce(Vector3.up * m_SecondJumpForce, ForceMode.Impulse);
            m_Animator.SetTrigger("DoubleJump");
        }
        else if (JumpType == JumpType.Triple)
        {
            m_PlayerRigidbody.AddForce(Vector3.up * m_ThirdJumpForce, ForceMode.Impulse);
            m_Animator.SetTrigger("TripleJump");
        }
        else if (JumpType == JumpType.Long)
        {
            m_PlayerRigidbody.AddForce(Vector3.up * m_ThirdJumpForce, ForceMode.Impulse);
            m_Animator.SetBool("Crouching", false);
            m_Animator.SetTrigger("LongJump");
            m_IsCrouch = false;
            m_CanMove = true;
        }
    }

    bool MustRestartJumpCombo()
    {
        return (Time.time - m_ComboJumpCurrentTime) > m_ComboJumpTime;
    }

    bool CanJump()
    {
        return !m_IsJumpActive;
    }

    public void SetJumpActive(bool jumpActive)
    {
        m_IsJumpActive = jumpActive;
    }

    void NextJump()
    {
        if (m_CurrentJumpType == JumpType.Single) SetJumpType(JumpType.Double);
        else if (m_CurrentJumpType == JumpType.Double) SetJumpType(JumpType.Triple);
        else if (m_CurrentJumpType == JumpType.Triple) SetJumpType(JumpType.Single);
    }

    // Punch
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
        if (m_Checkpoint == null)
        {
            transform.position = m_StartPosition;
            transform.rotation = m_StartRotation;
        }
        else
        {
            transform.position = m_Checkpoint.m_SpawnPosition.transform.position;
            transform.rotation = m_Checkpoint.m_SpawnPosition.transform.rotation;
        }
    }

    //Platformers
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Elevator" && CanAttachToElevator(other))
        {
            AttachToElevator(other);
        }

        if(other.tag == "Checkpoint")
        {
            m_Checkpoint = other.GetComponent<Checkpoint>();
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

    void JumpOverEnemy()
    {
        m_PlayerRigidbody.AddForce(Vector3.up * m_ThirdJumpForce, ForceMode.Impulse);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        /*Debug.Log("hit goomba"+ hit.gameObject.tag);
        if (hit.gameObject.tag == "Bridge")
        {
            hit.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal * m_BridgeForce, hit.point);
        }

        else if(hit.gameObject.tag == "Goomba")
        {
            Debug.Log("hit goomba");
            if(CanKillGoomba(hit.normal))
            {
                hit.gameObject.GetComponent<GoombaEnemy>().Kill();
                JumpOverEnemy();
                Debug.Log("kill goomba");
            }
            else
            {
                Debug.DrawRay(hit.point, hit.normal*3.0f, Color.blue, 5.0f);
                Debug.Break();
            }
            //Debug.DrawRay();
        }*/
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        if (collision.gameObject.tag == "Goomba" && CanKillGoomba(normal))
        {
            collision.gameObject.GetComponent<GoombaEnemy>().Kill();
            JumpOverEnemy();
        }
        else if(collision.gameObject.tag == "Goomba")
        {
            m_CanMove = false;
            m_Animator.SetTrigger("Hit");
            m_IsBouncing = true;
            m_CurrentBouncing = m_TimeToBouncing;
            //m_PlayerRigidbody.AddForce(Vector3.up * m_ThirdJumpForce/2, ForceMode.Impulse);
            StartCoroutine(EnableMovement(m_TimeToBouncing));
        }
        else if (collision.gameObject.tag == "Koopa")
        {
            m_CanMove = false;
            m_Animator.SetTrigger("Hit");
            m_IsBouncing = true;
            m_CurrentBouncing = m_TimeToBouncing;
            //m_PlayerRigidbody.AddForce(Vector3.up * m_ThirdJumpForce/2, ForceMode.Impulse);
            StartCoroutine(EnableMovement(m_TimeToBouncing));
        }
    }

    bool CanKillGoomba(Vector3 Normal)
    {
        return Vector3.Dot(Normal, Vector3.up) >= Mathf.Cos(m_MaxKillAngle * Mathf.Deg2Rad);
    }

    // Utilities
    bool CharacterTouchTheGround() => Physics.CheckSphere(m_FeetTransform.position, m_RadiusSphereTolerance, m_FloorMask);

    IEnumerator EnableMovement(float l_Time)
    {
        yield return new WaitForSeconds(l_Time);
        m_CanMove = true;
        m_IsBouncing = false;
        m_IsWallJumping = false;
    }
}
