using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace uinavigation.editor
{
    [CustomEditor(typeof(UINavigation))]
    public class UINavigationEidtor : Editor
    {
        private SerializedProperty _viewContainer;
        private ReorderableList _viewList;

        private void OnEnable()
        {
            _viewContainer = serializedObject.FindProperty("_uiViewContainer");
            _viewList = new ReorderableList(serializedObject, _viewContainer, false, true, true, true);
            _viewList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "UIView Container");
            _viewList.drawElementCallback = (rect, index, isActive, isFocused) => {
                var element = _viewContainer.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element, GUIContent.none);
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            {
                _viewList.DoLayoutList();
                if (GUILayout.Button("Collect views in children"))
                {
                    OnCollectChildrenViews();
                }
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUILayout.EndVertical();
        }

        private void OnCollectChildrenViews()
        {
            (target as UINavigation).CollectChildrenViews();
        }
    }
}