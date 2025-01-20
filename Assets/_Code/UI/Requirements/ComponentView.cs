using Game;
using TMPro;
using UnityEngine;

namespace UI.Requirements
{
    public class ComponentView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI type;
        [SerializeField] private TextMeshProUGUI pin1;
        [SerializeField] private TextMeshProUGUI pin2;
        [SerializeField] private TextMeshProUGUI width;
        [SerializeField] private TextMeshProUGUI length;

        public void SetValues(ComponentData data)
        {
            type.text = data.type == ComponentType.NTransistor ? "nMOS" : "pMOS";
            pin1.text = data.pin1;
            pin2.text = data.pin2;
            width.text = data.W.ToString();
            length.text = data.L.ToString();
        }
    }
}