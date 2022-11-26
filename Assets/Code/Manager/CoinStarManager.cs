using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinStarManager : MonoBehaviour
{
    private int m_Coins;
    private int m_Stars;

    [Header("UI")]
    public GameObject m_TextCoins;
    public GameObject m_TextStars;

    // Start is called before the first frame update
    void Start()
    {
        m_Coins = 0;
        m_Stars = 0;
        UpdateCoins();
        UpdateStars();
    }

    public void AddCoinds(int l_Coins)
    {
        m_Coins += l_Coins;
        Debug.Log("HE1");
        UpdateCoins();
    }

    public void AddStars(int l_Stars)
    {
        m_Stars += l_Stars;
        UpdateStars();
    }

    void UpdateCoins()
    {
        Debug.Log("HE1");
        m_TextCoins.GetComponent<TextMeshProUGUI>().text = m_Coins.ToString();
        Debug.Log("HE2");
    }

    void UpdateStars()
    {
        m_TextStars.GetComponent<TextMeshProUGUI>().text = m_Stars.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
