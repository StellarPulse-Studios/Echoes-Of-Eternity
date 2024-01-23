using UnityEditor;
using UnityEngine;

namespace VERS
{
    [CustomPropertyDrawer(typeof(Reference), true)]
    public class ReferencePropertyDrawer : PropertyDrawer
    {
        private readonly string[] m_PopupOptions = { "Use Constant", "Use Variable" };

        private GUIStyle m_PopupStyle;
        private GUIStyle m_ButtonStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_PopupStyle == null)
            {
                m_PopupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                m_PopupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            if (m_ButtonStyle == null)
            {
                m_ButtonStyle = new GUIStyle(GUI.skin.button);
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            // Get properties
            SerializedProperty useConstant = property.FindPropertyRelative("m_UseConstant");
            SerializedProperty constantValue = property.FindPropertyRelative("m_ConstantValue");
            SerializedProperty variable = property.FindPropertyRelative("m_Variable");

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += m_PopupStyle.margin.top;
            buttonRect.width = m_PopupStyle.fixedWidth + m_PopupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            int result = EditorGUI.Popup(buttonRect, useConstant.boolValue ? 0 : 1, m_PopupOptions, m_PopupStyle);
            useConstant.boolValue = result == 0;

            EditorGUI.PropertyField(position, useConstant.boolValue ? constantValue : variable, GUIContent.none);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
