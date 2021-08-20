using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{
    [Header("Asteroid Spawn")]
    [SerializeField]public GameObject asteroidPrefab_;
    public int numAsteroids_ = 500;

    [Header("MissleSpawn")]
    [SerializeField] public GameObject missleSiloPrefab_;
    public int numberOfSilos_ = 500;

    // Start is called before the first frame update
    public static EntityManager manager_;
    BlobAssetStore store_;

    void Start()
    {
        store_ = new BlobAssetStore();
        manager_ = World.DefaultGameObjectInjectionWorld.EntityManager;
        var _settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store_);
        Entity _asteroid = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefab_, _settings);
        Entity _missleSilo = GameObjectConversionUtility.ConvertGameObjectHierarchy(missleSiloPrefab_, _settings);

        for (int i = 0; i < numAsteroids_; i++)
        {
            var instance = manager_.Instantiate(_asteroid);

            float x = UnityEngine.Random.Range(-500, 500);
            float y = UnityEngine.Random.Range(-500, 500);
            float z = UnityEngine.Random.Range(-500, 500);

            float3 position = new float3(x, y, z);
            manager_.SetComponentData(instance, new Translation { Value = position });

            float rspeed = UnityEngine.Random.Range(1, 2) / 10.0f;
            manager_.SetComponentData(instance, new FloatData { speed = rspeed });
        }

        for (int i = 0; i < numberOfSilos_; i++)
        {
            var instance = manager_.Instantiate(_missleSilo);

            float x = UnityEngine.Random.Range(-500, 500);
            float y = UnityEngine.Random.Range(-500, 500);
            float z = UnityEngine.Random.Range(-500, 500);

            float3 position = new float3(x, y, z);
            manager_.SetComponentData(instance, new Translation { Value = position });//this spawns prefab

            float rspeed = UnityEngine.Random.Range(1, 2) / 10.0f;
            manager_.SetComponentData(instance, new FloatData { speed = rspeed });//this runs a new FloatData on the instance.
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        store_.Dispose();
    }
}
