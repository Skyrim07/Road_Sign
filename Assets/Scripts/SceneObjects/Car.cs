using SKCell;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
public class Car : MonoBehaviour
{
    [SKFolder("Basic Behaviour")]
    public float maxSpeed = 3f;
    public float acceleration = 1f;

    [SKFolder("Wiggle")]
    public float wiggleFrequency = 1f;
    public float wiggleAmplitude = 1f;

    [SKFolder("Advanced Behaviour")]
    public float turnDistanceFactor = 1.2f;
    public float raycastFwdLength = 1f, raycastSignLength = 3f;

    [SKFolder("References")]
    [SerializeField] Transform centerPos,frontPos;
    [SerializeField] Transform[] raycastPoses;
    [SerializeField] Transform[] visionPoints;
   

    public Queue<Transform> waypoints = new Queue<Transform>();



    //sign vars
    //private bool isInStopSign;


    [SerializeField] private float stopWaitTime = 12f;
    private bool isInStopSign;

    private bool otherCarWatchingSign;
    private bool exitingIntersection;


    private Road currentRoad;
    private int currentDirInRoad;
    //private int targetWaypointIndex;
    private float speed, targetSpeed;
    private float rotationSpeed, rotationDelta;
    private bool isOnRoad;
    private bool shouldStop;
    private int wiggleDirection = 1;

    List<Sign> watchedSigns = new List<Sign>();

    private void Start()
    {
        targetSpeed = maxSpeed;
    }
    private void FixedUpdate()
    {
        speed = Mathf.Lerp(speed, targetSpeed, acceleration * .05f);


        //Check for obstacles ahead'
        shouldStop = false; 
        exitingIntersection= false;
        otherCarWatchingSign = false;
        shouldStop = CheckForObstacles();
        CheckForSigns();
        shouldStop |= isInStopSign;

        if (shouldStop)
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
            //Debug.Log(waypoints.Count);
            //Debug.Log(isOnRoad);
            if (waypoints.Count==0) // If there is nowhere to go
            {
                if ( !exitingIntersection)
                {
                }
                isOnRoad = false;
            }
            if (currentRoad != null)
            {
                if( waypoints.TryPeek(out var targetWaypoint))
                {
                    //Debug.Log(targetWaypoint + ": " + targetWaypoint.parent.parent);
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

        //Wiggle
        /*
        float w = Mathf.Sin(Time.time * wiggleFrequency) * wiggleAmplitude;
        w += wiggleDirection * wiggleFrequency;
        if (w >= 1) wiggleDirection = -1;
        if (w <=0) wiggleDirection = 1;
        transform.Rotate(new Vector3(0, 0, w));
        */
    }

    private void CheckForSigns()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(centerPos.position, (transform.rotation * Vector2.right), raycastSignLength);
        Debug.DrawLine(centerPos.position, centerPos.position + (transform.rotation * Vector2.right) * raycastSignLength, Color.red, 0.02f);
        foreach(var hit in hits)
        {
            if(hit.transform!=null && hit.transform.CompareTag("SignSlot"))
            {
                SignSlot slot = hit.transform.GetComponent<SignSlot>();
                if(slot.road == currentRoad)
                    foreach(var sign in slot.signs)
                    {
                        SignBehavior(sign);
                    }
            }
        }
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
        float distanceFactor = turnDistanceFactor; 
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
        Pedestrian p = hit.transform.GetComponent<Pedestrian>();
        stop |= p != null;
        /*
        if (c!=null)
        {
            otherCarWatchingSign |= CheckForSignWatch(c);
        }
        */
        return stop;
    }
    private bool CheckForSignWatch(Car car)
    {
        bool signWatch = false;
        if (car.isInStopSign || car.otherCarWatchingSign)
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
        FlowManager.instance.OnCollisionHappens(1f, p.gameObject);
    }
    private void OnHitCar(Car car)
    {
        FlowManager.instance.OnCollisionHappens(0.5f, gameObject);
        Destroy(gameObject);
    }
    private void OnHitPlayer(PlayerLogic player)
    {
        FlowManager.instance.OnPlayerCollision();
    }
    private void OnHitDestination(Destination dest)
    {
        print("Reach destination!");
        LevelManager.instance.AddProgressValue(.05f);
        Destroy(gameObject);
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
        if (collision.TryGetComponent<Destination>(out var dest))
        {
            OnHitDestination(dest);
        }
        if(collision.TryGetComponent<PlayerLogic>(out var player))
        {
            OnHitPlayer(player);
        }
    }
    private void SignBehavior(Sign sign)
    {
        if (sign == null) return;
        if (watchedSigns.Contains(sign))
            return;

        watchedSigns.Add(sign);
        if(sign.type == SignType.Stop)
        {
            StartCoroutine(StopSign());
        }
    }

    private IEnumerator StopSign()
    {
        targetSpeed = 0;
        float freeTime = 0;
        isInStopSign = true;
        yield return new WaitForSeconds(stopWaitTime);

        bool isFree = false;
        while (!isFree)
        {
            freeTime += .2f * RuntimeData.timeScale;
            for (int i = 0; i < visionPoints.Length; i++)
            { 
               Vector2 dir = visionPoints[i].position -frontPos.position;
                RaycastHit2D[] hits = Physics2D.RaycastAll(frontPos.position, dir.normalized, dir.magnitude);
                foreach(var hit in hits)
                {
                    if (hit.transform != null && (hit.transform.CompareTag("Car") || hit.transform.CompareTag("Pedestrian")))
                    {
                        freeTime = 0;
                        break;
                    }
                }
                Debug.DrawLine(frontPos.position, frontPos.position +(Vector3) dir * dir.magnitude, Color.blue, .2f);
            }
            if (freeTime >= .6f)
                isFree = true;
            yield return new WaitForSeconds(.2f * RuntimeData.timeScale);
        }

        isInStopSign = false;
    }

}
