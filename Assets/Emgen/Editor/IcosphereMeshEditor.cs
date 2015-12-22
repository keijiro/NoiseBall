using UnityEngine;
using UnityEditor;
using System.IO;

namespace Emgen
{
    [CustomEditor(typeof(IcosphereMesh))]
    public class IcosphereMeshEditor : Editor
    {
        SerializedProperty _subdivisionLevel;
        SerializedProperty _splitVertices;

        void OnEnable()
        {
            _subdivisionLevel = serializedObject.FindProperty("_subdivisionLevel");
            _splitVertices = serializedObject.FindProperty("_splitVertices");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_subdivisionLevel);
            EditorGUILayout.PropertyField(_splitVertices);
            var rebuild = EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();

            if (rebuild)
                foreach (var t in targets)
                    ((IcosphereMesh)t).RebuildMesh();
        }

        [MenuItem("Assets/Create/Emgen/Icosphere Mesh")]
        public static void CreateIcosphereMeshAsset()
        {
            // Make a proper path from the current selection.
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (Path.GetExtension(path) != "")
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            var assetPathName = AssetDatabase.GenerateUniqueAssetPath(path + "/Icosphere.asset");

            // Create an IcosphereMesh asset.
            var asset = ScriptableObject.CreateInstance<IcosphereMesh>();
            AssetDatabase.CreateAsset(asset, assetPathName);
            AssetDatabase.AddObjectToAsset(asset.sharedMesh, asset);

            // Build an initial mesh for the asset.
            asset.RebuildMesh();

            // Save the generated mesh asset.
            AssetDatabase.SaveAssets();

            // Tweak the selection.
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
