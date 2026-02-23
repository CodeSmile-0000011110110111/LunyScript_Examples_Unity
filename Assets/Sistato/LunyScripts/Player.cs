using LunyScript;
using System;

namespace Sistato.LunyScripts
{
	public class Player : Script
	{
		public override void Build(ScriptContext context)
		{
			const Single Speed = 15f;

			On.Ready(Debug.Log($"Hello, {nameof(Player)}"));
			On.FrameUpdate(Transform.MoveBy(Input.Direction("Move"), Speed));
		}
	}
}
