using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HBPool 
{
    class Pool
    {
        //parent contain all pool member
        Transform m_sRoot = null;
        //object is can collect back to pool
        bool m_collect;
        //list object in pool
        Queue<GameUnit> m_inactive;
        //collect obj active ingame
        List<GameUnit> m_active;
        //The prefab that we are pooling
        GameUnit m_prefab;

        public List<GameUnit> Active => m_active;
        public int Count => m_inactive.Count + m_active.Count;
        public Transform Root => m_sRoot;

        // Constructor
        public Pool(GameUnit prefab, int initialQty, Transform parent)
        {
            m_sRoot = parent;
            this.m_prefab = prefab;
            m_inactive = new Queue<GameUnit>(initialQty);
            m_active = new List<GameUnit>();
        }

        public Pool(GameUnit prefab, int initialQty, Transform parent, bool collect)
        {
            m_inactive = new Queue<GameUnit>(initialQty);
            m_sRoot = parent;
            this.m_prefab = prefab;
            m_collect = collect;
            if (m_collect) m_active = new List<GameUnit>();
        }

        // Spawn an object from our pool with position and rotation
        public GameUnit Spawn(Vector3 pos, Quaternion rot)
        {
            GameUnit obj;

            if (m_inactive.Count <= 0)
            {
                obj = (GameUnit)GameObject.Instantiate(m_prefab, m_sRoot);
            }
            else
            {
                // Grab the last object in the inactive array
                obj = m_inactive.Dequeue();
            }

            m_active.Add(obj);

            obj.TF.SetPositionAndRotation(pos, rot);
            obj.gameObject.SetActive(true);

            return obj;
        }

        //// Spawn an object from our pool with position and rotation
        //public GameUnit Spawn(Vector3 pos, Quaternion rot)
        //{
        //    GameUnit obj = Spawn();

        //    obj.TF.SetPositionAndRotation(pos, rot);

        //    return obj;
        //}

        //spawn gameunit
        public GameUnit Spawn()
        {
            GameUnit obj;
            if (m_inactive.Count == 0)
            {
                obj = (GameUnit)GameObject.Instantiate(m_prefab, m_sRoot);
            }
            else
            {
                // Grab the last object in the inactive array
                obj = m_inactive.Dequeue();

                if (obj == null)
                {
                    return Spawn();
                }
            }

            if (m_collect) m_active.Add(obj);

            obj.gameObject.SetActive(true);

            return obj;
        }


        // Return an object to the inactive pool.
        public void Despawn(GameUnit obj)
        {
            if (obj != null && obj.gameObject.activeSelf)
            {
                obj.gameObject.SetActive(false);
                m_inactive.Enqueue(obj);
            }

            m_active.Remove(obj);
        }

        //collect all unit comeback to pool
        public void Collect()
        {
            while (m_active.Count > 0)
            {
                Despawn(m_active[0]);
            }
        }
    }
    //dict for faster search from pool type to prefab
    static Dictionary<PoolType, GameUnit> poolTypes = new Dictionary<PoolType, GameUnit>();

    static Dictionary<PoolType, Pool> poolInstance = new Dictionary<PoolType, Pool>();
    

    public static void Preload(GameUnit prefab, int amount, Transform parent = null)
    {
        if (!poolTypes.ContainsKey(prefab.poolType))
        {
            poolTypes.Add(prefab.poolType, prefab);
        }

        if (prefab != null && !poolInstance.ContainsKey(prefab.poolType))
        {
            poolInstance.Add(prefab.poolType, new Pool(prefab, amount, parent));
        }
    }

    static public void Preload(GameUnit prefab, int qty = 1, Transform parent = null, bool collect = false)
    {
        if (!poolTypes.ContainsKey(prefab.poolType))
        {
            poolTypes.Add(prefab.poolType, prefab);
        }

        if (prefab == null)
        {
            Debug.LogError(parent.name + " : IS EMPTY!!!");
            return;
        }

        InitPool(prefab, qty, parent, collect);

        // Make an array to grab the objects we're about to pre-spawn.
        GameUnit[] obs = new GameUnit[qty];
        for (int i = 0; i < qty; i++)
        {
            obs[i] = Spawn(prefab);
        }

        // Now despawn them all.
        for (int i = 0; i < qty; i++)
        {
            Despawn(obs[i]);
        }
    }
    public const int DEFAULT_POOL_SIZE = 20;
    //save member that is child transform other object
    static HashSet<int> memberInParent = new HashSet<int>();

    private static Transform root;

    public static Transform Root
    {
        get
        {
            if (root == null)
            {
                root = GameObject.FindObjectOfType<PoolControler>().transform;

                if (root == null)
                {
                    root = new GameObject("Pool").transform;
                }
            }

            return root;
        }
    }

    static void InitPool(GameUnit prefab = null, int qty = DEFAULT_POOL_SIZE, Transform parent = null, bool collect = false)
    {
        if (prefab != null && !IsHasPool(prefab))
        {
            poolInstance.Add(prefab.poolType, new Pool(prefab, qty, parent, collect));
        }
    }

    public static GameUnit GetPrefabByType(PoolType poolType)
    {
        if (!poolTypes.ContainsKey(poolType) || poolTypes[poolType] == null)
        {
            GameUnit[] resources = Resources.LoadAll<GameUnit>("Pool");

            for (int i = 0; i < resources.Length; i++)
            {
                poolTypes[resources[i].poolType] = resources[i];
            }
        }

        return poolTypes[poolType];
    }

    static public T Spawn<T>(GameUnit obj) where T : GameUnit
    {
        return Spawn(obj) as T;
    }

    public static T Spawn<T>(PoolType poolType, Vector3 localPoint, Quaternion localRot, Transform parent) where T : GameUnit
    {
        return Spawn<T>(GetPrefabByType(poolType), localPoint, localRot, parent);
    }

    public static T Spawn<T>(PoolType poolType, Vector3 pos, Quaternion rot) where T : GameUnit
    {
         return poolInstance[poolType].Spawn(pos, rot) as T;
    }
    
    public static T Spawn<T>(PoolType poolType) where T : GameUnit
    {
         return poolInstance[poolType].Spawn(Vector3.zero, Quaternion.identity) as T;
    }

    static public T Spawn<T>(GameUnit obj, Vector3 localPoint, Quaternion localRot, Transform parent) where T : GameUnit
    {
        T unit = Spawn<T>(obj);
        unit.TF.SetParent(parent);
        unit.TF.localPosition = localPoint;
        unit.TF.localRotation = localRot;
        unit.TF.localScale = Vector3.one;
        memberInParent.Add(unit.GetInstanceID());
        return unit;
    }

    static public GameUnit Spawn(GameUnit obj)
    {
        if (!IsHasPool(obj))
        {
            Transform newRoot = new GameObject(obj.name).transform;
            newRoot.SetParent(Root);
            Preload(obj, 1, newRoot, true);
        }

        return GetPool(obj).Spawn();
    }

    public static void Despawn(GameUnit gameUnit)
    {
        poolInstance[gameUnit.poolType].Despawn(gameUnit);
    }

    public static void CollectAll()
    {
        foreach (var pool in poolInstance)
        {
            pool.Value.Collect();
        }
    }

    static private bool IsHasPool(GameUnit obj)
    {
        return poolInstance.ContainsKey(obj.poolType);
    }

    static private Pool GetPool(GameUnit obj)
    {
        return poolInstance[obj.poolType];
    }

    //public static GameUnit GetPrefabByType(PoolType poolType)
    //{
    //    if (!poolTypes.ContainsKey(poolType) || poolTypes[poolType] == null)
    //    {
    //        GameUnit[] resources = Resources.LoadAll<GameUnit>("Pool");

    //        for (int i = 0; i < resources.Length; i++)
    //        {
    //            poolTypes[resources[i].poolType] = resources[i];
    //        }
    //    }

    //    return poolTypes[poolType];
    //}

    #region Get List object ACTIVE
    // get all member is active in game
    public static List<GameUnit> GetAllUnitIsActive(GameUnit obj)
    {
        return IsHasPool(obj) ? GetPool(obj).Active : new List<GameUnit>();
    }
    public static List<GameUnit> GetAllUnitIsActive(PoolType poolType)
    {
        return GetAllUnitIsActive(GetPrefabByType(poolType));
    }

    #endregion
}
