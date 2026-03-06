using Luny;
using LunyScript;

namespace Sistato.LunyScripts
{
	public class EnemySpawner : Script
	{
		public override void Build(ScriptContext context)
		{
			var engine = LunyEngine.Instance;
			var objects = engine.Objects;
			var enemiesFolder = objects.Find("Enemies");

			var createEnemy = Object.Create("Enemy").With("Assets/Sistato/Prefabs/Enemy").Parent(enemiesFolder);
			Coroutine("Spawn Enemy").Every(.25).Seconds().WhenElapsed(createEnemy, createEnemy, createEnemy);
		}
	}
}
