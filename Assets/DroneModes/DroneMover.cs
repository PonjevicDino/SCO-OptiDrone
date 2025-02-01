using System.Collections;
using System.Collections.Generic;
using TelloLib;
using UnityEngine;

public class DroneMover : MonoBehaviour
{
    public (float, float, float, float) CalculateFlightMotion(Transform drone, Vector3 target, float targetAngle)
    {
        float distanceToTarget = Vector3.Distance(drone.position, target);
        Vector3 deltaPosToTarget = target - drone.position;

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
        droneVertical = AllignHeightWithTarget(target.y, drone.position.y);

        float droneForward = 0.0f;
        float droneHorizontal = 0.0f;

        (droneForward, droneHorizontal) = MoveHorizontal(angleToTarget, distanceToTarget);

        return (droneLateral, droneVertical, droneForward, droneHorizontal);
    }

    private float AllignAngleWithTarget(float angleToTarget)
    {
        float lx = 0f;

        if (Mathf.Abs(angleToTarget) > 45.0f)
        {
            lx = angleToTarget / Mathf.Abs(angleToTarget);
        }
        else
        {
            lx = angleToTarget / 45.0f;
        }

        return lx;
    }

    private float AllignHeightWithTarget(float targetHeight, float droneHeight)
    {
        float ly = 0f;

        if (Mathf.Abs(targetHeight - droneHeight) > 1.0f) //0.1f)
        {
            ly = (targetHeight - droneHeight) / Mathf.Abs(targetHeight - droneHeight);
        }
        else
        {
            ly = targetHeight - droneHeight; //* 10.0f;
        }

        return ly;
    }

    private (float, float) MoveHorizontal(float targetAngle, float targetDistance)
    {
        float rx = 0.0f;
        float ry = 0.0f;

        rx = Mathf.Sin(targetAngle * Mathf.Deg2Rad) * targetDistance;
        ry = Mathf.Cos(targetAngle * Mathf.Deg2Rad) * targetDistance;

        if (targetAngle <= -90.0f && targetAngle >= 90.0f)
        {
            ry *= -1;
        }

        rx = Mathf.Clamp(rx, -1.0f, 1.0f);
        ry = Mathf.Clamp(ry, -1.0f, 1.0f);

        return (rx, ry);
    }
}
