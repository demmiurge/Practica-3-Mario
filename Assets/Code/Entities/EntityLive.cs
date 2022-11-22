using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class EntityLive : MonoBehaviour
{
    [Header("Health System")]
    public float m_CurrentLive = 100;
    public float m_MaxLive = 100;
    public float m_MinLive = 0;
    
    [Header("Event receive cures")]
    public UnityEvent m_IReceivedCures;

    [Header("Event upon taking damage")] 
    public UnityEvent m_TakingDamegeEvent;

    [Header("Event on death")]
    public UnityEvent m_IHaveDiedEvent;

    void Start()
    {
        if (m_CurrentLive > m_MaxLive)
            m_CurrentLive = m_MaxLive;
    }

    public void Hit(float l_DamageCaused)
    {
        CalculateNewLife(l_DamageCaused);
    }

    public void Healing(float l_CuresCaused)
    {
        if (m_CurrentLive + l_CuresCaused > m_MaxLive)
            m_CurrentLive = m_MaxLive;
        else
            m_CurrentLive -= l_CuresCaused;

        m_IReceivedCures?.Invoke();
    }

    void CalculateNewLife(float l_DamageCaused)
    {
        if (m_CurrentLive - l_DamageCaused > m_MinLive)
            Hitted(l_DamageCaused);
        else
            Die();
    }

    void Hitted(float l_DamageCaused)
    {
        m_CurrentLive -= l_DamageCaused;
        m_TakingDamegeEvent?.Invoke();
    }

    void Die()
    {
        m_CurrentLive = m_MinLive;
        m_IHaveDiedEvent?.Invoke();
    }
}