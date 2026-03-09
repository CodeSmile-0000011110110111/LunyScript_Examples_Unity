using LunyScript;
using System;

namespace Sistato.LunyScripts
{
	public class Player : Script
	{
		public override void Build(ScriptContext context)
		{
			const Single Speed = 9f;

			When.Input.Action("Move")
				.Begins(Debug.Log("Move begins ..."))
				.Changes(Debug.Log("Move value changes ..."))
				.Continues(Transform.ShiftBy(Input.Direction("Move"), Speed))
				.Ends(Debug.Log("Move ends ..."));
			When.Input.Action("Look").Continues(Transform.SetLocalRotation(Input.Rotation("Look")));

			var cameraMode = Var["CameraMode"];
			cameraMode.Set(0);

			var followCameraName = "CineCam (Flat Iso Player Follow)";
			When.Input.Action("ToggleCamera")
				.Changes(If(cameraMode != 1)
					.Then(Debug.Log("ENTER topdown"), cameraMode.Inc(), Object.Disable(followCameraName))
					.Else(Debug.Log("EXIT topdown"), cameraMode.Dec(), Object.Enable(followCameraName)));

			On.Ready(Var["count"].Set(0),
				For(10).Do(Debug.Log("for ...")),
				While(Var["count"] < 10).Do(Var["count"].Inc(), Debug.Log("while ..."), Debug.Log(Var["count"])));

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
