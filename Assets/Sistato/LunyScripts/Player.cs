using LunyScript;
using System;

namespace Sistato.LunyScripts
{
	public class Player : Script
	{
		public override void Build(ScriptContext context)
		{
			const Single Speed = 12f;

			//On.Ready(Debug.Log($"Hello, {nameof(Player)}"));
			On.FrameUpdate(
				//If(Input.Direction("Look") != Variable.FromVector2(LunyVector2.Zero)).Then(Transform.SetLocalRotation(Input.Rotation("Look"))),
				Transform.SetLocalRotation(Input.Rotation("Look")),
				Transform.ShiftBy(Input.Direction("Move"), Speed)
			);
		}
	}
}
