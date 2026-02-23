// 12/10/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

// 12/10/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabReplaceEditor : EditorWindow
{
    public GameObject newPrefab;

    //[MenuItem("CodeSmile/Replace Child Prefabs")]
    public static void ShowWindow() => GetWindow<PrefabReplaceEditor>("Replace Child Prefabs");

    private void OnGUI()
    {
        GUILayout.Label("Replace Child Prefabs", EditorStyles.boldLabel);

        newPrefab = (GameObject)EditorGUILayout.ObjectField("New Prefab", newPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Replace Child Prefabs"))
            ReplaceChildPrefabs();
    }

    private void ReplaceChildPrefabs()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("No GameObject selected. Please select a GameObject in the hierarchy.");
            return;
        }

        if (newPrefab == null)
        {
            Debug.LogError("No new prefab assigned. Please assign a new prefab.");
            return;
        }

        GameObject selectedObject = Selection.activeGameObject;
        Debug.Log($"Selected GameObject: {selectedObject.name}");

        // Collect all children of the direct children of the selected object
        List<Transform> prefabInstances = new List<Transform>();
        foreach (Transform directChild in selectedObject.transform)
        {
            foreach (Transform child in directChild)
            {
                if (PrefabUtility.IsPartOfPrefabInstance(child.gameObject))
                {
                    prefabInstances.Add(child);
                }
            }
        }


        // Replace prefab instances
        List<GameObject> newChildren = new List<GameObject>();
        foreach (Transform child in prefabInstances)
        {
            // Save position and rotation of the current child
            Vector3 position = child.position;
            Quaternion rotation = child.rotation;

            // Instantiate the new prefab
            GameObject newChild = (GameObject)PrefabUtility.InstantiatePrefab(newPrefab);
            newChild.transform.position = position;
            newChild.transform.rotation = rotation;
            newChild.transform.SetParent(child.parent); // Set parent to the original parent of the replaced child

            newChildren.Add(newChild);

            // Add the old child to the list for deletion
            DestroyImmediate(child.gameObject);
        }

        // Save the modified prefab
        string path = AssetDatabase.GetAssetPath(selectedObject);
        Debug.Log($"Prefab Path: {path}");
        PrefabUtility.SaveAsPrefabAsset(selectedObject, path);
        Debug.Log("Child prefabs replaced and prefab saved successfully.");
    }
}
