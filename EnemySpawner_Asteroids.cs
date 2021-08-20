using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_Asteroids : MonoBehaviour
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

    public bool forMissleAsteroid_;

    ObjectPooler_LIST objectPooler;

    private void Awake()
    {
        objectPooler = ObjectPooler_LIST.Instance;

    }

    // Start is called before the first frame update
    private void Start()
    {

        if (!forMissleAsteroid_)
        {
            StartCoroutine("SpawnAsteroids", timeBetweenAsteroidSpawns_);

        }
        else
        {
            Invoke("SpawnMisslePlatforms", timeBetweenAsteroidSpawns_);

        }

    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator SpawnAsteroids(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            float _randomNumberOfAsteroids = Random.Range(babiesToSpawn_.x, babiesToSpawn_.y);

            for (int i = 0; i < _randomNumberOfAsteroids; i++)
            {

                int xRandom = Random.Range(xMin, xMax);
                int yRandom = Random.Range(yMin, yMax);
                int zRandom = Random.Range(zMin, zMax);

                Vector3 spawnHere = new Vector3(xRandom, yRandom, zRandom);


                ObjectPooler_LIST.Instance.SpawnFromPool("Asteroid", spawnHere, Quaternion.identity);
            }            
        }
    }

    public void SpawnMisslePlatforms()
    {

        float _randomNumberOfAsteroids = Random.Range(babiesToSpawn_.x, babiesToSpawn_.y);

        for (int i = 0; i < _randomNumberOfAsteroids; i++)
        {

            int xRandom = Random.Range(xMin, xMax);
            int yRandom = Random.Range(yMin, yMax);
            int zRandom = Random.Range(zMin, zMax);

            Vector3 spawnHere = new Vector3(xRandom, yRandom, zRandom);


            ObjectPooler_LIST.Instance.SpawnFromPool("AsteroidForMissles", spawnHere, Quaternion.identity);
        }

    }
}
