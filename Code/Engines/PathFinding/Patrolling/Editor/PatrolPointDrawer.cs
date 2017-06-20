using UnityEngine;
using UnityEditor;
namespace ModularFramework.AI {

    /// <summary>
    /// Define how to display in editor a PatrolPoint.
    /// </summary>
    [CustomPropertyDrawer(typeof(PatrolPoint))]
    public class PatrolPointDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

            EditorGUI.BeginProperty(new Rect(new Vector2(position.x, position.y), new Vector2(100, 100)), label, property);

            // Label
            Rect contentPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            // Indent
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            // Rect Positions
            var OrderRect = new Rect(contentPosition.x, contentPosition.y, 50, contentPosition.height);
            var patrolPointsRect = new Rect(contentPosition.x + 50, contentPosition.y, contentPosition.width, contentPosition.height);
            
            // Properties
            EditorGUI.PropertyField(OrderRect, property.FindPropertyRelative("Order"), new GUIContent() { tooltip = "Order" });
            EditorGUI.PropertyField(patrolPointsRect, property.FindPropertyRelative("Position"), GUIContent.none);

            EditorGUI.EndProperty();

        }
    }
}