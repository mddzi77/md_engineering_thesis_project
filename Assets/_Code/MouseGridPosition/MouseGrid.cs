using MdUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MouseGridPosition
{
    public class MouseGrid : MonoSingleton<MouseGrid>
    {
        [SerializeField] private InputActionReference mousePosition;
        
        public static Vector2 GridPos => _gridPos;

        private static Vector2 _gridPos;
        private Camera _camera;

        private new void Awake()
        {
            base.Awake();
            _camera = Camera.main;
        }

        private void Update()
        {
            var mouseScreenPos = mousePosition.action.ReadValue<Vector2>();
            var mouseWorldPos = _camera.ScreenToWorldPoint(mouseScreenPos);
            CalculateGridPosition(mouseWorldPos);
        }
        

        private void CalculateGridPosition(Vector3 mouseWorldPos)
        {
            _gridPos = new Vector2(
                Mathf.Floor(mouseWorldPos.x),
                Mathf.Floor(mouseWorldPos.y)
            );
        }
    }
}