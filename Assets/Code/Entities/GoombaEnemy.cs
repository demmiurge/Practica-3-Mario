using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoombaEnemy : MonoBehaviour
{
    public enum GoombaState
    {
        PATROL = 0,
        ALERT,
        CHASE,
        ATTACK
    }

    public GoombaState m_State;
    public float m_HitPlayerTime = 1.5f;
    public float m_HitPlayerSpeed = 2f;
    public float m_DistanceToAttack = 8f;
    public float m_WaitToAttackTime = 3f;

    NavMeshAgent m_NavMeshAgent;
    public List<Transform> m_PatrolTargets;
    public LayerMask m_VisionLayerMask;
    int m_CurrentPatrolTargetID = 0;

    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    void UpdatePatrolState()
    {
        if(PatrolTargetArrived())
        {
            MoveToNextPosition();
        }
        if(SeesPlayer())
        {
            SetAlertState();
        }
    }

    void UpdateAlertState()
    {

    }

    void UpdateChaseState()
    {

    }

    void UpdateAttackState()
    {

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
        m_State |= GoombaState.CHASE;
    }

    void SetAttackState()
    {
        m_State &= ~GoombaState.ATTACK;
    }

    bool SeesPlayer()
    {

    }
}
