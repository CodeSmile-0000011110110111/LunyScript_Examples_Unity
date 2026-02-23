using LunyScratch;
using System;
using UnityEngine;
using static LunyScratch.Blocks;

public sealed class FlashingLights : ScratchBehaviour
{
	[SerializeField] private Double _flashLeftEnabledDuration = 0.16;
	[SerializeField] private Double _flashLeftDisabledDuration = 0.12;

	[SerializeField] private Double _flashRightEnabledDuration = 0.17;
	[SerializeField] private Double _flashRightDisabledDuration = 0.13;

	protected override void OnScratchReady()
	{
		base.OnScratchReady();

		const String FlashLeft = "FlashLeft";
		const String FlashRight = "FlashRight";

		Run(Disable(FlashLeft), Disable(FlashRight));

		// blinking signal lights
		RepeatForever(
			Enable(FlashLeft),
			Wait(_flashLeftEnabledDuration),
			Disable(FlashLeft),
			Wait(_flashLeftDisabledDuration)
		);
		RepeatForever(
			Disable(FlashRight),
			Wait(_flashRightDisabledDuration),
			Enable(FlashRight),
			Wait(_flashRightEnabledDuration)
		);
	}
}
