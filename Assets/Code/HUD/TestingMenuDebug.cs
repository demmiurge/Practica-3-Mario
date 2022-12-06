using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingMenuDebug : MonoBehaviour
{
    public KeyCode m_KeyResume = KeyCode.Keypad1;
    public UnityEvent m_Resume;

    public KeyCode m_KeyLoadFromLastCheckpoint = KeyCode.Keypad2;
    public UnityEvent m_LoadFromLastCheckpoint;

    public KeyCode m_KeyRestartLevel = KeyCode.Keypad3;
    public UnityEvent m_RestartLevel;

    public KeyCode m_KeyExitMenu = KeyCode.Keypad4;
    public UnityEvent m_ExitMenu;

    public KeyCode m_KeyExitGame = KeyCode.Keypad5;
    public UnityEvent m_ExitGame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(m_KeyResume))
        {
            m_Resume?.Invoke();
        }
        if (Input.GetKeyDown(m_KeyLoadFromLastCheckpoint))
        {
            m_LoadFromLastCheckpoint?.Invoke();
        }
        if (Input.GetKeyDown(m_KeyRestartLevel))
        {
            m_RestartLevel?.Invoke();
        }
        if (Input.GetKeyDown(m_KeyExitMenu))
        {
            m_ExitMenu?.Invoke();
        }
        if (Input.GetKeyDown(m_KeyExitGame))
        {
            m_ExitGame?.Invoke();
        }

    }
}
