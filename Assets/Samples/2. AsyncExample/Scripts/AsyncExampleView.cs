using UnityEngine;
using UnityEngine.UI;
using uinavigation.uiview;
using uinavigation;
using uinavigation.popup;
using Cysharp.Threading.Tasks;

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
            InitGUI();
        }

        private void InitGUI()
        {
            if (_nextButton != null)
                _nextButton.onClick.AddListener(() => OnClickNext());
            if (_backButton != null)
                _backButton.onClick.AddListener(() => UINavigation.PopUIView());
        }

        private void OnClickNext()
        {
            AsyncExampleJob.Instance.JobPaused = true;
            var uiPopup = UIPopup.GetUIPopup("DoubleButtonPopup");
            uiPopup.SetDependencyOnView(this).SetButtonEvent(async () =>
            {
                // Next Button
                await uiPopup.Hide();
                await UINavigation.PushUIViewAsync(_targetUIView.gameObject.name);
                AsyncExampleJob.Instance.JobPaused = false;
            }, async () =>
            {
                // Back Button
                await uiPopup.Hide();
                AsyncExampleJob.Instance.JobPaused = false;
            }).Show().Forget();
        }

        protected override void OnHiding()
        {
            base.OnHide();

            if (!AsyncExampleJob.Instance.JobPaused)
                AsyncExampleJob.Instance.JobPaused = true;
        }

        protected override void OnShow()
        {
            base.OnShowing();

            if (!AsyncExampleJob.Instance.JobStarted)
                AsyncExampleJob.Instance.JobStarted = true;

            AsyncExampleJob.Instance.InitJob();
            if (AsyncExampleJob.Instance.JobPaused)
                AsyncExampleJob.Instance.JobPaused = false;
        }
    }
}

