using Luny;
using LunyScript;
using LunyScript.BlockBuilders;
using UnityEngine;

namespace Sistato.LunyScripts
{
	public class Enemy : Script
	{
		public override void Build(ScriptContext context)
		{
			var engine = LunyEngine.Instance;
			var objects = engine.Objects;
			var player = objects.Get("Player");

			var isDead = Var["is dead"];

			On.FrameUpdate(
				If(!isDead)
					.Then(
						Transform.MoveTowards(player).Speed(3).LockY().Do(),
						Transform.RotateTowards(player).Responsiveness(0.05).Slerp()
					)
			);

			//Timer("speed bump").Every(1).Seconds().Do(Var["speed"].Inc());

			Timer("kill")
				.In(12)
				.Seconds()
				.Do(isDead.Set(true),
					Component.Disable(typeof(RigidbodyStayUpright)),
					Component.Disable(typeof(GooglyEyesFocus))
					);
			Timer("disappear").In(18).Seconds().Do(
				Component.Disable(typeof(CapsuleCollider)));

			Timer("destroy").In(20).Seconds().Do(Object.Destroy());
		}
	}
}
