using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstantDeath : MonoBehaviour
{
    [Header("Event sudden death")] 
    public UnityEvent m_SuddentDeath;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "DeathZone") return;
        gameObject.gameObject.SetActive(false); // We also make the enemy disappear
        m_SuddentDeath?.Invoke();
    }
}
