using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainCamera
{
    public class GridResizer : MonoBehaviour
    {
        [SerializeField] private float maxGridThickness = 0.22f;
        [SerializeField] private Vector2 thicknessMultiplier = new(0.00532213f, 0.00339f);
        [SerializeField] private float gridPlaneMultiplier = 1.4f;

        private Camera _mainCamera;
        private MeshRenderer _meshRenderer;
        private readonly int _gridThickness = Shader.PropertyToID("_Grid_Thickness");

        private void Awake()
        {
            _mainCamera = Camera.main;
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            UpdateThickness();
            UpdatePlaneSize();
        }

        private void UpdateThickness()
        {
            var thickness = _mainCamera.orthographicSize * thicknessMultiplier.x + thicknessMultiplier.y;
            if (thickness > maxGridThickness)
            {
                _meshRenderer.enabled = false;
                return;
            }

            _meshRenderer.enabled = true;
            _meshRenderer.materials[0].SetFloat(_gridThickness, thickness);
        }

        private void UpdatePlaneSize()
        {
            var size = _mainCamera.orthographicSize;
            var scale = transform.localScale;
            scale.x = gridPlaneMultiplier * size;
            scale.z = size;
            transform.localScale = scale;
        }
    }
}
