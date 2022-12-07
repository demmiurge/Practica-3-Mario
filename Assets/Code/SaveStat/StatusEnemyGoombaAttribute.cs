using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoombaEnemy))]

public class StatusEnemyGoombaAttribute : GameObjectStateLoadReload
{
    GoombaEnemy m_GoombaEnemy;

    int m_CurrentPunchesToKill;
    int m_CurrentPunchesToKillInitial;

    bool m_IsDead;
    bool m_IsDeadInitial;

    // Start is called before the first frame update
    void Start()
    {
        m_GoombaEnemy = GetComponent<GoombaEnemy>();

        SetCurrentAttributesAsDefault();
        m_CurrentPunchesToKillInitial = m_GoombaEnemy.GetCurrentPunches();
        m_IsDeadInitial = m_GoombaEnemy.GetIsDead();
    }

    public override void SetCurrentAttributesAsDefault()
    {
        m_CurrentPunchesToKill = m_GoombaEnemy.GetCurrentPunches();
        m_IsDead = m_GoombaEnemy.GetIsDead();
    }

    public override void LoadDefaultAttributes()
    {
        m_GoombaEnemy.SetCurrentPunches(m_CurrentPunchesToKill);
        m_GoombaEnemy.SetIsDead(m_IsDead);
    }

    public override void ResetDefaultAttributes()
    {
        m_CurrentPunchesToKill = m_CurrentPunchesToKillInitial;
        m_IsDead = m_IsDeadInitial;

        m_GoombaEnemy.SetCurrentPunches(m_CurrentPunchesToKillInitial);
        m_GoombaEnemy.SetIsDead(m_IsDeadInitial);
    }
}
