using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaShell : MonoBehaviour
{
    private bool isAttached = false;
    Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAttached(bool Attached)
    {
        isAttached = Attached;
    }

}
