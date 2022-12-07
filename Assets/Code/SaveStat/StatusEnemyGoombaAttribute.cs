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

    Vector3 m_Scale;
    Vector3 m_ScaleInitial;

    // Start is called before the first frame update
    void Start()
    {
        m_GoombaEnemy = GetComponent<GoombaEnemy>();

        SetCurrentAttributesAsDefault();
        m_CurrentPunchesToKillInitial = m_GoombaEnemy.GetCurrentPunches();
        m_IsDeadInitial = m_GoombaEnemy.GetIsDead();
        m_ScaleInitial = m_GoombaEnemy.gameObject.transform.localScale;
    }

    public override void SetCurrentAttributesAsDefault()
    {
        m_CurrentPunchesToKill = m_GoombaEnemy.GetCurrentPunches();
        m_IsDead = m_GoombaEnemy.GetIsDead();
        m_Scale = m_GoombaEnemy.gameObject.transform.localScale;
    }

    public override void LoadDefaultAttributes()
    {
        m_GoombaEnemy.SetCurrentPunches(m_CurrentPunchesToKill);
        m_GoombaEnemy.SetIsDead(m_IsDead);
        m_GoombaEnemy.transform.localScale = m_Scale;
    }

    public override void ResetDefaultAttributes()
    {
        m_CurrentPunchesToKill = m_CurrentPunchesToKillInitial;
        m_IsDead = m_IsDeadInitial;
        m_Scale = m_ScaleInitial;

        m_GoombaEnemy.SetCurrentPunches(m_CurrentPunchesToKillInitial);
        m_GoombaEnemy.SetIsDead(m_IsDeadInitial);
        m_GoombaEnemy.transform.localScale = m_ScaleInitial;
    }
}
