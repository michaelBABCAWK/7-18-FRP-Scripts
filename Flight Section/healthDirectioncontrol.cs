using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthDirectioncontrol : MonoBehaviour
{
    public bool m_useRelativeRotation = true;


    private Quaternion m_relativeRotation;

    // Start is called before the first frame update
    void Start()
    {
        m_relativeRotation = transform.parent.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_useRelativeRotation)
        {
            transform.rotation = m_relativeRotation;
        }
    }
}
