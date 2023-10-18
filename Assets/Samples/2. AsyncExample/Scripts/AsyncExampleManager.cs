using UnityEngine;
using uinavigation;
using System.Linq;

namespace example.uinavigation
{
    [RequireComponent(typeof(UINavigation))]
    public class AsyncExampleManager : MonoBehaviour
    {
        private UINavigation _uiNavigation;

        private void Awake()
        {
            _uiNavigation = GetComponent<UINavigation>();
        }

        private void Start()
        {
            SetChildTargetUIView();
            _uiNavigation.PushUIView("MainView");
        }

        private void SetChildTargetUIView()
        {
            for (int idx = 0; idx < _uiNavigation.UiViewContainer.Count - 1; idx++)
                (_uiNavigation.UiViewContainer[idx] as AsyncExampleView).TargetUIView = (_uiNavigation.UiViewContainer[idx + 1] as AsyncExampleView);
        }
    }
}

