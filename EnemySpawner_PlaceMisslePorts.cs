using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_PlaceMisslePorts : MonoBehaviour
{
    public LayerMask terrain_;
    public LayerMask avoid_;

    public GameObject missleSpawner_;
    public float spawnRadius_;
    public int spawnPerTile_;

    public bool forAsteroid_;

    // Start is called before the first frame update
    void Start()
    {
        if (!forAsteroid_)
        {
            for (int i = 0; i < spawnPerTile_; i++)
            {
                Vector2 _spawnPositionV2 = Random.insideUnitCircle * spawnRadius_;

                Vector3 _spawnPosition = new Vector3(_spawnPositionV2.x, 0.0f, _spawnPositionV2.y);

                Vector3 _transformOffsetSpawnPosition = transform.position + _spawnPosition;


                RaycastHit _hit;
                if (!Physics.Raycast(_transformOffsetSpawnPosition, Vector3.down, avoid_))//if the terrain is there
                {
                    Quaternion _orientation;
                    if (Physics.Raycast(_transformOffsetSpawnPosition, Vector3.down, out _hit))
                    {
                        _orientation = missleSpawner_.transform.rotation;
                        //_orientation = Quaternion.LookRotation(_hit.normal);

                        Vector3 _finalSpawnPosition = _hit.point;
                        Instantiate(missleSpawner_, _finalSpawnPosition, _orientation);
                    }
                }

            }
        }
        else
        {
            for (int i = 0; i < spawnPerTile_; i++)
            {
                Vector2 _spawnPositionV2 = Random.insideUnitSphere * spawnRadius_;

                Vector3 _spawnPosition = new Vector3(_spawnPositionV2.x, 0.0f, _spawnPositionV2.y);

                Vector3 _transformOffsetSpawnPosition = transform.position + _spawnPosition;


                RaycastHit _hit;
                if (!Physics.Raycast(_transformOffsetSpawnPosition, Vector3.down, avoid_))//if the terrain is there
                {
                    Quaternion _orientation;
                    if (Physics.Raycast(_transformOffsetSpawnPosition, Vector3.down, out _hit))
                    {
                        _orientation = missleSpawner_.transform.rotation;
                        //_orientation = Quaternion.LookRotation(_hit.normal);

                        Vector3 _finalSpawnPosition = _hit.point;
                        Instantiate(gameObject.transform.GetChild(0).gameObject, _finalSpawnPosition, _orientation);
                    }
                }

            }
        }
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
