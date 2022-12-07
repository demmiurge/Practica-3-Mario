using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventPlayerSounds : MonoBehaviour
{
    public AudioSource m_Audio;
    public AudioClip m_FootStep;
    public AudioClip m_PunchSound1;
    public AudioClip m_PunchSound2;
    public AudioClip m_PunchSound3;
    public AudioClip m_FinishPunch;
    public AudioClip m_Jump1;
    public AudioClip m_Jump2;
    public AudioClip m_Jump3; 
    public AudioClip m_LongJumpSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Step()
    {
        m_Audio.PlayOneShot(m_FootStep);
    }

    public void PunchSound1()
    {
        m_Audio.PlayOneShot(m_PunchSound1);
    }

    public void PunchSound2()
    {
        m_Audio.PlayOneShot(m_PunchSound2);
    }

    public void PunchSound3()
    {
        m_Audio.PlayOneShot(m_PunchSound3);
    }

    public void FinishPunch()
    {
        m_Audio.PlayOneShot(m_FinishPunch);
    }

    public void Jump1()
    {
        m_Audio.PlayOneShot(m_Jump1);
    }

    public void Jump2()
    {
        m_Audio.PlayOneShot(m_Jump2);
    }

    public void Jump3()
    {
        m_Audio.PlayOneShot(m_Jump3);
    }

    public void LongJumpSound()
    {
        m_Audio.PlayOneShot(m_LongJumpSound);
    }
    
}
