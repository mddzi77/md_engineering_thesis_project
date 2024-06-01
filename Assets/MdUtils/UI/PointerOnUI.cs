using System.Collections;
using System.Collections.Generic;
using MdUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PointerOnUI : Singleton<PointerOnUI>
{
    private GraphicRaycaster[] raycasters;
    private Vector2 pointerPosition;
    private PointerEventData pointerEventData;
    private List<RaycastResult> rayCastUIResults = new();
    private bool _value { 
        get {
            pointerPosition = Pointer.current.position.ReadValue();
            pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = pointerPosition;
            rayCastUIResults.Clear();
            foreach (GraphicRaycaster raycaster in raycasters) { 
                raycaster.Raycast(pointerEventData, rayCastUIResults);
                if (rayCastUIResults.Count > 0)
                    break;
            }
            return rayCastUIResults.Count > 0;
        }
    }

    private new void Awake()
    {
        base.Awake();
        raycasters = FindObjectsOfType<GraphicRaycaster>();
    }
    
    public static implicit operator bool(PointerOnUI pointerOnUI)
    {
        return pointerOnUI._value;
    }
}
