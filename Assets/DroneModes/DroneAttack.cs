using System;
using System.Collections;
using System.Collections.Generic;
using TelloLib;
using UnityEngine;

public class DroneAttack : MonoBehaviour
{
    [SerializeField] private GameObject drone;
    private bool attackModeEnabled = false;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attackModeEnabled = !attackModeEnabled;
        }
        if (!attackModeEnabled)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(drone.transform.position, this.transform.position);
        Vector3 deltaPosToTarget = this.transform.position - drone.transform.position;

        deltaPosToTarget = new Vector3(deltaPosToTarget.x, 0.0f, deltaPosToTarget.z);

        float angleToTarget = Vector3.Angle(drone.transform.right, deltaPosToTarget);
        Vector3 crossAngle = Vector3.Cross(drone.transform.right, deltaPosToTarget);
        if (crossAngle.y < 0)
        {
            angleToTarget *= -1;
        }

        Debug.Log("Target | Distance: " + distanceToTarget.ToString("0.000") + "m - Angle: " + angleToTarget.ToString("000.000") + "°");

        float droneLateral = 0.0f;
        droneLateral = AllignAngleWithTarget(angleToTarget);
        float droneVertical = 0.0f;
        droneVertical = AllignHeightWithTarget(this.transform.position.y, drone.transform.position.y);

        float droneForward = 0.0f;
        float droneHorizontal = 0.0f;
        //droneForward = MoveForwardTowardsTarget(angleToTarget, distanceToTarget);

        (droneForward, droneHorizontal) = MoveForAttack(angleToTarget, distanceToTarget);

        ControlDrone(droneLateral, droneVertical, droneForward, droneHorizontal);
    }

    private float AllignAngleWithTarget(float angleToTarget)
    {
        float lx = 0f;

        if (Mathf.Abs(angleToTarget) > 20.0f)
        {
            lx = angleToTarget / Mathf.Abs(angleToTarget);
        }
        else
        {
            lx = angleToTarget / 20.0f;
        }

        return lx;
    }

    private float AllignHeightWithTarget(float targetHeight, float droneHeight)
    {
        float ly = 0f;
        targetHeight += 0.2f;

        if (Mathf.Abs(targetHeight - droneHeight) > 0.5f)
        {
            ly = (targetHeight - droneHeight) / Mathf.Abs(targetHeight - droneHeight);
        }
        else
        {
            ly = targetHeight - droneHeight * 2.0f;
        }

        return ly;
    }

    private float MoveForwardTowardsTarget(float angleToTarget, float distanceToTarget)
    {
        float ry = 0f;

        if (Mathf.Abs(angleToTarget) <= 20.0)
        {
            if (distanceToTarget >= 0.2f)
            {
                ry = 1.0f;
            }
            else
            {
                ry = distanceToTarget * 5.0f;
            }
        }

        return ry;
    }

    private (float, float) MoveForAttack(float targetAngle, float targetDistance)
    {
        float rx = 0.0f;
        float ry = 0.0f;

        rx = Mathf.Sin(targetAngle * Mathf.Deg2Rad);
        ry = Mathf.Cos(targetAngle * Mathf.Deg2Rad);

        if (targetAngle < 0.0f)
        {
            //rx *= -1;
        }
        if (targetAngle <= -90.0f && targetAngle >= 90.0f)
        {
            ry *= -1;
        }

        //rx *= targetDistance;
        //ry *= targetDistance;

        rx = Mathf.Clamp(rx, -1.0f, 1.0f);
        ry = Mathf.Clamp(ry, -1.0f, 1.0f);

        return (rx, ry);
    }

    private void ControlDrone(float droneLateral, float droneVertical, float droneHorizontal, float droneForward)
    {
        Debug.Log("RX: " + droneHorizontal + " | RY: " + droneForward);

        Tello.controllerState.setAxis(droneLateral, droneVertical, droneHorizontal, droneForward);
        
        if (Tello.controllerState.speed != 1)
        {
            Tello.controllerState.setSpeedMode(1);
        }
    }
}
