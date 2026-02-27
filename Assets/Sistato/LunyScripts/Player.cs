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
				Transform.SetLocalRotation(Input.Rotation("Look")),
				Transform.ShiftBy(Input.Direction("Move"), Speed)
			);

			On.Collision.Layered("Enemy").Cooldown(1).Begins(Debug.Log(">>>>> OUCH !!! <<<<<<"));

			Run(MyActionMethod());
			Run(MyVoidMethod);

			// Counts frames or heartbeats: In() => once-only, Every() => repeating
			Counter("counter in beats").In(60).Heartbeats().Do(Debug.Log("counter in beats"));
			Counter("counter every beats").Every(60).Heartbeats().Do(Debug.Log("counter every beats"));
			Counter("counter in frames").In(60).Frames().Do(Debug.Log("counter in frames"));
			Counter("counter every frames").Every(60).Frames().Do(Debug.Log("counter every frames"));

			//For(3).Do(Debug.Log("for 3"));
			//Coroutine("hello").For(1).Seconds().Do(Debug.Log("for a second"));
			EveryBuilderStartEx.Do(Every(10).Heartbeats(), Debug.Log("bu-bumm"));
			EveryBuilderStartEx.Do(Every(60).Heartbeats(), Debug.Log("60 frames passed"));

			/*
			On.Collision
				.Layered("Enemy", "Enemy")
				.Typed(typeof(RigidbodyStayUpright), typeof(Rigidbody))
				.Named("Enemy", "Enemy")
				.Begins(Debug.Log("ouch"));
				*/
		}

		private Action MyActionMethod() => () => Debug.Log("Hello World!");

		private void MyVoidMethod() => Debug.Log("Hello World!");
	}
}
