using LunyScript;
using System;
using UnityEngine;

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

			On.Collision.Layered("Enemy").Cooldown(1).Begins(Debug.Log(">>>>> OUCH !!! <<<<<<"));

			/*
			On.Collision
				.Layered("Enemy", "Enemy")
				.Typed(typeof(RigidbodyStayUpright), typeof(Rigidbody))
				.Named("Enemy", "Enemy")
				.Begins(Debug.Log("ouch"));
				*/
		}
	}
}
