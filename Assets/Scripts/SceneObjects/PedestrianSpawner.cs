using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    [SKFolder("Behaviour")]
    public float angle, speed;
    public float spawnIntervalMin = 2, spawnIntervalMax = 5;
    public bool spawnAtStart = true;

    [SKFolder("References")]
    public GameObject[] pedPrefabs;

    private float interval;
    private float timer;

    private void Start()
    {
        GetNewInterval();
        if (spawnAtStart)
            SpawnCar();
    }
    private void Update()
    {
        timer += Time.deltaTime * RuntimeData.timeScale;
        if (timer > interval)
        {
            SpawnCar();
            timer = 0;
        }

    }

    private void GetNewInterval()
    {
        interval = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }
    private void SpawnCar()
    {
        Pedestrian ped = Instantiate(pedPrefabs[Random.Range(0, pedPrefabs.Length)], transform.position, Quaternion.Euler(0, 0, angle)).GetComponent<Pedestrian>();
        ped.direction = SKUtils.Angle2Vector(angle);
        ped.speed = speed;

    }
}
