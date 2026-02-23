using LunyScratch;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class DayNightCycleScratch : ScratchBehaviour
{
	[SerializeField] private Single _timeIncrement = 0.02f;

	protected override void OnScratchReady()
	{
		var rotIncrement = Quaternion.Euler(_timeIncrement, 0, 0);
		RepeatForeverPhysics(new ExecuteBlock(() =>
		{
			var rot = transform.localRotation;
			rot *= rotIncrement;
			transform.localRotation = rot;
		}));
	}
}
