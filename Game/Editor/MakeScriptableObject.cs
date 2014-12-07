using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableObject : MonoBehaviour {

    [MenuItem("Assets/Create/Ball Prefab List")]
    public static void CreateMyAsset()
    {
        BallPrefabs asset = ScriptableObject.CreateInstance<BallPrefabs>();

        AssetDatabase.CreateAsset(asset, "Assets/Ball Prefab List.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
