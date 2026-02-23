using LunyScratch;
using System;
using UnityEngine;

public sealed class StringBulbLights : ScratchBehaviour
{
	[SerializeField] private Double _enableNextDuration = 0.3;

	protected override void OnScratchReady()
	{
		base.OnScratchReady();

		Run(//Blocks.Disable("Light0"),
			Blocks.Disable("Light1"), Blocks.Disable("Light2"), Blocks.Disable("Light3"), Blocks.Disable("Light4"));

		RepeatForever(
			Blocks.Enable("Light0"),
			Blocks.Wait(_enableNextDuration),
			Blocks.Enable("Light1"),
			Blocks.Wait(_enableNextDuration),
			Blocks.Enable("Light2"),
			Blocks.Wait(_enableNextDuration),
			Blocks.Enable("Light3"),
			Blocks.Wait(_enableNextDuration),
			Blocks.Enable("Light4"),
			Blocks.Wait(_enableNextDuration),
			Blocks.Disable("Light0"),
			Blocks.Wait(_enableNextDuration),
			Blocks.Disable("Light1"),
			Blocks.Wait(_enableNextDuration),
			Blocks.Disable("Light2"),
			Blocks.Wait(_enableNextDuration),
			Blocks.Disable("Light3"),
			Blocks.Wait(_enableNextDuration),
			Blocks.Disable("Light4"),
			Blocks.Wait(_enableNextDuration)
		);
	}
}
