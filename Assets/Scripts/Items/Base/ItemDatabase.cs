using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Items.Base
{
    [CreateAssetMenu(menuName = "Items/Database")]
    public class ItemDatabase : Utils.ScriptableSingleton<ItemDatabase>
    {
        public IReadOnlyCollection<ItemDefinition> definitions => selfDefinitions;
        
        [Tooltip("View only, filled automatically")] [SerializeField]
        private List<ItemDefinition> selfDefinitions;

        public ItemDefinition GetDefinition(ItemIdentifier id)
        {
            return selfDefinitions.First(x => x.id.Equals(id));
        }

#if UNITY_EDITOR
        public ItemDefinition TryGetDefinition(ItemIdentifier id)
        {
            return selfDefinitions.FirstOrDefault(x => x.id.Equals(id));
        }

        public void AddDefinition(ItemDefinition definition)
        {
            var existingDefinitions = selfDefinitions.FindAll(x => x.id.Equals(definition.id));

            if (existingDefinitions.Count > 0)
            {
                var names = string.Join("\n", existingDefinitions);

                throw new Exception(
                    $"Item with this identification is already exists.\nItem to add: {definition}\nDatabase items: {names}");
            }

            selfDefinitions.Add(definition);
        }

        public void DeleteDefinition(ItemDefinition definition)
        {
            selfDefinitions.RemoveAll(x => x.Equals(definition));
        }

        public void FillIn()
        {
            var type = typeof(ItemDefinition).Name;

            var guids = AssetDatabase.FindAssets("t:" + type);

            selfDefinitions.Clear();

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var definition = AssetDatabase.LoadAssetAtPath<ItemDefinition>(path);

                AddDefinition(definition);
            }
        }

        private void OnEnable()
        {
            FillIn();
        }
#endif
    }
}