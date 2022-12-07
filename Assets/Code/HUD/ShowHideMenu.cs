using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideMenu : MonoBehaviour
{
    public KeyCode m_PauseMenuKeyCode = KeyCode.P;
    public GameObject m_PauseMenuCanvas;
    public ShowHideMouse m_ShowHideMouse;
    public TimeGame m_TimeGame;


    // Start is called before the first frame update
    void Start()
    {
        m_PauseMenuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
            HideShowCanvas(!m_PauseMenuCanvas.activeSelf);
    }

    public void HideShowCanvas(bool l_Status)
    {
        m_PauseMenuCanvas.SetActive(l_Status);
        if (l_Status)
        {
            m_ShowHideMouse.ShowMouse();
            m_TimeGame.PauseGame();
        }
        else
        {
            m_ShowHideMouse.HideMouse();
            m_TimeGame.ResumeGame();
        }
    }
}
