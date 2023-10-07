using Cysharp.Threading.Tasks;
using UnityEngine;
using uinavigation;
using uinavigation.popup;

namespace example
{
    [RequireComponent(typeof(UINavigation))]
    public class BasicExampleManager : MonoBehaviour
    {
        private UINavigation _uiNavigation;
        public UINavigation UiNavigation
        {
            get
            {
                if (_uiNavigation == null)
                {
                    _uiNavigation = this.GetComponent<UINavigation>();
                }
                return _uiNavigation;
            }
        }

        private static BasicExampleManager _instance;
        public static BasicExampleManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<BasicExampleManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_uiNavigation == null)
                _uiNavigation = this.GetComponent<UINavigation>();
        }

        private void Start()
        {
            _uiNavigation.PushUIView("MainView");
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Backspace))
                _uiNavigation.PopUIView();
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
                Debug.LogWarning("마지막 가이드 문서입니다.");
            }
        }
    }

}