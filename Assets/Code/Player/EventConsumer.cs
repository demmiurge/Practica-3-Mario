using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConsumer : MonoBehaviour
{
    public ParticleSystem m_Step;
    public ParticleSystem m_Land;
    public ParticleSystem m_Hit;
    public ParticleSystem m_Punch;
    public void Step(AnimationEvent _AnimationEvent)
    {
        Object l_Object = _AnimationEvent.objectReferenceParameter;
        m_Step.Play();
    }

    public void Land(AnimationEvent _AnimationEvent)
    {
        m_Land.Play();
    }

    public void HitFront(AnimationEvent _AnimationEvent)
    {
        m_Hit.Play();
    }

    public void PunchSound1(AnimationEvent _AnimationEvent)
    {
        m_Punch.Play();

       // Debug.Log("event sound" + l_StringParameter + " + " + l_FloatParameter + " + " + l_IntParameter + " + " + l_object);
    }

    public void PunchSound2(AnimationEvent _AnimationEvent)
    {
        m_Punch.Play();
 
    }

    public void PunchSound3(AnimationEvent _AnimationEvent)
    {
        m_Punch.Play();
    }

    /*public void FinishPunch(AnimationEvent _AnimationEvent)
    {
        m_Punch.Play();
    }*/
}
