using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles_MoveBlastShield : MonoBehaviour
{
    public Transform _activePosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CoveringBlast()
    {
        transform.position = _activePosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
