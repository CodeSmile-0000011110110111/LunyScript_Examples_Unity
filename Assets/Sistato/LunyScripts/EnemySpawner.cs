using Luny;
using LunyScript;
using LunyScript.BlockBuilders;
using UnityEngine.Localization.SmartFormat.Utilities;

namespace Sistato.LunyScripts
{
	public class EnemySpawner : Script
	{
		public override void Build(ScriptContext context)
		{
			var engine = LunyEngine.Instance;
			var objects = engine.Objects;
			var enemiesFolder = objects.Find("Enemies");

			Timer("Spawn Enemy")
				.Every(.25)
				.Seconds()
 			.Do(Object.Create("Enemy").From("Assets/Sistato/Prefabs/Enemy").Parent(enemiesFolder));

			//On.ObjectCreated("Enemy").Do(..);

			//.At(Spatial.RandomInBounds(spawnArea))
		}
	}
}
