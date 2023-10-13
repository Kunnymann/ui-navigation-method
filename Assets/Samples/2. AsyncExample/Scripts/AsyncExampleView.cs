using UnityEngine;
using UnityEngine.UI;
using uinavigation.uiview;

namespace example.uinavigation
{
    public class AsyncExampleView : UIView
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _backButton;

        private UIView _targetUIView;
        public UIView TargetUIView
        {
            get => _targetUIView;
            set => _targetUIView = value;
        }

        protected override void Start()
        {
            base.Start();
            if (_nextButton != null)
                _nextButton.onClick.AddListener(async () => await UINavigation.PushUIViewAsync(_targetUIView.gameObject.name));
            if (_backButton != null)
                _backButton.onClick.AddListener(async () => await UINavigation.PopUIViewAsync());
        }
    }
}

