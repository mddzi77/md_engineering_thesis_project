using System.Collections;
using System.Collections.Generic;
using TheLayers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Drawer : MonoBehaviour
{
    [SerializeField] private InputActionReference mousePosition;
    [SerializeField] private InputActionReference leftClick;

    [SerializeField] private LayerConfig _layerConfig;
    
    //private LayerConfig _layerConfig;
    
    private void Awake()
    {
        leftClick.action.performed += OnLeftClick;
    }
    
    private void OnLeftClick(InputAction.CallbackContext ctx)
    {
        var mouseScreenPos = mousePosition.action.ReadValue<Vector2>();
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        var gridPos = CalculateGridPosition(mouseWorldPos);
        Draw(gridPos);
    }
    
    private Vector3 CalculateGridPosition(Vector3 mouseWorldPos)
    {
        var gridPos = new Vector3(
            Mathf.Floor(mouseWorldPos.x),
            Mathf.Floor(mouseWorldPos.y),
            _layerConfig.Order
        );
        return gridPos;
    }
    
    private void Draw(Vector3 gridPos)
    {
        var rec = new GameObject(_layerConfig.LayerName);
        rec.transform.position = gridPos;
        var spriteRenderer = rec.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = _layerConfig.Sprite;
    }
}
