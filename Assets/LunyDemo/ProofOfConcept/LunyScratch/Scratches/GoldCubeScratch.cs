using LunyScratch;
using UnityEngine;
using static LunyScratch.Blocks;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public sealed class GoldCubeScratch : ScratchBehaviour
{
	protected override void OnScratchReady()
	{
		var globalTimeout = GlobalVariables["MiniCubeSoundTimeout"];
		When(CollisionEnter(),
			If(AND(IsVariableLessThan(globalTimeout, 0), IsCurrentSpeedGreater(10)),
				PlaySound(), SetVariable(globalTimeout, 0)));
	}
}
