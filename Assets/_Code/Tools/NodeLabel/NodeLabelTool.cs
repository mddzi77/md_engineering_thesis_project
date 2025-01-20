using System.Collections.Generic;
using MouseGridPosition;
using TheLayers;
using TheLayers.Cells;
using TriInspector;
using UI;
using UI.Bottom;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools.NodeLabel
{
    public class NodeLabelTool: ToolAbstract
    {
        [SerializeField] private NodeLabelType nodeLabelType;
        [SerializeField] private LayerConfig labelLayer;
        [SerializeField] private InputActionReference leftMouse;
        [SerializeField] private List<LayerConfig> metalLayers;
        [AssetsOnly]
        [SerializeField] private GameObject dynamicLabelPrefab;
        
        private NodeLabel _nodeLabel;
        private Vector2 _gridPos;
        private LayersManager _layerManager;
        private LayerHolder _layerHolder;

        private void Start()
        {
            _layerManager = LayersManager.Instance;
            _layerHolder = _layerManager.LayerHolders[labelLayer];
        }

        private void Update()
        {
            if (leftMouse.action.IsPressed())
                OnPressed();
        }

        private void OnPressed()
        {
            _gridPos = MouseGrid.GridPos;
            if (PointerOnUI.Instance) return;
            DrawLabel(_gridPos.x, _gridPos.y);
        }
        
        private void DrawLabel(float posX, float posY)
        {
            var start = new Vector3(posX + 0.5f, posY + 0.5f, 0.5f);
            
            if (CheckForMetal(start))
            {
                if (_nodeLabel == null) CreateLabel();
                if (!_nodeLabel.gameObject.activeInHierarchy)
                {
                    _nodeLabel.gameObject.SetActive(true);
                    SetDynamicLabel();
                }

                var position = _nodeLabel.transform.position;
                position.x = posX;
                position.y = posY;
                _nodeLabel.transform.position = position;
            }
            else
            {
                InfoPanel.Instance.SetErrorText("Node label should be put on metal layer", 1f);
            }
        }

        private void CreateLabel()
        {
            var label = CellsPool.GetCell(labelLayer);
            label.name = nodeLabelType.ToString();
            _nodeLabel = label.AddComponent<NodeLabel>();
            _nodeLabel.SetType(nodeLabelType);
            _layerHolder.NewCell(label);
            
            SetDynamicLabel();
        }
        
        private void SetDynamicLabel()
        {
            var dynamicLabel = Instantiate(dynamicLabelPrefab, DynamicCanvas.Instance.transform);
            dynamicLabel.GetComponent<DynamicCanvasElement>().SetWorldSpaceElement(_nodeLabel.transform);
        }

        private bool CheckForMetal(Vector3 start)
        {
            if (Physics.Raycast(start, Vector3.forward, out var hit, 20f))
            {
                var hitTag = hit.transform.tag;
                foreach (var layer in metalLayers)
                {
                    if (string.Equals(hitTag, layer.LayerName)) return true;
                }
            }

            return false;
        }
    }
}