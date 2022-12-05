using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoombaEnemy : MonoBehaviour, IRestartGame
{
    public enum GoombaState
    {
        PATROL = 0,
        ALERT,
        CHASE,
        ATTACK
    }

    public Animator m_Animator;

    public GoombaState m_State;
    public float m_HitPlayerTime = 1.5f;
    public float m_HitPlayerSpeed = 1f;
    public float m_DistanceToAttack = 0.1f;
    public float m_WaitToAttackTime = 3f;

    public float m_VisualConeAngle = 60.0f;
    public float m_SightDistance = 8.0f;
    public float m_EyesHeight = 1f;
    public float m_EyesPlayerHeight = 1.5f;
    public float m_HearRangeDistance = 20.0f;
    public float m_KillTime = 0.5f;
    public float m_KillScale = 0.2f;

    NavMeshAgent m_NavMeshAgent;
    public List<Transform> m_PatrolTargets;
    public LayerMask m_VisionLayerMask;
    int m_CurrentPatrolTargetID = 0;

    int m_NumPunches = 0;

    Vector3 m_StartScale;

    //Patrolling, when Mario is close it turns facing Mario, jumps and starts going towards him, if Marion changes position, Goomba doesn't change direction and stops after colliding or in certan
    //time
    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameController.GetGameController().AddRestartGameElements(this);
        m_StartScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_State)
        {
            case GoombaState.PATROL:
                UpdatePatrolState();
                break;
            case GoombaState.ALERT:
                UpdateAlertState();
                break;
            case GoombaState.CHASE:
                UpdateChaseState();
                break;
            case GoombaState.ATTACK:
                UpdateAttackState();
                break;
        }

        if(m_NumPunches > 2)
        {
            StartCoroutine(KillGoomba());
        }
    }

    void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    void UpdatePatrolState()
    {
        m_Animator.SetBool("Chasing", false);
        if (PatrolTargetArrived())
        {
            MoveToNextPosition();
        }
        if(HearsPlayer())
        {
            SetAlertState();
        }
        else
        {
            SetPatrolState();
        }
    }

    void UpdateAlertState()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
        m_Animator.SetBool("Alert", true);
        if (SeesPlayer())
            SetChaseState();
        else
            SetPatrolState();
    }

    void UpdateChaseState()
    {
        m_Animator.SetBool("Alert", false);
        m_Animator.SetBool("Chasing", true);
        m_NavMeshAgent.destination = GameController.GetGameController().GetPlayer().transform.position;
        if(InDistanceToAttack())
        {
            SetAttackState();
        }
        if (!HearsPlayer())
        {
            SetPatrolState();
        }
    }

    bool InDistanceToAttack()
    {
        return (transform.position - GameController.GetGameController().GetPlayer().transform.position).magnitude <= m_DistanceToAttack;
    }

    void UpdateAttackState()
    {
        if(InDistanceToAttack())
        {
            Debug.Log("attacking");
        }
        else
        {
            SetChaseState();
        }
    }

    void SetPatrolState()
    {
        m_State = GoombaState.PATROL;
        m_NavMeshAgent.destination = m_PatrolTargets[m_CurrentPatrolTargetID].position;
    }

    bool PatrolTargetArrived()
    {
        return !m_NavMeshAgent.hasPath && !m_NavMeshAgent.pathPending && m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
    }

    void MoveToNextPosition()
    {
        ++m_CurrentPatrolTargetID;
        if(m_CurrentPatrolTargetID >= m_PatrolTargets.Count)
        {
            m_CurrentPatrolTargetID = 0;
        }
        m_NavMeshAgent.destination = m_PatrolTargets[m_CurrentPatrolTargetID].position;
    }

    void SetAlertState()
    {
        m_State = GoombaState.ALERT;
    }

    void SetChaseState()
    {
        m_State = GoombaState.CHASE;
    }

    void SetAttackState()
    {
        m_State = GoombaState.ATTACK;
    }

    bool SeesPlayer()
    {

        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        Vector3 l_DirectionToPlayerXZ = l_PlayerPosition - transform.position;
        l_DirectionToPlayerXZ.y = 0.0f;
        l_DirectionToPlayerXZ.Normalize();

        Vector3 l_ForwardXZ = transform.forward;
        l_ForwardXZ.y = 0.0f;
        l_ForwardXZ.Normalize();

        Vector3 l_EyesPosition = transform.position + Vector3.up * m_EyesHeight;
        Vector3 l_PlayerEyesPosition = l_PlayerPosition + Vector3.up * m_EyesPlayerHeight;
        Vector3 l_Direction = l_PlayerEyesPosition - l_EyesPosition;

        float l_Lenght = l_Direction.magnitude;
        l_Direction /= l_Lenght;

        Ray l_Ray = new Ray(l_EyesPosition, l_Direction);

        return Vector3.Distance(l_PlayerPosition, transform.position) < m_VisualConeAngle &&
               Vector3.Dot(l_ForwardXZ, l_DirectionToPlayerXZ) > Mathf.Cos(m_VisualConeAngle * Mathf.Deg2Rad / 2.0f) &&
               !Physics.Raycast(l_Ray, l_Lenght, m_VisionLayerMask.value);
    }

    bool HearsPlayer()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        return Vector3.Distance(l_PlayerPosition, transform.position) <= m_HearRangeDistance;
    }
    public void Kill()
    {
        transform.localScale = new Vector3(1.0f, m_KillScale, 1.0f);
        StartCoroutine(KillGoomba());
    }

    IEnumerator KillGoomba()
    {
        yield return new WaitForSeconds(m_KillTime);
        gameObject.SetActive(false);
        m_NumPunches = 0;
    }

    public void RestartGame()
    {
        transform.localScale = m_StartScale;
        gameObject.SetActive(true);
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "MarioHit")
        {
            m_NumPunches++;
        }
    }

    IEnumerator SetChase(float Time)
    {
        yield return new WaitForSeconds(Time);
        SetChaseState();
    }
}
