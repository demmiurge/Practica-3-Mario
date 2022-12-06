using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinStarManager : MonoBehaviour
{
    private int m_Coins;
    private int m_Stars;

    [Header("UI")]
    public TextMeshProUGUI m_TextCoins;
    public TextMeshProUGUI m_TextStars;

    // Start is called before the first frame update
    void Start()
    {
        m_Coins = 0;
        m_Stars = 0;
        UpdateCoins();
        UpdateStars();
    }

    public int GetCoins() => m_Coins;
    public int GetStars() => m_Coins;

    public void SetCoins(int l_NewCurrentCoins)
    {
        m_Coins = l_NewCurrentCoins;
        UpdateCoins();
    }

    public void SetStars(int m_NewCurrentStars)
    {
        m_Stars = m_NewCurrentStars;
        UpdateStars();
    }

    public void AddCoinds(int l_Coins)
    {
        m_Coins += l_Coins;
        UpdateCoins();
    }

    public void AddStars(int l_Stars)
    {
        m_Stars += l_Stars;
        UpdateStars();
    }

    void UpdateCoins()
    {
        m_TextCoins.text = m_Coins.ToString();
    }

    void UpdateStars()
    {
        m_TextStars.text = m_Stars.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
