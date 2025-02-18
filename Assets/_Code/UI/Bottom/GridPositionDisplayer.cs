﻿using System.Text;
using MouseGridPosition;
using TMPro;
using UnityEngine;

namespace UI.Bottom
{
    public class GridPositionDisplayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI xText;
        [SerializeField] private TextMeshProUGUI yText;

		private void FixedUpdate()
        {
            if (PointerOnUI.Instance) return;
            var x = MouseGrid.GridPos.x;
            var y = MouseGrid.GridPos.y;
            var xString = x < 0 ? $"-{Mathf.Abs(x)}" : $"+{x+1}";
            var yString = y < 0 ? $"-{Mathf.Abs(y)}" : $"+{y+1}";
            xText.text = xString;
            yText.text = yString;
        }
    }
}