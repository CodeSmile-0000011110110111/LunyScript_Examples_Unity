using Luny;
using LunyScript;

namespace Sistato.LunyScripts
{
	public class EnemySpawner : Script
	{
		public override void Build(ScriptBuildContext context)
		{
			var engine = LunyEngine.Instance;
			var objects = engine.Objects;
			var enemiesFolder = objects.Find("Enemies");

			var createEnemy = Object.Create("Enemy").From("Assets/Sistato/Prefabs/Enemy").Parent(enemiesFolder);
			Coroutine("Spawn Enemy").Every(.25).Seconds().WhenElapsed(createEnemy, createEnemy, createEnemy);
		}
	}
}
