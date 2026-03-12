using Luny;
using LunyScript;
using LunyScript.Api;
using LunyScript.Unity;
using LunyScript.Unity.Blocks;
using UnityEngine;

namespace Sistato.LunyScripts
{
	public class Enemy : Script
	{
		public override void Build(ScriptContext context)
		{
			var engine = LunyEngine.Instance;
			var objects = engine.Objects;
			var player = objects.GetCached("Player");

			var isDead = Var["is dead"];

			On.FrameUpdate(
				If(!isDead)
					.Then(
						Transform.MoveTowards(player).Speed(3).LockY(),
						Transform.RotateTowards(player).Responsiveness(0.05).Slerp()
					)
			);

			//Timer("speed bump").Every(1).Seconds().Do(Var["speed"].Inc());

			Coroutine("kill")
				.In(12)
				.Seconds()
				.WhenElapsed(isDead.Set(true),
					Component.Disable(typeof(RigidbodyStayUpright)),
					Component.Disable(typeof(GooglyEyesFocus)));

			Coroutine("disappear").In(18).Seconds().WhenElapsed(Component.Disable(typeof(CapsuleCollider)));
			Coroutine("destroy").In(20).Seconds().WhenElapsed(Object.Destroy());
		}
	}
}
