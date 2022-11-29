using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class MarioLive : MonoBehaviour
{
    private bool m_CanTakeDamage = false;
    private bool m_HUDIsVisible = false;
    private bool m_OnStartCounter = false;
    private float m_TimerHUD;

    [Header("Health HUD")] 
    public Animation m_LifePower;
    public AnimationClip m_ShowPower;
    public AnimationClip m_HidePower;
    [Range(8.0f, 32.0f)] public float m_TimeToDisappear = 8.0f;
    public float m_TimeBetweenRemoveLifeTolerance = 0.5f;
    public float m_TimeBetweenAddLifeTolerance = 0.5f;

    [Header("Health HUD Colors")]
    public Image m_ImageCurrentLive;
    public Color m_BaseColorLive = new Color(0, 0, 1, 1);
    public Color m_FirstColorLive = new Color(0,1,0,1);
    public Color m_SecondColorLive = new Color(1, 1, 0, 1);
    public Color m_ThirdColorLive = new Color(1, 0, 0, 1);
    public Image m_ImageMaxLive;
    public Color m_BaseColorMaxLive = new Color(0, 0, 0.5f, 1);
    public Color m_FirstColorMaxLive = new Color(0, 0.5f, 0, 1);
    public Color m_SecondColorMaxLive = new Color(0.5f, 0.5f, 0, 1);
    public Color m_ThirdColorMaxLive = new Color(0.5f, 0, 0, 1);

    [Header("Health System")]
    public float m_CurrentLive = 8;
    public float m_MaxLive = 8;
    public float m_MinLive = 0;
    
    [Header("Event receive cures")]
    public UnityEvent m_IReceivedCures;

    [Header("Event upon taking damage")] 
    public UnityEvent m_TakingDamegeEvent;

    [Header("Event on death")]
    public UnityEvent m_IHaveDiedEvent;

    void Start()
    {
        UpdateLiveParametersHUD(); // Enter live parameters in the HUD

        m_CanTakeDamage = true; // Whenever they start they can take damage

        if (m_CurrentLive > m_MaxLive)
            m_CurrentLive = m_MaxLive;
    }

    public void ChangeInvincibilityStatus(bool l_CanTakeDamage) => m_CanTakeDamage = l_CanTakeDamage;
    public bool CanIHitIt() => m_CanTakeDamage;

    public void UpdateLiveParametersHUD()
    {
        m_ImageCurrentLive.fillAmount = m_CurrentLive / m_MaxLive;

        if (m_CurrentLive > m_MaxLive / 2 + m_MaxLive / 4)
        {
            m_ImageCurrentLive.color = m_BaseColorLive;
            m_ImageMaxLive.color = m_BaseColorMaxLive;
        }
        else if (m_CurrentLive > m_MaxLive / 2)
        {
            m_ImageCurrentLive.color = m_FirstColorLive;
            m_ImageMaxLive.color = m_FirstColorMaxLive;
        } 
        else if (m_CurrentLive > m_MaxLive / 4)
        {
            m_ImageCurrentLive.color = m_SecondColorLive;
            m_ImageMaxLive.color = m_SecondColorMaxLive;
        }
        else
        {
            m_ImageCurrentLive.color = m_ThirdColorLive;
            m_ImageMaxLive.color = m_ThirdColorMaxLive;
        }
    }

    void Update() // FEO
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Hit(1);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Healing(1);
        }

        if (m_OnStartCounter)
        {
            m_TimerHUD += Time.deltaTime;
            if (m_TimerHUD >= m_TimeToDisappear)
            {
                m_TimerHUD = 0;
                m_LifePower.Play(m_HidePower.name);
                m_HUDIsVisible = false;
                m_OnStartCounter = false;
            }
        }
    }

    public void Healing(float l_CuresCaused)
    {
        if (!m_HUDIsVisible)
        {
            m_HUDIsVisible = true;
            m_LifePower.Play(m_ShowPower.name);
            StartCoroutine(CanReturnToGetHeal(m_ShowPower.length + m_TimeBetweenAddLifeTolerance, l_CuresCaused));
        }
        else
            StartCoroutine(CanReturnToGetHeal(m_TimeBetweenAddLifeTolerance, l_CuresCaused));
    }

    IEnumerator CanReturnToGetHeal(float l_Duration, float l_CuresCaused)
    {
        m_TimerHUD = 0;
        yield return new WaitForSeconds(l_Duration);
        Heal(l_CuresCaused);
        UpdateLiveParametersHUD();
    }

    void Heal(float l_CuresCaused)
    {
        if (m_CurrentLive + l_CuresCaused > m_MaxLive)
            m_CurrentLive = m_MaxLive;
        else
            m_CurrentLive += l_CuresCaused;

        m_IReceivedCures?.Invoke();
    }

    public void Hit(float l_DamageCaused)
    {
        if (!m_CanTakeDamage) return;
        m_CanTakeDamage = false;

        if (!m_HUDIsVisible)
        {
            m_HUDIsVisible = true;
            m_LifePower.Play(m_ShowPower.name);
            StartCoroutine(CanReturnToGetDamage(m_ShowPower.length + m_TimeBetweenRemoveLifeTolerance, l_DamageCaused));
        }
        else
            StartCoroutine(CanReturnToGetDamage(m_TimeBetweenRemoveLifeTolerance, l_DamageCaused));
    }

    IEnumerator CanReturnToGetDamage(float l_Duration, float l_DamageCaused)
    {
        m_TimerHUD = 0;
        yield return new WaitForSeconds(l_Duration);
        CalculateNewLife(l_DamageCaused);
        UpdateLiveParametersHUD();
        m_CanTakeDamage = true;
        m_OnStartCounter = true;
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
        Debug.Log("Slap me more Veronika");
        m_TakingDamegeEvent?.Invoke();
    }

    void Die()
    {
        m_CurrentLive = m_MinLive;
        Debug.Log("I die");
        m_IHaveDiedEvent?.Invoke();
    }
}