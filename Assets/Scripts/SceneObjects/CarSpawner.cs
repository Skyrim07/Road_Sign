using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SKFolder("Behaviour")]
    public float angle;
    public float spawnIntervalMin= 2, spawnIntervalMax = 5;
    public bool spawnAtStart = true;

    [SKFolder("References")]
    public GameObject[] carPrefabs;

    private float interval;
    private float timer;

    private void Start()
    {
        GetNewInterval();
        if (spawnAtStart)
        {
            SKUtils.InvokeAction(1f, () =>
            {
                SpawnCar();
            });
        }
 
    }
    private void Update()
    {
        timer += Time.deltaTime * RuntimeData.timeScale;
        if(timer > interval)
        {
            SpawnCar();
            GetNewInterval();
            timer = 0;
        }

    }

    private void GetNewInterval()
    {
        interval = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }
    private protected virtual void SpawnCar()
    {
        GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], transform.position, Quaternion.Euler(0, 0, angle));

    }
}
