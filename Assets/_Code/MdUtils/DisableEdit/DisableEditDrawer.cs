using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MdUtils.DisableEdit
{
    [CustomPropertyDrawer(typeof(DisableEditAttribute))]
    public class DisableEditDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var field = new PropertyField(property);
            field.SetEnabled(false);
            return field;
        }
    }
}