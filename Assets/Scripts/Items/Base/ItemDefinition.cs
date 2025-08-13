using System;
using UnityEngine;

namespace Items.Base
{
    [Flags]
    public enum ItemTag
    {
        None = 0,
        Metallic = 1 << 0,
        Bag = 1 << 1,
        Concealed = 1 << 2,
        Special = 1 << 3,
        Radioactive = 1 << 4,
        GasDetectable = 1 << 5
    }

    [Flags]
    public enum ItemPlacement
    {
        None = 0,
        Hands = 1 << 0,
        Pocket = 1 << 1,
        Bag = 1 << 2
    }

    [Flags]
    public enum IllegalTag
    {
        None = 0,
        Weapon = 1 << 0,
        Ammunition = 1 << 1,
        NarcoticSubstances = 1 << 2,
        ExplosiveDevices = 1 << 3,
        ExplosiveSubstances = 1 << 4,
        FlammableSubstances = 1 << 5,
        PoisonousSubstances = 1 << 6,
        RadioactiveSubstances = 1 << 7,
        Other = 1 << 8
    }

    //Defines item id, tags and prefab
    //Id managed automatically
    public class ItemDefinition : ScriptableObject
    {
        public string itemName => selfName;
        [SerializeField] private string selfName;
        
        public ItemIdentifier id => selfId;
        [HideInInspector, SerializeField] private ItemIdentifier selfId;

        public ItemTag tag => selfTag;
        [SerializeField] private ItemTag selfTag;

        public ItemPlacement placement => selfPlacement;
        [SerializeField] private ItemPlacement selfPlacement;

        public IllegalTag illegal => selfIllegal;
        [SerializeField] private IllegalTag selfIllegal; 
        
        public Item prefab;
        private Item _prefabCopy;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            ValidatePrefab();
        }

        private void ValidatePrefab()
        {
            if (prefab == null && _prefabCopy == null)
            {
                return;
            }

            if (prefab == null)
            {
                _prefabCopy.SetPrefabId(null);
                _prefabCopy = null;
                return;
            }

            if (_prefabCopy == null)
            {
                prefab.SetPrefabId(selfId);
                _prefabCopy = prefab;
            }
        }
#endif
    }
}