using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusActiveAttribute : GameObjectStateLoadReload
{
    bool m_IsActive;

    void Start()
    {
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
}
