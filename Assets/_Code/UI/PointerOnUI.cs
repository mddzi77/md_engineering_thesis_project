using System.Collections;
using System.Collections.Generic;
using MdUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class PointerOnUI : MonoSingleton<PointerOnUI>
    {
        private GraphicRaycaster[] _raycasters;
        private Vector2 _pointerPosition;
        private PointerEventData _pointerEventData;
        private readonly List<RaycastResult> _rayCastUIResults = new();

        private bool _value
        {
            get
            {
                _pointerPosition = Pointer.current.position.ReadValue();
                _pointerEventData = new PointerEventData(EventSystem.current);
                _pointerEventData.position = _pointerPosition;
                _rayCastUIResults.Clear();
                foreach (GraphicRaycaster raycaster in _raycasters)
                {
                    raycaster.Raycast(_pointerEventData, _rayCastUIResults);
                    if (_rayCastUIResults.Count > 0)
                        break;
                }

                return _rayCastUIResults.Count > 0;
            }
        }

        private new void Awake()
        {
            base.Awake();
            _raycasters = FindObjectsByType<GraphicRaycaster>(FindObjectsSortMode.None);
        }

        public static implicit operator bool(PointerOnUI pointerOnUI)
        {
            return pointerOnUI._value;
        }
    }
}
