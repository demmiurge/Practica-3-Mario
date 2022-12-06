using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusFlagAttribute : GameObjectStateLoadReload
{
    Material m_Flag;
    Material m_FlagInitial;

    bool m_IsArrive;
    bool m_IsArriveInitial;

    LoadedObjectsArea m_LoadedObjectsArea;

    void Start()
    {
        m_LoadedObjectsArea = GetComponent<LoadedObjectsArea>();

        SetCurrentAttributesAsDefault();
        m_FlagInitial = m_LoadedObjectsArea.GetCurrentFlag();
        m_IsArriveInitial = m_LoadedObjectsArea.GetChangeStateFlag();
    }

    public override void SetCurrentAttributesAsDefault()
    {
        m_Flag = m_LoadedObjectsArea.GetCurrentFlag();
        m_IsArrive = m_LoadedObjectsArea.GetChangeStateFlag();
    }

    public override void LoadDefaultAttributes()
    {
        m_LoadedObjectsArea.SetNewFlag(m_Flag);
        m_LoadedObjectsArea.SetChangeStateFlag(m_IsArrive);
    }

    public override void ResetDefaultAttributes()
    {
        m_Flag = m_FlagInitial;
        m_IsArrive = m_IsArriveInitial;

        m_LoadedObjectsArea.SetNewFlag(m_FlagInitial);
        m_LoadedObjectsArea.SetChangeStateFlag(m_IsArriveInitial);
    }
}
