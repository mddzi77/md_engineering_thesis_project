using TheLayers.Grid;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheLayers.Editor
{
    [CustomPropertyDrawer(typeof(Corners))]
    public class CornersEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            var topProperty = property.FindPropertyRelative("top");
            var rightProperty = property.FindPropertyRelative("right");
            var bottomProperty = property.FindPropertyRelative("bottom");
            var leftProperty = property.FindPropertyRelative("left");
            
            var topField = new PropertyField(topProperty);
            topField.label = "";
            topField.style.width = 60;
            
            var rightField = new PropertyField(rightProperty);
            rightField.label = "";
            rightField.style.marginLeft = 30;
            rightField.style.width = 60;
            
            var bottomField = new PropertyField(bottomProperty);
            bottomField.label = "";
            bottomField.style.width = 60;
            
            var leftField = new PropertyField(leftProperty);
            leftField.label = "";
            leftField.style.marginRight = 30;
            leftField.style.width = 60;
            
            var topContainer = new VisualElement();
            var middleContainer = new VisualElement();
            var bottomContainer = new VisualElement();
            
            topContainer.Add(topField);
            topContainer.style.alignItems = Align.Center;
            
            middleContainer.Add(leftField);
            middleContainer.Add(rightField);
            middleContainer.style.flexDirection = FlexDirection.Row;
            middleContainer.style.justifyContent = Justify.Center;
            
            bottomContainer.Add(bottomField);
            bottomContainer.style.alignItems = Align.Center;
            
            var box = new Box();
            box.style.flexDirection = FlexDirection.Row;
            
            var cornersContainer = new VisualElement();
            cornersContainer.style.display = DisplayStyle.Flex;
            cornersContainer.style.flexGrow = 1;
            
            var label = new Label("Corners");
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.fontSize = 25;
            label.style.display = DisplayStyle.Flex;
            label.style.flexGrow = 1;
            
            cornersContainer.Add(topContainer);
            cornersContainer.Add(middleContainer);
            cornersContainer.Add(bottomContainer);
            
            box.Add(label);
            box.Add(cornersContainer);
            
            root.Add(box);
            
            return root;
        }
    }
}