using System.IO;
using System.Linq;
using Items.Base;
using UnityEditor;
using UnityEngine;

namespace Editor.Items
{
    public static class ItemDefinitionCreator
    {
        [MenuItem("Assets/Create/Items/Item Definition")]
        public static void CreateItemDefinitionWithDialog()
        {
            var defaultName = "NewItemDefinition";
            var path = EditorUtility.SaveFilePanelInProject(
                "Create Item Definition",
                defaultName,
                "asset",
                "Specify the name and location for the new Item Definition"
            );

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var folderPath = Path.GetDirectoryName(path);
            var definitionName = Path.GetFileNameWithoutExtension(path);

            var definition = ScriptableObject.CreateInstance<ItemDefinition>();
            AssetDatabase.CreateAsset(definition, path);

            var identifier = ScriptableObject.CreateInstance<ItemIdentifier>();
            var identifierPath =
                AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{definitionName}_Identifier.asset");
            AssetDatabase.CreateAsset(identifier, identifierPath);

            var field = typeof(ItemDefinition).GetField("selfId",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
                field.SetValue(definition, identifier);

            EditorUtility.SetDirty(definition);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = definition;
        }

        [MenuItem("Assets/Create/Items/Item Definitions From Selected Prefabs")]
        public static void CreateItemDefinitionsFromSelectedPrefabs()
        {
            var selectedPrefabs = Selection.gameObjects
                .Where(go => PrefabUtility.IsPartOfAnyPrefab(go))
                .ToArray();

            if (selectedPrefabs.Length == 0)
            {
                EditorUtility.DisplayDialog("No Prefabs Selected",
                    "Please select prefabs in the Project window first.", "OK");
                return;
            }

            var folderPath = EditorUtility.OpenFolderPanel(
                "Select Folder for Item Definitions",
                "Assets",
                "");

            if (string.IsNullOrEmpty(folderPath))
            {
                return;
            }

            if (!folderPath.StartsWith(Application.dataPath))
            {
                EditorUtility.DisplayDialog("Invalid Folder",
                    "Please select a folder within your Unity project's Assets folder.", "OK");
                return;
            }

            var relativeFolderPath = "Assets" + folderPath.Substring(Application.dataPath.Length);

            foreach (var prefab in selectedPrefabs)
            {
                var prefabName = prefab.name;
                var definitionPath = $"{relativeFolderPath}/{prefabName}_Definition.asset";
                var identifierPath = $"{relativeFolderPath}/{prefabName}_Identifier.asset";

                var definition = ScriptableObject.CreateInstance<ItemDefinition>();
                definitionPath = AssetDatabase.GenerateUniqueAssetPath(definitionPath);
                AssetDatabase.CreateAsset(definition, definitionPath);

                var identifier = ScriptableObject.CreateInstance<ItemIdentifier>();
                identifierPath = AssetDatabase.GenerateUniqueAssetPath(identifierPath);
                AssetDatabase.CreateAsset(identifier, identifierPath);

                var field = typeof(ItemDefinition).GetField("selfId",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (field != null)
                    field.SetValue(definition, identifier);

                EditorUtility.SetDirty(definition);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}