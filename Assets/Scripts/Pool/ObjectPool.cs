using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;

    public List<GameObject> pooledObjects;

    [Serializable]
    public struct ObjectInfo
    {       
        public ObjectType type;
        public GameObject prefab;
        public int startCount;
    }

    [SerializeField] private List<ObjectInfo> objectsInfo;

    private Dictionary<ObjectType, Pool> pools;

    private void Awake()
    {
        if (SharedInstance == null)
            SharedInstance = this;

        InitPool();
    }

    private void InitPool()
    {
        pools = new Dictionary<ObjectType, Pool>();

        var emptyGO = new GameObject();

        foreach (var item in objectsInfo)
        {
            var container = Instantiate(emptyGO, transform, false);
            container.name = item.type.ToString();

            pools[item.type] = new Pool(container.transform);

            for (int i = 0; i < item.startCount; i++)
            {
                var go = InstantiateObject(item.type, container.transform);
                pools[item.type].Objects.Enqueue(go);
            }
        }

        Destroy(emptyGO);
    }

    private GameObject InstantiateObject(ObjectType type, Transform parent)
    {
        var go = Instantiate(objectsInfo.Find(x => x.type == type).prefab, parent);
        go.SetActive(false);
        return go;
    }

    public GameObject GetObject(ObjectType type)
    {
        var obj = pools[type].Objects.Count > 0 ?
            pools[type].Objects.Dequeue() : InstantiateObject(type, pools[type].Container);

        obj.SetActive(true);

        return obj;
    }

    public void DestroyObject(GameObject obj)
    {
        ObjectType type = obj.GetComponent<IPooledObject>().Type;
        pools[type].Objects.Enqueue(obj);

        obj.transform.SetParent(pools[type].Container);
        obj.SetActive(false);
    }
}
