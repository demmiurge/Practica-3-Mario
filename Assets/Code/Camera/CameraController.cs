using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_LookAt;
    //float m_Yaw = 0.0f;
    float m_Pitch = 0.0f;
    //public float m_Distance = 15.0f;
    public float m_YawRotationalSpeed = 720.0f;
    public float m_PitchRotationalSpeed = 360.0f;

    public float m_MinPitch = 30.0f;
    public float m_MaxPitch = 60.0f;

    public float m_MinDistance = 15f;
    public float m_MaxDistance = 15f;

    public LayerMask m_AvoidObjectsLayerMask;
    public float m_Offset = 0.1f;

    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;

    bool m_AngleLocked = false;
    bool m_AimLocked = true;

    [HideInInspector]
    public bool m_HasToReset = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
    }

#if UNITY_EDITOR
    void UpdateInputDebug()
    {
        if (Input.GetKeyDown(m_DebugLockAngleKeyCode))
            m_AngleLocked = !m_AngleLocked;
        if (Input.GetKeyDown(m_DebugLockKeyCode))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        }
    }
#endif

    void LateUpdate()
    {
#if UNITY_EDITOR
        UpdateInputDebug();
#endif

        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");

#if UNITY_EDITOR
        if (m_AngleLocked)
        {
            l_MouseX = 0.0f;
            l_MouseY = 0.0f;
        }
#endif
        transform.LookAt(m_LookAt.position);
        float l_Distance = Vector3.Distance(transform.position, m_LookAt.position);
        Vector3 l_EulerAngles = transform.rotation.eulerAngles;
        float l_Yaw = l_EulerAngles.y;

        l_Yaw += l_MouseX * m_YawRotationalSpeed * Time.deltaTime;
        m_Pitch += l_MouseY * m_PitchRotationalSpeed * Time.deltaTime;
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        Vector3 l_ForwardCamera = new Vector3(Mathf.Sin(l_Yaw * Mathf.Deg2Rad) * Mathf.Cos(m_Pitch * Mathf.Deg2Rad), Mathf.Sin(m_Pitch * Mathf.Deg2Rad), Mathf.Cos(l_Yaw * Mathf.Deg2Rad) * Mathf.Cos(m_Pitch * Mathf.Deg2Rad));
        l_Distance = Mathf.Clamp(l_Distance, m_MinDistance, m_MaxDistance);
        Vector3 l_DesiredPosition = m_LookAt.transform.position - l_ForwardCamera * l_Distance;

        Ray l_Ray = new Ray(m_LookAt.position, -l_ForwardCamera);
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(l_Ray, out l_RaycastHit, l_Distance, m_AvoidObjectsLayerMask.value))
        {
            l_DesiredPosition = l_RaycastHit.point + l_ForwardCamera * m_Offset;
        }
        transform.position = l_DesiredPosition;
        transform.LookAt(m_LookAt.position);

        if(m_HasToReset)
        {
            //if(Vector3.Dot(this.transform.forward, m_LookAt.transform.forward) < Mathf.Cos(15 * Mathf.Deg2Rad))
            Debug.Log("reset");
            l_ForwardCamera = new Vector3(Mathf.Sin(l_Yaw * Mathf.Deg2Rad) * Mathf.Cos(m_LookAt.transform.rotation.x * Mathf.Deg2Rad), Mathf.Sin(m_LookAt.transform.rotation.x * Mathf.Deg2Rad), Mathf.Cos(l_Yaw * Mathf.Deg2Rad) * Mathf.Cos(m_LookAt.transform.rotation.x * Mathf.Deg2Rad));
            m_HasToReset = false;
        }
        //(0, -90, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
