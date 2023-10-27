using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SnailordObjectPool : MonoBehaviour
{
    public static SnailordObjectPool instance;

    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 10;

    [SerializeField] private GameObject projectilePrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void InstantiateProjectilePrefabs()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            //obj.GetComponent<NetworkObject>().Spawn();
            obj.SetActive(false);
            obj.transform.parent = this.transform;
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }    
        }
        return null;
    }
}
