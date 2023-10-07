using DG.Tweening;
using UnityEngine;
using uinavigation.uiview;
using System.Collections.Generic;
using System;

namespace uinavigation
{
    /// <summary>
    /// UI 라이프 사이클을 관리하는 클래스
    /// </summary>
    public class UIContextManager : MonoBehaviour
    {
        private static UIContextManager _instance;
        public static UIContextManager Instance
        {
            get
            {
                if (_instance == null)
                    throw new Exception("UIContextManager가 초기화되지 않았습니다.");
                return _instance;
            }
        }

        private List<UINavigation> _navigators = new List<UINavigation>();
        public List<UINavigation> Navigators => _navigators;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns>UIContextManager</returns>
        public static UIContextManager Initialize()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIContextManager>();
                if (_instance == null)
                    _instance = new GameObject(UINavigationConst.UIContextManager_GameObject).AddComponent<UIContextManager>();
            }
            return _instance;
        }

        /// <summary>
        /// Destroy시, DOTween, UINavigation, UIViewContainer를 Dispose합니다.
        /// </summary>
        private void OnDestroy()
        {
            DOTween.Clear();

            // 현재 가지고 있는 모든 UINavigator를 Dispose 처리
            _navigators.Clear();
        }
    }
}
