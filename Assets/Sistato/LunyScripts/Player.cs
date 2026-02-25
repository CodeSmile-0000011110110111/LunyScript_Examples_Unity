using LunyScript;
using System;

namespace Sistato.LunyScripts
{
	public class Player : Script
	{
		public override void Build(ScriptContext context)
		{
			const Single Speed = 12f;

			On.FrameUpdate(
				//If(Input.Rotation("Look").Length > 0.5f).Then(..),
				Transform.SetLocalRotation(Input.Rotation("Look")),
				Transform.ShiftBy(Input.Direction("Move"), Speed)
			);

			//On.CollisionWith("Enemy").Do(blocks);
		}
	}
}
