using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerStatusObjectsOnTime : MonoBehaviour
{
    public List<GameObject> m_PoolObjectsToRestart;

    // Start is called before the first frame update
    void Start()
    {
        ForceSetChangeAttribute();
        // RemoveObjects(); // GUILLEM THINGS
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    void ForceSetChangeAttribute()
    {
        foreach (GameObject l_GameObject in m_PoolObjectsToRestart)
            foreach (GameObjectStateLoadReload l_ComponentToRestart in l_GameObject.GetComponents<GameObjectStateLoadReload>())
                l_ComponentToRestart.m_AvailableToSetAttributes = true;
    }

    void RemoveObjects() // DO NOT LOOK
    {
        for (var index = 0; index < m_PoolObjectsToRestart.Count; index++)
        {
            var l_GameObject = m_PoolObjectsToRestart[index];
            var reloads = l_GameObject.GetComponents<GameObjectStateLoadReload>();
            for (var i = 0; i < reloads.Length; i++)
            {
                var l_ComponentToRestart = reloads[i];
                Destroy(l_ComponentToRestart.gameObject);
            }
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

    public void RestartLevel()
    {
        foreach (GameObject l_GameObject in m_PoolObjectsToRestart)
            foreach (GameObjectStateLoadReload l_ComponentToRestart in l_GameObject.GetComponents<GameObjectStateLoadReload>())
                l_ComponentToRestart.ResetDefaultAttributes();
    }
}
