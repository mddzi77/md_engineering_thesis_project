using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace MdUtils.InputCombos
{
    [Serializable]
    public class InputComboWithMain
    {
        [SerializeField] private InputActionReference mainAction;
        [ListDrawerSettings(Draggable = true, AlwaysExpanded = true)]
        [SerializeField] private List<InputActionReference> modifierActions;
        
        public bool IsPressed => _isPressed;
        public bool IsMainPressed => _isMainPressed;
        public event Action Performed;
        public event Action Cancelled;
        public event Action MainPerformed;
        public event Action MainCancelled;
        
        private bool _isPressed;
        private bool _isMainPressed;

        private void Update()
        {
            if (!mainAction.action.IsPressed())
            {
                Cancel();
                CancelMain();
                return;
            }
            PerformMain();
            if (modifierActions.Count == 0)
            {
                Perform();
                return;
            }
            foreach (var action in modifierActions)
            {
                if (action.action.IsPressed()) continue;
                Cancel();
                return;
            }

            Perform();
        }
        
        private void Perform()
        {
            if (!_isPressed) return;
            _isPressed = false;
            Performed?.Invoke();
        }
        
        private void Cancel()
        {
            if (!_isPressed) return;
            _isPressed = false;
            Cancelled?.Invoke();
        }
        
        private void PerformMain()
        {
            if (!_isMainPressed) return;
            _isMainPressed = false;
            MainPerformed?.Invoke();
        }
        
        private void CancelMain()
        {
            if (!_isMainPressed) return;
            _isMainPressed = false;
            MainCancelled?.Invoke();
        }
    }

    // [CustomPropertyDrawer(typeof(InputComboWithMain))]
    // public class InputComboWithMainDrawer : PropertyDrawer
    // {
    //     public override VisualElement CreatePropertyGUI(SerializedProperty property)
    //     {
    //         var root = new VisualElement();
    //         
    //         var mainAction = property.FindPropertyRelative("mainAction");
    //         var modifierActions = property.FindPropertyRelative("modifierActions");
    //         
    //         var mainField = new PropertyField(mainAction);
    //         var modifierField = new PropertyField(modifierActions);
    //         
    //         root.Add(new Label("Main Action"));
    //         root.Add(mainField);
    //         root.Add(modifierField);
    //         
    //         return root;
    //     }
    // }
}