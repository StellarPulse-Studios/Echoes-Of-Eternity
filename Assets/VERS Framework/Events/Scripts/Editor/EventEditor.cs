using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VERS
{
    [CustomEditor(typeof(Event))]
    public class EventEditor : Editor
    {
        private FieldInfo m_Listeners;

        private void OnEnable()
        {
            Type type = typeof(Event);
            m_Listeners = type.GetField("m_Listeners", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Event e = (Event)target;

            EditorGUILayout.Space();

            if (GUILayout.Button("Raise Event", GUILayout.Height(30.0f)))
            {
                e.Raise();
            }

            EditorGUILayout.Space();

            List<EventListener> listeners = m_Listeners.GetValue(e) as List<EventListener>;

            EditorGUILayout.LabelField($"Registered Listeners (Count : {listeners.Count})", EditorStyles.boldLabel);

            for (int i = 0; i < listeners.Count; i++)
            {
                EditorGUILayout.LabelField(listeners[i].gameObject.name);
            }
        }
    }
}
