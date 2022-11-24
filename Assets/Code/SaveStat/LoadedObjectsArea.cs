using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedObjectsArea : MonoBehaviour
{
    public ManagerStatusObjectsOnTime m_ManagerStatusObjectsOnTime;
    public List<GameObject> m_PoolObjectsArea;
    private bool m_WasAlreadyHere = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !m_WasAlreadyHere)
        {
            m_WasAlreadyHere = true;
            m_ManagerStatusObjectsOnTime.AddNewObjectsIntoPool(m_PoolObjectsArea);
            m_ManagerStatusObjectsOnTime.SaveNewPointGameObject();
        }
    }
}
