using System.Collections.Generic;
using UnityEngine;

namespace Tools.Editing
{
    public class SelectContainer : MonoBehaviour
    {
        public bool HasSelection { get; private set; }

        private List<GameObject> _selectedObjects;

        public void SetSelectedObjects(List<GameObject> selectedObjects)
        {
            _selectedObjects = selectedObjects;
        }
    }
}