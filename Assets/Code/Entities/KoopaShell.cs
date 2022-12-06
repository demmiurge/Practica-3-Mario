using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaShell : MonoBehaviour
{
    private bool isAttached = false;
    Rigidbody m_Rigidbody;
    public bool m_hasMovement;
    float m_Rotation = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Rigidbody.velocity.magnitude > 0.3f && m_hasMovement == true)
        {
            m_Rotation += 0.05f;
            transform.Rotate(0, m_Rotation, 0 * Time.deltaTime);
        }
        if (m_Rigidbody.velocity.magnitude < 0.3f)
        {
            m_hasMovement = false;
            //Debug.Log("stop");
        }
        //Debug.Log(m_Rigidbody.velocity.magnitude);
    }

    public void SetAttached(bool Attached)
    {
        isAttached = Attached;
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Goomba")
        {
            collision.gameObject.GetComponent<GoombaEnemy>().Kill();
        }
        else if (collision.gameObject.tag == "Koopa")
        {
            collision.gameObject.GetComponent<KoopaEnemy>().Kill();
        }
    }
}
