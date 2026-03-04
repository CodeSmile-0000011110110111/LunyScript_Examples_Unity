using LunyScript;
using System;

namespace Sistato.LunyScripts
{
	public class Player : Script
	{
		public override void Build(ScriptContext context)
		{
			const Single Speed = 9f;

			When.InputAction("Move").Performing(Transform.ShiftBy(Input.Direction("Move"), Speed));
			When.InputAction("Look").Performing(Transform.SetLocalRotation(Input.Rotation("Look")));

			//InputAction("Move").WhenStarted().WhenPerformed().WhenCanceled().WhilePerforming();
			//When.InputAction("Move").Started().Performed().Canceled();
			//While.InputAction("Move").Performing();

			/*
			On.FrameUpdate(
				Transform.SetLocalRotation(Input.Rotation("Look")),
				Transform.ShiftBy(Input.Direction("Move"), Speed)
			);
			*/

			On.Collision().Layered("Enemy").Cooldown(1).Begins(Debug.Log(">>>>> OUCH !!! <<<<<<"));

			Run(MyActionMethod());
			Run(MyVoidMethod);

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
