using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle_StartFiring : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Boss_MissleBehavior>().enabled = true;
        Destroy(GetComponent<Missle_StartFiring>());
    }
}
