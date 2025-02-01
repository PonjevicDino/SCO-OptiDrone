using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePath : MonoBehaviour
{
    private LineRenderer dronePath;
    [SerializeField] Transform drone;

    void Start()
    {
        dronePath = this.GetComponent<LineRenderer>();
        dronePath.positionCount = 0;
    }

    void Update()
    {
        dronePath.positionCount += 1;
        dronePath.SetPosition(dronePath.positionCount - 1, drone.position);

        if (Input.GetKeyDown(KeyCode.R))
        {
            dronePath.positionCount = 0;
        }
    }
}
