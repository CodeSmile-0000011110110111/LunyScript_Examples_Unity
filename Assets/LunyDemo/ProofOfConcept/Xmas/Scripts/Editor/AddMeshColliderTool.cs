// 12/7/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEditor;
using UnityEngine;

public class AddMeshColliderTool : MonoBehaviour
{
	//[MenuItem("CodeSmile/Add MeshCollider to Selected Objects %#m")]
	private static void AddMeshCollidersToSelectedObjects()
	{
		// Get all selected GameObjects in the scene
		var selectedObjects = Selection.gameObjects;

		if (selectedObjects.Length == 0)
		{
			Debug.LogWarning("No GameObjects selected. Please select one or more GameObjects in the scene.");
			return;
		}

		foreach (var obj in selectedObjects)
		{
			// Get all MeshRenderers in the selected GameObject and its children
			var meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();

			foreach (var meshRenderer in meshRenderers)
			{
				var meshCollider = meshRenderer.GetComponent<MeshCollider>();

				if (meshCollider == null)
				{
					// Add a MeshCollider if it doesn't exist
					meshCollider = meshRenderer.gameObject.AddComponent<MeshCollider>();
					meshCollider.convex = true;
					Debug.Log($"Added MeshCollider to GameObject: {meshRenderer.name}");
				}
				else
				{
					// Log if a MeshCollider already exists
					meshCollider.convex = true;
					Debug.Log($"MeshCollider already exists on GameObject: {meshRenderer.name}");
				}
			}
		}
	}
}
