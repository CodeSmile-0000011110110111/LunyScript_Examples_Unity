using LunyScript;
using System;

namespace Sistato.LunyScripts
{
	public class Player : Script
	{
		public override void Build(ScriptContext context)
		{
			const Single Speed = 9f;

			On.FrameUpdate(
				//If(Input.Rotation("Look").Length > 0.5f).Then(..),
				Transform.SetLocalRotation(Input.Rotation("Look")),
				Transform.ShiftBy(Input.Direction("Move"), Speed)
			);

			On.Collision.Begins(Debug.Log("ouch"));

			//On.CollisionWith("Enemy").Do(blocks);
			// On.CollisionStarted(Debug.Log("OnCollisionStarted ==>"));
			// On.CollisionEnded(Debug.Log("OnCollisionEnded <=="));
			// On.Colliding(Debug.Log("OnColliding..."));
			//
			// On.TriggerEntered(Debug.Log("OnTriggerEntered ==>"));
			// On.TriggerExited(Debug.Log("OnTriggerExited <=="));
			// On.Triggering(Debug.Log("OnTriggering..."));
		}
	}
}
