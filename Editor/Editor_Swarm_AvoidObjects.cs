using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Swarm_AvoidObjects))]
public class Editor_Swarm_AvoidObjects : Editor
{
    private void OnSceneGUI()
    {
        Swarm_AvoidObjects fow = (Swarm_AvoidObjects)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius_);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.ViewAngle_ / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.ViewAngle_ / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius_);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius_);

        Handles.color = Color.red;

        foreach (Transform visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }
}