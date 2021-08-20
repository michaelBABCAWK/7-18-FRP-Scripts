using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCheck : MonoBehaviour
{
    public GameObject other_;

    float dist_;

    // Start is called before the first frame update
    void Start()
    {
        dist_ = Vector3.Magnitude(other_.transform.position - this.transform.position);
        print(dist_);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
