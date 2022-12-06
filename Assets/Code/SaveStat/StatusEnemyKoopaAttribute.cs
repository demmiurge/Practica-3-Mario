using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KoopaEnemy))]

public class StatusEnemyKoopaAttribute : GameObjectStateLoadReload
{
    KoopaEnemy m_KoopaEnemy;

    int m_CurrentPunchesToKill;
    int m_CurrentPunchesToKillInitial;

    bool m_IsDead;
    bool m_IsDeadInitial;

    // Start is called before the first frame update
    void Start()
    {
        m_KoopaEnemy = GetComponent<KoopaEnemy>();

        SetCurrentAttributesAsDefault();
        m_CurrentPunchesToKillInitial = m_KoopaEnemy.GetCurrentPunches();
        m_IsDeadInitial = m_KoopaEnemy.GetIsDead();
    }

    public override void SetCurrentAttributesAsDefault()
    {
        m_CurrentPunchesToKill = m_KoopaEnemy.GetCurrentPunches();
        m_IsDead = m_KoopaEnemy.GetIsDead();
    }

    public override void LoadDefaultAttributes()
    {
         m_KoopaEnemy.SetCurrentPunches(m_CurrentPunchesToKill);
         m_KoopaEnemy.SetIsDead(m_IsDead);
    }

    public override void ResetDefaultAttributes()
    {
        m_CurrentPunchesToKill = m_CurrentPunchesToKillInitial;
        m_IsDead = m_IsDeadInitial;

        m_KoopaEnemy.SetCurrentPunches(m_CurrentPunchesToKillInitial);
        m_KoopaEnemy.SetIsDead(m_IsDeadInitial);
    }
}
