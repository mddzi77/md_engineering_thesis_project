using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace MdUtils.Attributes.DisableEdit
{

    [CustomPropertyDrawer(typeof(DisableEdit))]
    public class DisableEditDrawer : PropertyDrawer 
    {


        /// <summary>
        /// Display attribute and his value in inspector depending on the type
        /// Fill attribute needed
        /// </summary>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            
            return root;
        }
        
        
    }
}