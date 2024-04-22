using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawner : CarSpawner
{
    private protected override void SpawnCar()
    {
        base.SpawnCar();
        SKAudioManager.instance.PlaySound("train horn");
    }
}
