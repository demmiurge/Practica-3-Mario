using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConsumer : MonoBehaviour
{
    public void Step(AnimationEvent _AnimationEvent)
    {
        Object l_Object = _AnimationEvent.objectReferenceParameter;
       
    }

    public void PunchSound1(AnimationEvent _AnimationEvent)
    {
        string l_StringParameter = _AnimationEvent.stringParameter;
        float l_FloatParameter = _AnimationEvent.floatParameter;
        int l_IntParameter = _AnimationEvent.intParameter;
        Object l_object = _AnimationEvent.objectReferenceParameter;

        Debug.Log("event sound" + l_StringParameter + " + " + l_FloatParameter + " + " + l_IntParameter + " + " + l_object);
    }
}
