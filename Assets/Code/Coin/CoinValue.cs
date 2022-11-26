using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinValue : MonoBehaviour
{
    int m_CoinValue;

    [Header("Parameters")] 
    public GameObject m_CoinMaterial;
    public TypeCoin m_TypeCoin;

    [Header("Value coins")]
    public List<int> m_ValueCoins;
    public List<Material> m_MaterialsCoins;

    // Start is called before the first frame update
    void Start()
    {
        m_CoinValue = m_ValueCoins[(int)m_TypeCoin];
        m_CoinMaterial.GetComponent<Renderer>().material = m_MaterialsCoins[(int)m_TypeCoin];
    }

    public int GetCoinValue() => m_CoinValue;
}
