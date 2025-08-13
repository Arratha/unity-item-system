using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Items.Base
{
    //Determines what item prefab belongs to
    //Id managed automaticallyz
    public class Item : MonoBehaviour
    {
        public ItemIdentifier identifier => selfId;
        [HideInInspector, SerializeField] private ItemIdentifier selfId;

        public void Destroy()
        {
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        public void SetPrefabId(ItemIdentifier id)
        {
            if (Application.isPlaying)
            {
                throw new Exception("You cannot update ID's during runtime.");
            }

            SetId(id);
            EditorUtility.SetDirty(this);

            EditorApplication.update += SaveAssets;
        }

        private void SaveAssets()
        {
            EditorApplication.update -= SaveAssets;
            AssetDatabase.SaveAssets();
        }

        private void SetId(ItemIdentifier id)
        {
            if (Application.isPlaying)
            {
                throw new Exception("You cannot update ID's during runtime.");
            }

            selfId = id;
        }
#endif
    }
}