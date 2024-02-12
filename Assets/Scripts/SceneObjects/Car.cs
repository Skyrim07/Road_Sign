using SKCell;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
public class Car : MonoBehaviour
{
    public float maxSpeed = 3f;
    public float acceleration = 1f;

    public float raycastFwdLength = 1f;
    [SerializeField] Transform[] raycastPoses;

    //sign vars
    [SerializeField] private float stopWaitTime = 10f;


    private Road currentRoad;
    private int targetWaypointIndex;
    private float speed, targetSpeed;
    private float rotationSpeed, rotationDelta;
    private bool isOnRoad;
    private bool shouldStop;
    private bool watchingSign;

    private void Start()
    {
        targetSpeed = maxSpeed;
    }
    private void FixedUpdate()
    {
        speed = Mathf.Lerp(speed, targetSpeed, acceleration * .05f);


        //Check for obstacles ahead
        shouldStop = false;
        CheckForObstacles();

        //Waypoint approaching
        if (currentRoad != null)
        {
            
                if (targetWaypointIndex >= currentRoad.waypoints.Count)
                {
                if (currentRoad.mySignSlot.isOccupied)
                {
                    //SignBehavior(currentRoad.mySignSlot.mySign);
                }
                currentRoad = currentRoad.connectTo.Count == 0 ? null : currentRoad.connectTo[Random.Range(0, currentRoad.connectTo.Count)];
                    targetWaypointIndex = 0;
                    isOnRoad = false;
                    
                    PrepareEnterNewRoad();
                }
                if (currentRoad != null)
                {
                    Transform targetWaypoint = currentRoad.waypoints[targetWaypointIndex];
                    if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
                    {
                        Vector3 from = transform.position, to = targetWaypoint.position;
                        Quaternion rfrom = transform.rotation, rto = targetWaypoint.rotation;
                        SKUtils.StartProcedure(SKCurve.QuadraticDoubleIn, .2f, (t) =>
                        {
                            transform.rotation = Quaternion.Slerp(rfrom, rto, t);
                            transform.position = Vector3.Lerp(from, to, t);
                        });

                        rotationDelta = 0;
                        targetWaypointIndex++;
                        isOnRoad = true;
                    }
                }
            
        }

        //Movement
        transform.Translate((transform.rotation * Vector2.up) * speed * Time.fixedDeltaTime * RuntimeData.timeScale, Space.World);
        float rotationFactor = speed / maxSpeed;
        transform.Rotate(0, 0, rotationFactor *rotationDelta * Time.fixedDeltaTime * RuntimeData.timeScale);
    }

    private void CheckForObstacles()
    {
        foreach (var raycastPos in raycastPoses)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(raycastPos.position, (transform.rotation * Vector2.up), raycastFwdLength);
            Debug.DrawLine(raycastPos.position, raycastPos.position + (transform.rotation * Vector2.up) * raycastFwdLength, Color.red, 0.02f);


            shouldStop |= CheckHitStop(hits);
            shouldStop &= isOnRoad;
        }
        if (shouldStop)
        {
            targetSpeed = 0;
        }
        else
        {
            targetSpeed = maxSpeed;
        }
    }
    private void PrepareEnterNewRoad()
    {
        if (currentRoad==null || currentRoad.waypoints.Count == 0) return;
        Transform from = transform;
        Transform to = currentRoad.waypoints[targetWaypointIndex];
        float distanceFactor = 1f; 
        float dist = Vector2.Distance(from.position, to.position) * distanceFactor;
        float time = dist / maxSpeed;
        float currentAngle = from.eulerAngles.z;
        float targetAngle = to.eulerAngles.z;
        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
        rotationDelta = angleDifference / time;
    }

    private bool CheckHitStop(RaycastHit2D[] hit)
    {
        bool stop = false;
        for (int i = 0; i < hit.Length; i++)
        {
            stop |= CheckHitStop(hit[i]);
        }
        return stop;
    }
    private bool CheckHitStop(RaycastHit2D hit)
    {
        if (hit.transform == null) return false;
        bool stop = false;
        Car c = hit.transform.GetComponent<Car>();
        stop |= c != null;
        return stop;
    }

    private void OnEnterIntersection(Intersection intersection)
    {
        List<Road> roads = intersection.roads;
        Road targetRoad = roads[Random.Range(0,roads.Count)];
        while(targetRoad == currentRoad)
        {
            targetRoad = roads[Random.Range(0, roads.Count)];
        }

        currentRoad = targetRoad;
    }
    private void OnEnterRoad(Road road)
    {
        targetWaypointIndex = 0;
        currentRoad = road;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Intersection>(out var intersection))
        {
            //OnEnterIntersection(intersection);
        }
        if (collision.TryGetComponent<Road>(out var road))
        {
            OnEnterRoad(road);
        }
    }
    private void SignBehavior(Sign sign)
    {
        switch (sign.type)
        {
            case Sign.SignType.Stop:
                StartCoroutine(StopSign());
                break;
        }
    }
    private IEnumerator StopSign()
    {
        Debug.Log("stop sign");
        yield return new WaitForSeconds(stopWaitTime);
        //CheckForObstacles();
    }

}
