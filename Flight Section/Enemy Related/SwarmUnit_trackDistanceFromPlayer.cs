using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmUnit_trackDistanceFromPlayer : MonoBehaviour
{
    public static float Distance;

    [SerializeField] float distanceToTrigger;

    Transform Target;

    GameObject Player;

    Transform enemyPosition;

    public GameObject[] lasers;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");

        Target = Player.transform;

        enemyPosition = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        TrackDistance();
    }

    private void TrackDistance()
    {
        Distance = Vector3.Distance(Target.position, enemyPosition.position);

        if (distanceToTrigger >= Distance)
        {
            lasers[0].SetActive(true);
            lasers[1].SetActive(true);
            lasers[2].SetActive(true);
        }
        else
        {
            lasers[0].SetActive(false);
            lasers[1].SetActive(false);
            lasers[2].SetActive(false);
        }
    }
}
