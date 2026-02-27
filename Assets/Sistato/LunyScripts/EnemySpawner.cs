using Luny;
using LunyScript;
using LunyScript.ApiBuilders.Object;

namespace Sistato.LunyScripts
{
	public class EnemySpawner : Script
	{
		public override void Build(ScriptContext context)
		{
			var engine = LunyEngine.Instance;
			var objects = engine.Objects;
			var enemiesFolder = objects.Find("Enemies");

			var createEnemy = Object.Create("Enemy").From("Assets/Sistato/Prefabs/Enemy").Parent(enemiesFolder);

			Timer("Spawn Enemy").Every(.25).Seconds().Do(createEnemy, createEnemy, createEnemy);

			//On.ObjectCreated("Enemy").Do(..);

			//.At(Spatial.RandomInBounds(spawnArea))
		}
	}
}
