using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class CheckFloorMaterial : MonoBehaviour
{
    //public MeshRenderer m_AllMaterials;
    //public List<Material> m_AllMaterials;
    public Transform m_Feet;
    public float m_DistanceCheckTolerance = 1f;
    public List<Material> m_SlipperyMaterials;
    public LayerMask m_FloorLayer;
    public float m_FallDotAngle = 45.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray l_Ray = new Ray(m_Feet.position, Vector3.down);
        RaycastHit l_RaycastHit;

        Debug.DrawRay(l_Ray.origin, l_Ray.direction * m_DistanceCheckTolerance, Color.red);

        if (Physics.Raycast(l_Ray, out l_RaycastHit, m_DistanceCheckTolerance, m_FloorLayer.value))
        {
            if (Vector3.Angle(l_RaycastHit.normal, Vector3.up) > m_FallDotAngle)
                Debug.Log("Resbalo " + Vector3.Angle(l_RaycastHit.normal, Vector3.up));
            else
                Debug.Log("No resbalo " + Vector3.Angle(l_RaycastHit.normal, Vector3.up));

            //if (Vector3.Dot(l_RaycastHit.normal, Vector3.up) < m_FallDotAngle)
            //       Debug.Log("Resbalo");
            //   else
            //       Debug.Log("No resbalo");

            foreach (Material l_Material in m_SlipperyMaterials)
            {
                
                //Debug.Log("QUE ES "+);
                //Debug.Log("Collider material " + l_RaycastHit.collider.GetComponent<Renderer>().material.name);
                //if (l_Material.name == l_RaycastHit.collider.GetComponent<Renderer>().material.name)
                //    Debug.Log("Resbalo");
                //else
                //    Debug.Log("No resbalo");
            }
            //Debug.Log("Collider material " + l_RaycastHit.collider.GetComponent<Renderer>().material);
        }
        // do something
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }
}
