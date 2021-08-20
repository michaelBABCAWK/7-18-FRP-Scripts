using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_AsteroidField : MonoBehaviour
{

    //NEEDS TO BE SET DEPENDING ON WHAT 
    [Header("Spawn Zones of Asteroids")]
    [SerializeField]
    int xMin;
    [SerializeField]
    int xMax;
    [SerializeField]
    int yMin;
    [SerializeField]
    int yMax;
    [SerializeField]
    int zMin;
    [SerializeField]
    int zMax;

    [Header("Random Asteroid Number")]
    [SerializeField] Vector2 babiesToSpawn_;

    [Header("Time Between Asteroid Spawns")]
    [SerializeField] float timeBetweenAsteroidSpawns_;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    private void Start()
    {
        Invoke("SpawnField", .1f);

    }

    public void SpawnField()
    {
        float _randomNumberOfAsteroids = Random.Range(babiesToSpawn_.x, babiesToSpawn_.y);

        for (int i = 0; i < _randomNumberOfAsteroids; i++)
        {
            SpawnAsteroids();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnAsteroids()
    {
        GameObject _pools = GameObject.Find("Enemy Pools");

        ObjectPooler_LIST _objectPooler = _pools.GetComponent<ObjectPooler_LIST>();


        int xRandom = Random.Range(xMin, xMax);
        int yRandom = Random.Range(yMin, yMax);
        int zRandom = Random.Range(zMin, zMax);

        Vector3 spawnHere = new Vector3(xRandom, yRandom, zRandom);


        _objectPooler.SpawnFromPool("Asteroif Field", spawnHere, Quaternion.identity);


    }

}
