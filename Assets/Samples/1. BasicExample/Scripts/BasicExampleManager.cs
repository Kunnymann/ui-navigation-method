using Cysharp.Threading.Tasks;
using UnityEngine;
using uinavigation;
using uinavigation.popup;

namespace example.uinavigation
{
    [RequireComponent(typeof(UINavigation))]
    public class BasicExampleManager : MonoBehaviour
    {
        private bool _init;
        private bool _active = true;

        public bool Active
        {
            set
            {
                if (_active != value)
                {
                    SetActive(value);
                    if (value && !_init)
                    {
                        _init = value;
                        OnInitialized();
                    }
                    _active = value;
                }
            }
        }

        private UINavigation _uiNavigation;

        private void Awake()
        {
            if (_uiNavigation == null)
                _uiNavigation = this.GetComponent<UINavigation>();
        }

        private void OnInitialized()
        {
            _uiNavigation.PushUIView("MainView");
        }

        private void Update()
        {
            if (_active && Input.GetKeyUp(KeyCode.Backspace))
                _uiNavigation.PopUIView();
        }

        private void SetActive(bool state = true)
        {
            var navigatorCanvas = this._uiNavigation.GetComponent<CanvasGroup>();

            navigatorCanvas.alpha = state ? 1 : 0;
            navigatorCanvas.blocksRaycasts = state;
            navigatorCanvas.interactable = state;
        }

        public void OnClickNextButton(GameObject guideViewGameObject)
        {
            int curGameObjectIndex = guideViewGameObject.transform.GetSiblingIndex();
            int nextGameObjectIndex = curGameObjectIndex + 1;

            if (nextGameObjectIndex < guideViewGameObject.transform.parent.childCount)
            {
                var popUp = UIPopup.GetUIPopup("DoubleButtonPopup");

                popUp.SetDependencyOnView(_uiNavigation.CurrentView)
                    .SetButtonEvent(async () =>
                    {
                        await popUp.Hide();
                        _uiNavigation.PushUIView(guideViewGameObject.transform.parent.GetChild(nextGameObjectIndex).name);
                    })
                    .Show().Forget();
            }
            else
            {
                Debug.LogWarning("this is the last one.");
            }
        }
    }

}