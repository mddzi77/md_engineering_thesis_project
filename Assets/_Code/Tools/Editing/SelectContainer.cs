using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tools.Editing
{
    public class SelectContainer : MonoBehaviour
    {
        [SerializeField] private LayerMask selectionLayer;
        [ReadOnly]
        [SerializeField] private List<GameObject> selectedObjects;
        
        public bool HasSelection { get; private set; }

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
                // selected.layer = selectionLayer;
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
                // selected.layer = 0;
                var sprite = selected.GetComponent<SpriteRenderer>();
                var color = sprite.color;
                color.a = .95f;
                sprite.color = color;
            }
            selectedObjects.Clear();
        }

        public void ReParent(Transform parent)
        {
            foreach (var selected in selectedObjects)
            {
                selected.transform.parent = parent;
            }
        }

        public void ResetParent()
        {
            
        }
        
        public void ForEachSelected(Action<GameObject> action)
        {
            foreach (var selected in selectedObjects)
            {
                action(selected);
            }
        }
    }
}