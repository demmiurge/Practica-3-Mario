using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusTransformAttribute : GameObjectStateLoadReload
{
    Vector3 m_Position;
    Quaternion m_Rotation;
    Vector3 m_Velocity;

    void Start()
    {
        SetCurrentAttributesAsDefault();
    }

    public override void SetCurrentAttributesAsDefault()
    {
        m_Position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        m_Rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);

        if (GetComponent<Rigidbody>() != null)
        {
            Rigidbody l_Rigidbody = GetComponent<Rigidbody>();
            m_Velocity = l_Rigidbody.velocity;
        }
    }

    public override void LoadDefaultAttributes()
    {
        transform.position = m_Position;
        transform.rotation = m_Rotation;

        if (GetComponent<Rigidbody>() != null)
        {
            Rigidbody l_Rigidbody = GetComponent<Rigidbody>();
            l_Rigidbody.velocity = m_Velocity;
        }
    }
}
