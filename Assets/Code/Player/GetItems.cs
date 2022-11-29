using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItems : MonoBehaviour
{
    public CoinStarManager m_CoinStarManager;
    private MarioLive m_MarioLive;

    void Awake()
    {
        m_MarioLive = GetComponent<MarioLive>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin") 
        {
            m_CoinStarManager.AddCoinds(other.GetComponent<CoinValue>().GetCoinValue());
            m_MarioLive.SetHealing(other.GetComponent<CoinValue>().GetCoinLife());
            other.gameObject.SetActive(false);
        }

        if (other.tag == "Star")
        {
            m_CoinStarManager.AddStars(1);
            other.gameObject.SetActive(false);
        }
    }
}
