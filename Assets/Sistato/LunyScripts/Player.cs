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
				Transform.SetLocalRotation(Input.Rotation("Look")),
				Transform.ShiftBy(Input.Direction("Move"), Speed)
			);

			On.Collision.Layered("Enemy").Cooldown(1).Begins(Debug.Log(">>>>> OUCH !!! <<<<<<"));

			//For(3).Do(Debug.Log("for 3"));
			//Coroutine("hello").For(1).Seconds().Do(Debug.Log("for a second"));
			Every(10).Heartbeats().Do(Debug.Log("bu-bumm"));
			Every(60).Heartbeats().Do(Debug.Log("60 frames passed"));

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
