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

			var createEnemy = Object.Create("Enemy").From("Assets/Sistato/Prefabs/Enemy").Parent(enemiesFolder);

			TimerBuilderStartEx.Do(TimerBuilderStartEx.Seconds(Timer("Spawn Enemy").Every(15.25)), createEnemy, createEnemy, createEnemy);

			//On.ObjectCreated("Enemy").Do(..);

			//.At(Spatial.RandomInBounds(spawnArea))
		}
	}
}
