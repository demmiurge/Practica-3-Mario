using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MarioLife))]
public class StatusLifeAttribute : GameObjectStateLoadReload
{
    MarioLife m_MarioLife;

    float m_CurrentLife;
    float m_CurrentLifeInitial;

    // Start is called before the first frame update
    void Start()
    {
        m_MarioLife = GetComponent<MarioLife>();

        SetCurrentAttributesAsDefault();
        m_CurrentLifeInitial = m_MarioLife.GetCurrentLife();
    }

    public override void SetCurrentAttributesAsDefault()
    {
        m_CurrentLife = m_MarioLife.GetCurrentLife();
    }

    public override void LoadDefaultAttributes()
    {
        m_MarioLife.SetCurrentLife(m_CurrentLife);
    }

    public override void ResetDefaultAttributes()
    {
        m_CurrentLife = m_CurrentLifeInitial;

        m_MarioLife.SetCurrentLife(m_CurrentLifeInitial);
    }
}
