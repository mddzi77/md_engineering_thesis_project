using System;
using System.Collections.Generic;
using TheLayers;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tools.Editing
{
    public class SelectContainer : MonoBehaviour
    {
        [SerializeField] private int selectionLayer = 3;
        [ReadOnly]
        [SerializeField] private List<GameObject> selectedObjects;
        [SerializeField] private List<LayerTag> layerTags;
        
        public bool HasSelection { get; private set; }
        
        private bool _reparent;

        public void SetSelectedObjects(List<GameObject> newSelected)
        {
            if (HasSelection)
            {
                ClearSelection();
            }
            HasSelection = true;
            selectedObjects.Clear();
            selectedObjects.AddRange(newSelected);
            foreach (var selected in selectedObjects)
            {
                selected.layer = selectionLayer;
                var sprite = selected.GetComponent<SpriteRenderer>();
                var color = sprite.color;
                color.a = 0.3f;
                sprite.color = color;
            }
        }
        
        public void AddSelectedObjects(List<GameObject> selectedObject)
        {
            foreach (var selected in selectedObject)
            {
                if (selectedObjects.Contains(selected)) continue;
                selectedObjects.Add(selected);
                selected.layer = selectionLayer;
                var sprite = selected.GetComponent<SpriteRenderer>();
                var color = sprite.color;
                color.a = 0.3f;
                sprite.color = color;
            }
        }
        
        public void RemoveSelectedObjects(List<GameObject> selectedObject)
        {
            foreach (var selected in selectedObject)
            {
                if (!selectedObjects.Contains(selected)) continue;
                selectedObjects.Remove(selected);
                var sprite = selected.GetComponent<SpriteRenderer>();
                var color = sprite.color;
                color.a = .95f;
                sprite.color = color;
            }
        }
        
        public void ClearSelection()
        {
            HasSelection = false;
            foreach (var selected in selectedObjects)
            {
                selected.layer = LayerMask.NameToLayer("Tile");
                var sprite = selected.GetComponent<SpriteRenderer>();
                var color = sprite.color;
                color.a = .95f;
                sprite.color = color;
            }
            ResetParent();
            selectedObjects.Clear();
        }

        public void ReParent(Transform parent)
        {
            _reparent = true;
            foreach (var selected in selectedObjects)
            {
                selected.transform.parent = parent;
            }
        }

        public void ResetParent()
        {
            if (!_reparent) return;
            ForEachSelected(SortByTag);
            _reparent = false;
        }
        
        public void ForEachSelected(Action<GameObject> action)
        {
            foreach (var selected in selectedObjects)
            {
                action(selected);
            }
        }

        private void SortByTag(GameObject obj)
        {
            foreach (var layerTag in layerTags)
            {
                if (obj.tag.Equals(layerTag.Tag))
                {
                    var newParent = LayersManager.Instance.LayerHolders[layerTag.layer].transform;
                    obj.transform.parent = newParent;
                }
            }
        }

        [Serializable]
        public class LayerTag
        {
            public LayerConfig layer;
            public string Tag => layer.LayerName;
        }
    }
}