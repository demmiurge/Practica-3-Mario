using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedObjectsArea : MonoBehaviour
{
    public ManagerStatusObjectsOnTime m_ManagerStatusObjectsOnTime;
    public List<GameObject> m_PoolObjectsArea;
    private bool m_WasAlreadyHere = false;

    public Material m_FlagBowser;
    public Material m_FlagMario;
    public SkinnedMeshRenderer m_Flag;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Material GetCurrentFlag()
    {
        return m_Flag.material;
    }

    public void SetNewFlag(Material l_NewFlag)
    {
        m_Flag.material = l_NewFlag;
    }

    public bool GetChangeStateFlag() => m_WasAlreadyHere;
    public void SetChangeStateFlag(bool l_StateFlag) => m_WasAlreadyHere = l_StateFlag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !m_WasAlreadyHere)
        {
            m_WasAlreadyHere = true;
            // m_ManagerStatusObjectsOnTime.AddNewObjectsIntoPool(m_PoolObjectsArea);
            m_Flag.material = m_FlagMario;
            m_ManagerStatusObjectsOnTime.SaveNewPointGameObject();
        }
    }
}
