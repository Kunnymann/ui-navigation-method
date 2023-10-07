using System;
using UnityEditor;
using UnityEngine;
using uinavigation.uiview;

namespace uinavigation.editor
{
    [CustomEditor(typeof(UINavigation))]
    public class UINavigationEidtor : Editor
    {
        SerializedProperty viewContainer;

        private void Awake()
        {
            viewContainer = serializedObject.FindProperty("_uiViewContainer");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.PropertyField(viewContainer);
                if (GUILayout.Button("Collect views in children"))
                {
                    OnCollectChildrenViews();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void OnCollectChildrenViews()
        {
            (target as UINavigation).CollectViews();
        }
    }
}