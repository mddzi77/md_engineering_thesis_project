using MouseGridPosition;
using TheLayers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Drawer
{
    public class Rectangle : DrawerAbstract
    {
        [SerializeField] private InputActionReference leftMouse;
        [SerializeField] private InputActionReference rightMouse;
        [SerializeField] private InputActionReference modifierAction;
        
        private LayersManager _layerManager;
        private Vector2 _startPos;
        private Vector2 _endPos;
        private bool _startPointSet;

        private void Start()
        {
            _layerManager = LayersManager.Instance;
            leftMouse.action.performed += OnLeftMouse;
            rightMouse.action.performed += OnRightMouse;
        }
        
        private void OnLeftMouse(InputAction.CallbackContext context)
        {
            _startPos = MouseGrid.GridPos;
        }
        
        private void OnRightMouse(InputAction.CallbackContext context)
        {
            
        }
    }
}