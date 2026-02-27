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
			// Counter("counter in beats").In(60).Heartbeats().Do(Debug.Log("counter in beats"));
			// Counter("counter every beats").Every(60).Heartbeats().Do(Debug.Log("counter every beats"));
			// Counter("counter in frames").In(60).Frames().Do(Debug.Log("counter in frames"));
			// Counter("counter every frames").Every(60).Frames().Do(Debug.Log("counter every frames"));

			Every(2).Heartbeats().Do(Debug.Log("every even beat"));
			Every(2).Heartbeats().DelayBy(1).Do(Debug.Log("every odd beat"));
			Every(2).Frames().Do(Debug.Log("every even frame"));
			Every(2).Frames().DelayBy(1).Do(Debug.Log("every odd frame"));

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
