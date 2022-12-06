using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CoinStarManager))]
public class StatusCoinsAndStarsAttribute : GameObjectStateLoadReload
{
    CoinStarManager m_CoinStarManager;

    int m_CurrentCoins;
    int m_CoinsInitial;

    int m_CurrentStars;
    int m_StarsInitial;

    // Start is called before the first frame update
    void Start()
    {
        m_CoinStarManager = GetComponent<CoinStarManager>();

        SetCurrentAttributesAsDefault();
        m_CoinsInitial = m_CoinStarManager.GetCoins();
        m_StarsInitial = m_CoinStarManager.GetStars();
    }

    public override void SetCurrentAttributesAsDefault()
    {
        m_CurrentCoins = m_CoinStarManager.GetCoins();
        m_CurrentStars = m_CoinStarManager.GetStars();
    }

    public override void LoadDefaultAttributes()
    {
        m_CoinStarManager.SetCoins(m_CurrentCoins);
        m_CoinStarManager.SetStars(m_CurrentStars);
    }

    public override void ResetDefaultAttributes()
    {
        m_CurrentCoins = m_CoinsInitial;
        m_CurrentStars = m_StarsInitial;

        m_CoinStarManager.SetCoins(m_CoinsInitial);
        m_CoinStarManager.SetStars(m_StarsInitial);
    }
}
