using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class MarioLife : MonoBehaviour
{
    private bool m_CanTakeDamage = false;
    private bool m_HUDIsVisible = false;
    private bool m_OnStartCounter = false;
    private bool m_MaxCurrentLife = false;
    private float m_TimerHUD;

    [Header("Lives")] 
    public TextMeshProUGUI m_TextLives;

    [Header("Health HUD")] 
    public Animation m_LifePower;
    public AnimationClip m_ShowPower;
    public AnimationClip m_HidePower;
    [Range(8.0f, 32.0f)] public float m_TimeToDisappear = 8.0f;
    public float m_TimeBetweenRemoveLifeTolerance = 0.5f;
    public float m_TimeBetweenAddLifeTolerance = 0.5f;

    [Header("Health HUD Colors")]
    public Image m_ImageCurrentLife;
    public Color m_BaseColorLife = new Color(0, 0, 1, 1);
    public Color m_FirstColorLife = new Color(0,1,0,1);
    public Color m_SecondColorLife = new Color(1, 1, 0, 1);
    public Color m_ThirdColorLife = new Color(1, 0, 0, 1);
    public Image m_ImageMaxLife;
    public Color m_BaseColorMaxLife = new Color(0, 0, 0.5f, 1);
    public Color m_FirstColorMaxLife = new Color(0, 0.5f, 0, 1);
    public Color m_SecondColorMaxLife = new Color(0.5f, 0.5f, 0, 1);
    public Color m_ThirdColorMaxLife = new Color(0.5f, 0, 0, 1);

    [Header("Health System")]
    public float m_CurrentLife = 8;
    public float m_MaxLife = 8;
    public float m_MinLife = 0;
    
    [Header("Event receive cures")]
    public UnityEvent m_IReceivedCures;

    [Header("Event upon taking damage")] 
    public UnityEvent m_TakingDamegeEvent;

    [Header("Event on death")]
    public UnityEvent m_IHaveDiedEvent;

    void Start()
    {
        UpdateLifeParametersHUD(); // Enter life parameters in the HUD

        m_CanTakeDamage = true; // Whenever they start they can take damage

        if (m_CurrentLife >= m_MaxLife)
        {
            m_CurrentLife = m_MaxLife;
            m_MaxCurrentLife = true;
        }
        else
        {
            m_MaxCurrentLife = false;
        }
    }

    public void ChangeInvincibilityStatus(bool l_CanTakeDamage) => m_CanTakeDamage = l_CanTakeDamage;
    public bool CanIHitIt() => m_CanTakeDamage;

    public void UpdateLifeParametersHUD()
    {
        // We update the fill first, it can be smoothed
        m_ImageCurrentLife.fillAmount = m_CurrentLife / m_MaxLife;

        if (m_CurrentLife > m_MaxLife / 2 + m_MaxLife / 4) // Has more than 75% life
        {
            m_ImageCurrentLife.color = m_BaseColorLife;
            m_ImageMaxLife.color = m_BaseColorMaxLife;
        }
        else if (m_CurrentLife > m_MaxLife / 2) // Has more than 50% life
        {
            m_ImageCurrentLife.color = m_FirstColorLife;
            m_ImageMaxLife.color = m_FirstColorMaxLife;
        } 
        else if (m_CurrentLife > m_MaxLife / 4) // Has more than 25% life
        {
            m_ImageCurrentLife.color = m_SecondColorLife;
            m_ImageMaxLife.color = m_SecondColorMaxLife;
        }
        else // The life that still remains
        {
            m_ImageCurrentLife.color = m_ThirdColorLife;
            m_ImageMaxLife.color = m_ThirdColorMaxLife;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            SetDamage(1);

        if (Input.GetKeyDown(KeyCode.H))
            SetHealing(1);

        // Timer to control how long the life interface is present
        if (!m_OnStartCounter) return;

        m_TimerHUD += Time.deltaTime;

        if (!(m_TimerHUD >= m_TimeToDisappear)) return;

        m_TimerHUD = 0;
        m_LifePower.Play(m_HidePower.name);
        m_HUDIsVisible = false;
        m_OnStartCounter = false;
    }

    // Accessible method to add cure
    public void SetHealing(float l_CuresCaused)
    {
        if (m_MaxCurrentLife) return;

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
        UpdateLifeParametersHUD();
        m_OnStartCounter = true;
    }

    void Heal(float l_CuresCaused)
    {
        if (m_CurrentLife + l_CuresCaused > m_MaxLife)
            m_CurrentLife = m_MaxLife;
        else
        {
            m_IReceivedCures?.Invoke();
            m_CurrentLife += l_CuresCaused;
        }

        m_MaxCurrentLife = m_CurrentLife >= m_MaxLife;
    }

    // Accessible method to add damage
    public void SetDamage(float l_DamageCaused)
    {
        if (!m_CanTakeDamage) return;
        
        m_CanTakeDamage = false;
        m_MaxCurrentLife = false;

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
        Damage(l_DamageCaused);
        UpdateLifeParametersHUD();
        m_CanTakeDamage = true;
        m_OnStartCounter = true;
    }

    void Damage(float l_DamageCaused)
    {
        if (m_CurrentLife - l_DamageCaused > m_MinLife)
            Hitted(l_DamageCaused);
        else
            Die();
    }

    void Hitted(float l_DamageCaused)
    {
        m_CurrentLife -= l_DamageCaused;
        m_TakingDamegeEvent?.Invoke();
    }

    void Die()
    {
        m_CurrentLife = m_MinLife;
        m_IHaveDiedEvent?.Invoke();
    }
}