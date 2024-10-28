using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainCamera
{
    public class GridResizer : MonoBehaviour
    {
        [SerializeField] private Vector2 cameraZoomLimits = new(0.6f, 60f);
        [SerializeField] private Vector2 thicknessMultiplier = new(0.002f, 0.3f);
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
            var t = Mathf.Clamp01(
                (_mainCamera.orthographicSize - cameraZoomLimits.x) / (cameraZoomLimits.y - cameraZoomLimits.x)
                );
            if (1 - t < 0.001f)
            {
                _meshRenderer.enabled = false;
                return;
            }
            var thickness = Mathf.Lerp(thicknessMultiplier.x, thicknessMultiplier.y, t); //_mainCamera.orthographicSize * thicknessMultiplier.x + thicknessMultiplier.y;

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
