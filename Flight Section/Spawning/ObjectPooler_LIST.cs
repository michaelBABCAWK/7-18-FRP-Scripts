using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



class classToAdd : MonoBehaviour
{

}


public class ObjectPooler_LIST : MonoBehaviour
{
    //Make it so typing in the name of a component will add it to the gameobjects


    //variables in Class Pool will be refered to by pool in the foreach(Pool pool in pools)
    [System.Serializable]
    public class Pool//add attachment throug bool in this section
    {
        //tag is required to know what prefabs the dictionary will be capable of using.
        public string tag;
        public GameObject prefab;
        //[SerializeField]public string componentsToAdd_;//if not null add component to this object
        //public Type effectType = typeof();

        //controls size of pool before reusing objects.
        public int size;
    }

    public string[] tagsInPooler;

    public static ObjectPooler_LIST Instance;

    

    #region singelton instance = this;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    //List  is similar to array. Only difference is a List goes in order from 0-10. While an Array can 
    //go from 0-10, while also giving the option to jump to a direct value.
    //Basically this line beneath will create a List containing the Class Pool
    //each List of Pool will be called pools.
    //is pools = to the variables within Pool? == No it is the name of the List holding each Pool.
    public List<Pool> pools;

    //string = the tag which will be looked for on the object in Queue. Queue<GameObject> is the object that will be placed in the dictionary?
    //that is waiting to be spawned. this dictionary will be called poolDictionary.
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    //Dictionary will not work unless asked to take in a Key, and Value.
    //So the above line is showing a dictionary is similar to any other variable. It just requires input (Key, value)
    //definiitions.
    //public Dictionary hello;

    void Start()
    {

        //poolDictionary now equals a dictionary wating to take in Keys, and values. Called with Pool class ** see top.
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //pool is a variable assigned to equal an instance of Pool in the list pools. pools being the List of objects in Pool.
        //for each. Going through every instance of Pool being created. Then assigns pool as the name of each instance in the pools.
        //pools being the names given to every List holding <Pool>.

        //for each Pool, the information/variables inside will be called pool. This will be true for everything in pools.
        //lets us reference all variables within Pool class.
        foreach(Pool pool in pools)

        {
            //objectPool is the names of the new queue containing GameObjects.
            Queue<GameObject> objectPool = new Queue<GameObject>();

            //the values assigned to size in Class Pool will determine how many inactive gameobjects are going to be 
            //queued and stored in the dictionary:     public Dictionary<string, Queue<GameObject>> poolDictionary;.
            for (int i = 0; i < pool.size; i++)

            {


                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);

                //this line below places the nonactive game object to the end of the queue which will be placed in the
                //Value section of:     public Dictionary<string, Queue<GameObject>> poolDictionary;.
                objectPool.Enqueue(obj);



            }

            //poolDictionary is adding a Key (pool.tag) and assigning ita value (objectPool) which is a queue of Gameobjects.
            //All this is established at the top:     public Dictionary<string, Queue<GameObject>> poolDictionary;
            poolDictionary.Add(pool.tag, objectPool);


        }
    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation)
    {
       

        
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag" + tag + "Doesn't exist");
            return null;
        }
        //string componentToAdd = "hello";


        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        //line is returning an error
        objectToSpawn.SetActive(true);

        objectToSpawn.transform.position = position;

        objectToSpawn.transform.rotation = rotation;


        //objectToSpawn.AddComponent<classToAdd>();//class needs to be inputfac

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        poolDictionary[tag].Enqueue(objectToSpawn);

        if(pooledObj != null)
        {
            pooledObj.OnObjectsSpawn();
        }

        return objectToSpawn;
    }

}
