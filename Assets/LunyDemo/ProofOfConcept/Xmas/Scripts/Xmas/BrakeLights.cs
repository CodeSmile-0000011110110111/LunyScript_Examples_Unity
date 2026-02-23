using LunyScratch;
using System;
using static LunyScratch.Blocks;

public sealed class BrakeLights : ScratchBehaviour
{
	protected override void OnScratchReady()
	{
		base.OnScratchReady();

		const String Brakelights = "BrakeLights";

		Run(Disable(Brakelights));

		// blinking signal lights
		RepeatForever(
			If(IsKeyPressed(Key.S),
					Enable(Brakelights))
				.Else(Disable(Brakelights))
		);
	}
}
