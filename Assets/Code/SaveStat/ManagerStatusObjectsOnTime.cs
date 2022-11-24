using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerStatusObjectsOnTime : MonoBehaviour
{
    public List<GameObject> m_PoolObjectsToRestart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Recargar");
            Respawn();
        }
    }

    public void AddNewObjectsIntoPool(List<GameObject> l_NewGameObjects)
    {
        foreach (GameObject l_NewGameObject in l_NewGameObjects)
        {
            foreach (GameObjectStateLoadReload l_ComponentToRestart in l_NewGameObject.GetComponents<GameObjectStateLoadReload>())
                l_ComponentToRestart.m_AvailableToSetAttributes = true;

            m_PoolObjectsToRestart.Add(l_NewGameObject);
        }
    }

    public void SaveNewPointGameObject()
    {
        foreach (GameObject l_GameObject in m_PoolObjectsToRestart)
            foreach (GameObjectStateLoadReload l_ComponentToRestart in l_GameObject.GetComponents<GameObjectStateLoadReload>())
                if (l_ComponentToRestart.CanAttributesBeSet())
                    l_ComponentToRestart.SetCurrentAttributesAsDefault();
    }

    public void Respawn()
    {
        foreach (GameObject l_GameObject in m_PoolObjectsToRestart)
            foreach (GameObjectStateLoadReload l_ComponentToRestart in l_GameObject.GetComponents<GameObjectStateLoadReload>())
                l_ComponentToRestart.LoadDefaultAttributes();
    }
}
