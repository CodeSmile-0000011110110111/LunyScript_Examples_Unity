using Luny;
using LunyScript;
using LunyScript.Api;
using LunyScript.Blocks;
using LunyScript.Unity.Blocks;
using UnityEngine;

namespace Sistato.LunyScripts
{
	public class Enemy : Script
	{
		public override void Build(ScriptBuildContext context)
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


			var prefabPath = "Assets/Prefabs/Enemy";
			for (var i = 0; i < 10; i++)
			{
				var name = $"Enemy_{i+1}";
				var createEnemy = Object.Create(name).With(prefabPath);

				On.Ready(createEnemy);

			}

			var enemyCount = Var.Define("num enemies to spawn", 3);
			On.Enabled(For(enemyCount).Do(
				Object.Create("Enemy").With(prefabPath)),
				enemyCount.Add(2)
			);

		}

	}
}
