using System.Collections;
using System.Collections.Generic;
using TelloLib;
using UnityEngine;

public class DroneFlightPath : MonoBehaviour
{
    private bool drawPath;
    private bool flyPath;
    private Vector3 nextWaypoint;
    private LineRenderer flightPath;

    private List<Vector3> flightPathAngles;

    [SerializeField] private Transform drone;

    private DroneMover droneMover;
    [SerializeField] private GameObject target;

    IEnumerator drawCoroutine;
    IEnumerator flyCoroutine;

    void Start()
    {
        flightPath = GameObject.Find("FlightPath").GetComponent<LineRenderer>();
        flightPathAngles = new List<Vector3>();

        droneMover = GameObject.Find("DroneMover").GetComponent<DroneMover>();

        drawCoroutine = DrawFlightPath();
        flyCoroutine = FollowFlightPath();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(flyCoroutine);
        }

        if (Input.GetKeyDown(KeyCode.P) && !drawPath && !flyPath)
        {
            drawPath = true;
            flyPath = false;
            StartCoroutine(drawCoroutine);
        }
        else if ((Input.GetKeyDown(KeyCode.P) && drawPath && !flyPath))
        {
            target.SetActive(true);
            drawPath = false;
            flyPath = true;
            StopCoroutine(drawCoroutine);
            StartCoroutine(flyCoroutine);
        }
        else if ((Input.GetKeyDown(KeyCode.P) && !drawPath && flyPath))
        {
            target.SetActive(false);
            drawPath = false;
            flyPath = false;
            StopCoroutine(flyCoroutine);
            drawCoroutine = DrawFlightPath();
            flyCoroutine = FollowFlightPath();
        }
    }

    private IEnumerator DrawFlightPath()
    {
        flightPath.positionCount = 0;
        flightPathAngles = new List<Vector3>();

        while (true)
        {
            flightPath.positionCount += 1;
            flightPath.SetPosition(flightPath.positionCount - 1, this.transform.position);
            flightPathAngles.Add(this.transform.right);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    private IEnumerator FollowFlightPath()
    {
        nextWaypoint = flightPath.GetPosition(0);
        int currentWaypoint = 0;
        Tello.takeOff();

        while (true)
        {
            target.transform.SetPositionAndRotation(nextWaypoint, Quaternion.identity);

            while (Vector3.Distance(drone.position, nextWaypoint) > 0.33f)
            {
                float lx, ly, rx, ry;
                (lx, ly, rx, ry) = droneMover.CalculateFlightMotion(drone, nextWaypoint, 0.0f);
                //rx = AllignAngleToTarget(drone, flightPathAngles[currentWaypoint]);
                if (Vector3.Distance(drone.position, nextWaypoint) < 0.5f)
                {
                    lx = 0.0f;
                }

                Tello.controllerState.setAxis(lx, ly, rx, ry);
                //Tello.controllerState.setSpeedMode(2);
                yield return new WaitForSecondsRealtime(0.1f);
            }

            if (currentWaypoint < flightPath.positionCount - 1)
            {
                currentWaypoint++;
            }
            else
            {
                currentWaypoint = 0;
            }
            nextWaypoint = flightPath.GetPosition(currentWaypoint);
        }
    }

    private float AllignAngleToTarget(Transform drone, Vector3 targetAngle)
    {
        float angleToTarget = Vector3.Angle(drone.transform.right, targetAngle);
        float lx = 0.0f;

        Vector3 crossAngle = Vector3.Cross(drone.transform.right, targetAngle);
        if (crossAngle.y < 0)
        {
            angleToTarget *= -1;
        }

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
}
