using System.Collections.Generic;
using UnityEngine;

namespace Tools.Editing
{
    public class SelectContainer : MonoBehaviour
    {
        [SerializeField] private LayerMask selectionLayer;
        
        public bool HasSelection { get; private set; }

        private List<GameObject> _selectedObjects;

        public void SetSelectedObjects(List<GameObject> selectedObjects)
        {
            HasSelection = true;
            _selectedObjects = selectedObjects;
            foreach (var selected in _selectedObjects)
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
                if (_selectedObjects.Contains(selected)) continue;
                _selectedObjects.Add(selected);
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
                if (!_selectedObjects.Contains(selected)) continue;
                _selectedObjects.Remove(selected);
                var sprite = selected.GetComponent<SpriteRenderer>();
                var color = sprite.color;
                color.a = .9f;
                sprite.color = color;
            }
        }
        
        public void ClearSelection()
        {
            HasSelection = false;
            foreach (var selected in _selectedObjects)
            {
                // selected.layer = 0;
                var sprite = selected.GetComponent<SpriteRenderer>();
                var color = sprite.color;
                color.a = .9f;
                sprite.color = color;
            }
        }
    }
}