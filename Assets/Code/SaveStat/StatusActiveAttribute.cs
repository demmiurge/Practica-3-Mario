using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusActiveAttribute : GameObjectStateLoadReload
{
    bool m_IsActiveInitial;
    bool m_IsActive;

    void Start()
    {
        m_IsActiveInitial = gameObject.activeSelf;
        SetCurrentAttributesAsDefault();
    }

    public override void SetCurrentAttributesAsDefault()
    {
        m_IsActive = gameObject.activeSelf;
    }

    public override void LoadDefaultAttributes()
    {
        gameObject.SetActive(m_IsActive);
    }

    public override void ResetDefaultAttributes()
    {
        m_IsActive = m_IsActiveInitial;
        gameObject.SetActive(m_IsActiveInitial);
    }
}
