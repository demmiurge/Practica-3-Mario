using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, IRestartGame
{
    static GameController m_GameController = null;
    PlayerMovementWithRigidbody m_Player;
    List<IRestartGame> m_RestartGameElements = new List<IRestartGame>();

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public static GameController GetGameController()
    {
        if (m_GameController == null)
        {
            m_GameController = new GameObject("GameController").AddComponent<GameController>();
            //GameControllerData l_GameControllerData = Resources.Load<GameControllerData>("GameControllerData");
        }
        return m_GameController;
    }

    public PlayerMovementWithRigidbody GetPlayer()
    {
        return m_Player;
    }

    public void SetPlayer(PlayerMovementWithRigidbody Player)
    {
        m_Player = Player;
    }

    public static void DestroySingleton()
    {
        if (m_GameController != null)
            GameObject.Destroy(m_GameController.gameObject);
        m_GameController = null;
    }

    public void AddRestartGameElements(IRestartGame Element)
    {
        m_RestartGameElements.Add(Element);
    }

    public void RestartGame()
    {
        foreach (IRestartGame l_RestartElement in m_RestartGameElements)
        {
            l_RestartElement.RestartGame();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }
}
