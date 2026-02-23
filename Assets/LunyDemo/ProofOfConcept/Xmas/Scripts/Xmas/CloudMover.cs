using LunyScratch;
using System;
using UnityEngine;
using static LunyScratch.Blocks;

public class CloudMover : ScratchBehaviour
{
    [SerializeField] private Double _speed = 0.16;

    protected override void OnScratchReady()
    {
        base.OnScratchReady();

        // blinking signal lights
        RepeatForever(MoveForward((float)_speed));
    }
}
