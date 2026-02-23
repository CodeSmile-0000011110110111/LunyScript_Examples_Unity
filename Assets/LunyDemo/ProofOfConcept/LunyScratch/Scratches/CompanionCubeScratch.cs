using LunyScratch;
using System;
using UnityEngine;
using static LunyScratch.Blocks;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public sealed class CompanionCubeScratch : ScratchBehaviour
{
	[SerializeField] private Single _minVelocityForSound = 25f;

	protected override void OnScratchReady()
	{
		var progressVar = GlobalVariables["Progress"];
		var counterVar = Variables["Counter"];

		// increment counter to be able to hit the ball again
		RepeatForever(AddVariable(counterVar, 5), Wait(1));

		Run(Disable("Lights"));

		When(CollisionEnter("police"),
			// play bump sound unconditionally and make cube glow
			PlaySound(), Enable("Lights"),
			// count down from current progress value to spawn more cube instances the longer the game progresses
			RepeatWhileTrue(() =>
			{
				if (counterVar.Number > progressVar.Number)
					counterVar.Set(Math.Clamp(progressVar.Number, 1, 50));
				counterVar.Subtract(1);
				return counterVar.Number >= 0;
			}, CreateInstance("Prefabs/HitEffect")),
			Wait(1), Disable("Lights"));

		// play sound when ball bumps into anything
		When(CollisionEnter(),
			If(IsCurrentSpeedGreater(_minVelocityForSound),
				PlaySound()));
	}
}
