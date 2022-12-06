using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StatusTransformAttribute : GameObjectStateLoadReload
{
    Vector3 m_Position;
    Quaternion m_Rotation;
    Vector3 m_Velocity;

    Vector3 m_PositionInitial;
    Quaternion m_RotationInitial;
    Vector3 m_VelocityInitial;

    void Start()
    {
        m_PositionInitial = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        m_RotationInitial = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);

        if (GetComponent<Rigidbody>() != null)
        {
            Rigidbody l_Rigidbody = GetComponent<Rigidbody>();
            m_VelocityInitial = l_Rigidbody.velocity;
        }

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

    public override void ResetDefaultAttributes()
    {
        m_Position = m_PositionInitial;
        m_Rotation = m_RotationInitial;

        if (GetComponent<Rigidbody>() != null)
        {
            m_Velocity = m_VelocityInitial;
        }

        transform.position = m_PositionInitial;
        transform.rotation = m_RotationInitial;

        if (GetComponent<Rigidbody>() != null)
        {
            Rigidbody l_Rigidbody = GetComponent<Rigidbody>();
            l_Rigidbody.velocity = m_VelocityInitial;
        }
    }
}
