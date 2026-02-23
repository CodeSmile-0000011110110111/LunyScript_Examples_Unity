using LunyScratch;
using System;
using UnityEngine;

public sealed class XmasTreeLights : ScratchBehaviour
{
	[SerializeField] private Double _enabledDuration = 0.16;

	protected override void OnScratchReady()
	{
		base.OnScratchReady();

		const String Red = "RedLights";
		const String Yellow = "YellowLights";

		Run(Blocks.Disable(Red), Blocks.Disable(Yellow));

		// blinking signal lights
		RepeatForever(
			Blocks.Enable(Red),
			Blocks.Wait(_enabledDuration),
			Blocks.Disable(Red),
			Blocks.Wait(_enabledDuration)
		);
		RepeatForever(
			Blocks.Disable(Yellow),
			Blocks.Wait(_enabledDuration),
			Blocks.Enable(Yellow),
			Blocks.Wait(_enabledDuration)
		);
	}
}
