using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConsumer : MonoBehaviour
{
    public ParticleSystem m_Step;
    public ParticleSystem m_Hit;
    public void Step(AnimationEvent _AnimationEvent)
    {
        Object l_Object = _AnimationEvent.objectReferenceParameter;
        m_Step.Play();
    }

    public void LandParticles(AnimationEvent _AnimationEvent)
    {
        Object l_Object = _AnimationEvent.objectReferenceParameter;
        m_Step.Play();
        Debug.Log("land");
    }

    public void HitFront(AnimationEvent _AnimationEvent)
    {
        Object l_Object = _AnimationEvent.objectReferenceParameter;
        m_Hit.Play();
    }

    public void PunchSound1(AnimationEvent _AnimationEvent)
    {
        string l_StringParameter = _AnimationEvent.stringParameter;
        float l_FloatParameter = _AnimationEvent.floatParameter;
        int l_IntParameter = _AnimationEvent.intParameter;
        Object l_object = _AnimationEvent.objectReferenceParameter;

       // Debug.Log("event sound" + l_StringParameter + " + " + l_FloatParameter + " + " + l_IntParameter + " + " + l_object);
    }
}
