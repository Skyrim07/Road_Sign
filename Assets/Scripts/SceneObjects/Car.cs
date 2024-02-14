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

    public Queue<Transform> waypoints = new Queue<Transform>();
    //sign vars
    [SerializeField] private float stopWaitTime = 6f;


    private Road currentRoad;
    private int currentDirInRoad;
    //private int targetWaypointIndex;
    private float speed, targetSpeed;
    private float rotationSpeed, rotationDelta;
    private bool isOnRoad;
    private bool shouldStop;
    private bool watchingSign;
    private bool otherCarWatchingSign;

    private void Start()
    {
        targetSpeed = maxSpeed;
    }
    private void FixedUpdate()
    {
        speed = Mathf.Lerp(speed, targetSpeed, acceleration * .05f);


        //Check for obstacles ahead
        shouldStop = false;
        otherCarWatchingSign= false;
        shouldStop = CheckForObstacles();

        if (shouldStop || watchingSign || otherCarWatchingSign)
        {
            targetSpeed = 0;
        }
        else
        {
            targetSpeed = maxSpeed;
        }

        //Waypoint approaching
        if (currentRoad != null)
        {
            
            if (waypoints.Count==0) // If there is nowhere to go
            {
                if (!watchingSign && currentRoad.mySignSlot.isOccupied)
                {
                    watchingSign = true;
                    SignBehavior(currentRoad.mySignSlot.mySign);
                }
                //currentRoad = currentRoad.connectTo.Count==0?null:currentRoad.connectTo[Random.Range(0, currentRoad.connectTo.Count)];
                isOnRoad = false;
                //PrepareEnterNewRoad();
            }
            if (currentRoad != null)
            {
                if( waypoints.TryPeek(out var targetWaypoint))
                {
                    if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
                    {
                        Vector3 from = transform.position, to = targetWaypoint.position;
                        Quaternion rfrom = transform.rotation;
                        Quaternion rto = Quaternion.Euler(0, 0, currentDirInRoad == 1 ? targetWaypoint.rotation.eulerAngles.z : 180 + targetWaypoint.rotation.eulerAngles.z);
                        SKUtils.StartProcedure(SKCurve.QuadraticDoubleIn, .2f, (t) =>
                        {
                            transform.rotation = Quaternion.Slerp(rfrom, rto, t);
                            transform.position = Vector3.Lerp(from, to, t);
                        });

                        rotationDelta = 0;
                        waypoints.Dequeue();
                        isOnRoad = true;
                    }
                }

            }
        }

        //Movement
        transform.Translate((transform.rotation * Vector2.up) * speed * Time.fixedDeltaTime * RuntimeData.timeScale, Space.World);
        float rotationFactor = speed / maxSpeed;
        transform.Rotate(0, 0, rotationFactor *rotationDelta * Time.fixedDeltaTime * RuntimeData.timeScale);

    }


    private bool CheckForObstacles()
    {
        bool check = false;
        foreach (var raycastPos in raycastPoses)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(raycastPos.position, (transform.rotation * Vector2.up), raycastFwdLength);
            Debug.DrawLine(raycastPos.position, raycastPos.position + (transform.rotation * Vector2.up) * raycastFwdLength, Color.red, 0.02f);


            check |= CheckHitStop(hits);
            check &= isOnRoad;
        }
        return check;
        
    }
    private void PrepareEnterNewRoad()
    {
        if (currentRoad==null || currentRoad.waypoints.Count == 0) return;
        Transform from = transform;
        Transform to = waypoints.Peek();
        float distanceFactor = 1.07f; 
        float dist = Vector2.Distance(from.position, to.position) * distanceFactor;
        float time = dist / maxSpeed;
        float currentAngle = from.eulerAngles.z;
        float targetAngle = currentDirInRoad == 1? to.eulerAngles.z : 180 + to.eulerAngles.z;
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
        if(stop)
        {
            otherCarWatchingSign |= CheckForSignWatch(c);
        }
        return stop;
    }
    private bool CheckForSignWatch(Car car)
    {
        bool signWatch = false;
        if(car.watchingSign || car.otherCarWatchingSign)
        {
            signWatch = true;
        }
        return signWatch;
    }

    private void OnEnterIntersection(Intersection intersection)
    {
        Road targetRoad = intersection.GetRandomTargetRoadWithRestrictions(currentRoad);

        if (targetRoad != null)
        {
            //Get the nearest waypoint of the target road
            int nearestIndex = 0;
            float nearest = float.MaxValue;
            for (int i = 0; i < targetRoad.waypoints.Count; i++)
            {
                float dist = Vector2.Distance(transform.position, targetRoad.waypoints[i].position);
                if (dist < nearest)
                {
                    nearestIndex = i;
                    nearest = dist;
                }
            }
            currentDirInRoad = nearestIndex == 0 ? 1 : -1;
            waypoints.Clear();
            waypoints.Enqueue(targetRoad.waypoints[nearestIndex]); //Go to the nearest waypoint of the target road
            currentRoad = targetRoad;


            PrepareEnterNewRoad();
        }

    }
    private void OnEnterRoad(Road road)
    {
        int nearestIndex = 0;
        float nearest = float.MaxValue;
        for (int i = 0; i < road.waypoints.Count; i++)
        {
            float dist = Vector2.Distance(transform.position, road.waypoints[i].position);
            if(dist < nearest)
            {
                nearestIndex = i;
                nearest = dist;
            }
        }
        currentDirInRoad = nearestIndex == 0 ? 1 : -1;
        waypoints.Enqueue(road.waypoints[1 - nearestIndex]); //Go to the other waypoint
        currentRoad = road;
    }
    private void OnHitPedestrian(Pedestrian p)
    {
        print("Collision with pedestrian!");
        FlowManager.instance.OnCollisionHappens();
    }
    private void OnHitCar(Car car)
    {
        print("Collision with car!");
        FlowManager.instance.OnCollisionHappens();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Intersection>(out var intersection))
        {
            OnEnterIntersection(intersection);
        }
        if (collision.TryGetComponent<Pedestrian>(out var pedestrain))
        {
            OnHitPedestrian(pedestrain);
        }
        if (collision.TryGetComponent<Car>(out var car))
        {
            OnHitCar(car);
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
        targetSpeed= 0;
        watchingSign = true;
        yield return new WaitForSeconds(stopWaitTime/20);
        watchingSign = false;
    }

}
