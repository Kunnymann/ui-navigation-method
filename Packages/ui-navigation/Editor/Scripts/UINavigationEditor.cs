using System;
using UnityEditor;
using UnityEngine;
using uinavigation.uiview;

namespace uinavigation.editor
{
    [CustomEditor(typeof(UINavigation))]
    public class UINavigationEidtor : Editor
    {
        private SerializedProperty _viewContainer;

        private void Awake()
        {
            _viewContainer = serializedObject.FindProperty("_uiViewContainer");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.PropertyField(_viewContainer);
                if (GUILayout.Button("Collect views in children"))
                {
                    OnCollectChildrenViews();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void OnCollectChildrenViews()
        {
            (target as UINavigation).CollectChildrenViews();
        }
    }
}