using LunyScratch;
using System;
using UnityEngine;
using static LunyScratch.Blocks;

public sealed class TurnLights : ScratchBehaviour
{
	[SerializeField] private Double _enabledDuration = 0.25;
	[SerializeField] private Double _disabledDuration = 0.15;

	protected override void OnScratchReady()
	{
		base.OnScratchReady();

		const String TurnLeft = "TurnLeft";
		const String TurnRight = "TurnRight";

		Run(Disable(TurnLeft), Disable(TurnRight));

		// blinking signal lights
		RepeatForever(
			If(IsKeyPressed(Key.A),
					Enable(TurnLeft),
					Wait(_enabledDuration),
					Disable(TurnLeft),
					Wait(_disabledDuration))
				.Else(Disable(TurnLeft)),
			If(IsKeyPressed(Key.D),
					Enable(TurnRight),
					Wait(_enabledDuration),
					Disable(TurnRight),
					Wait(_disabledDuration))
				.Else(Disable(TurnRight))
		);
	}
}
