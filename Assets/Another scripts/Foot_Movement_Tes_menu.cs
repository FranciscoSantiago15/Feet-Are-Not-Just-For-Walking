using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot_Movement_Tes_menu : MonoBehaviour
{
    public Transform pos1;

    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector3 orig = transform.position;
        Vector3 dest = pos1.position;

        transform.position = Vector3.Lerp(orig, dest, 0.1f);
    }
}
