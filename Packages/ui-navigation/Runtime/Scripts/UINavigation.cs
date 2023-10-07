using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using uinavigation.uiview;

namespace uinavigation
{
    /// <summary>
    /// UIView를 관리하는 Navigation 클래스
    /// </summary>
    public partial class UINavigation : MonoBehaviour
    {
        [SerializeField] private List<UIView> _uiViewContainer = new List<UIView>();

        /// <summary>
        /// UIView Container
        /// </summary>
        public List<UIView> UiViewContainer => _uiViewContainer;

        /// <summary>
        /// UIView 전환 애니메이션을 관리하기 위한 Queue
        /// </summary>
        private Queue<(UIView hide, UIView show)> _uiViewTransQueue;

        /// <summary>
        /// UIView History를 관리하기 위한 Stack
        /// </summary>
        private Stack<UIView> _views;

        private UIView _currentView;

        /// <summary>
        /// 현재 활성화된 UIView
        /// </summary>
        public UIView CurrentView => _currentView;

        private void Awake()
        {
            UIContextManager.Initialize().Navigators.Add(this);

            _views = new Stack<UIView>();
            _uiViewTransQueue = new Queue<(UIView hide, UIView show)>();
            Observable.EveryUpdate().Where(x => _uiViewTransQueue.Count > 0)
                .Subscribe(y =>
                {
                    if (_currentView == null || _currentView.VisibleState == VisibleState.Appeared)
                    {
                        var dequeuedItem = _uiViewTransQueue.Dequeue();
                        TransUIView(dequeuedItem.hide, dequeuedItem.show);
                    }
                });
        }

        /// <summary>
        /// Hide 대상, Show 대상의 UIView를 전환하는 함수
        /// </summary>
        /// <param name="hide">Hide target</param>
        /// <param name="show">Show target</param>
        private async void TransUIView(UIView hide, UIView show)
        {
            if (hide != null) await hide.Hide();
            _currentView = show;
            if (show != null) await show.Show();

        }

        /// <summary>
        /// UIView를 Push합니다.
        /// </summary>
        /// <param name="viewName">이름</param>
        /// <returns>UIView</returns>
        public UIView PushUIView(string viewName)
        {
            UIView show = _uiViewContainer.Find(x => x.name == viewName);

            if (show == null)
            {
                Debug.LogWarning($"{viewName}의 UIView를 찾을 수 없습니다.");
                return null;
            }
            _views.TryPeek(out UIView hide);
            _views.Push(show);
            _uiViewTransQueue.Enqueue((hide, show));
            return show;
        }

        /// <summary>
        /// UIView를 비동기로 Push합니다.
        /// </summary>
        /// <param name="viewName">이름</param>
        /// <returns>UIView</returns>
        public async Task<UIView> PushUIViewAsync(string viewName)
        {
            UIView show = _uiViewContainer.Find(x => x.name == viewName);
            if (show == null)
            {
                Debug.LogWarning($"{viewName}의 UIView를 찾을 수 없습니다.");
                return null;
            }

            _views.Push(show);
            await show.Show();
            _currentView = show;
            return show;
        }

        /// <summary>
        /// UIView를 Pop합니다.
        /// </summary>
        /// <returns>UIView</returns>
        public UIView PopUIView()
        {
            if (_views == null || _views.Count == 0)
            {
                Debug.LogWarning("UIView가 더이상 존재하지 않습니다.");
                return null;
            }
            _views.TryPop(out UIView hide);
            _views.TryPeek(out UIView show);
            _uiViewTransQueue.Enqueue((hide, show));
            return hide;
        }

        /// <summary>
        /// UIView를 비동기로 Pop합니다.
        /// </summary>
        /// <returns>UIView</returns>
        public async Task<UIView> PopUIViewAsync()
        {
            if (_views == null || _views.Count == 0)
            {
                Debug.LogWarning("UIView가 더이상 존재하지 않습니다.");
                return null;
            }
            _views.TryPop(out UIView hide);
            await hide.Hide();
            _currentView = null;
            return hide;
        }

        /// <summary>
        /// 특정 UIView 직전까지, 모든 UIView를 Pop합니다.
        /// </summary>
        /// <param name="viewName">이름</param>
        /// <returns>UIView</returns>
        public UIView PopToUIView(string viewName)
        {
            if (_views == null || _views.Count == 0)
            {
                Debug.LogWarning("UIView가 더이상 존재하지 않습니다.");
                return null;
            }

            if (!_views.Any(view => view.name == viewName))
            {
                Debug.LogWarning($"{viewName}의 UIView를 찾을 수 없습니다.");
                return null;
            }

            if (_views.Last().name == viewName)
            {
                Debug.LogWarning($"{viewName}의 UIView는 가장 최근에 Push한 UIView입니다.");
                return null;
            }

            _views.TryPop(out UIView hide);
            while (_views.Count > 0)
            {
                _views.TryPeek(out UIView show);
                if (show.name == viewName)
                {
                    _uiViewTransQueue.Enqueue((hide, show));
                    break;
                }
                _views.Pop();
            }
            return hide;
        }

        /// <summary>
        /// 가장 마지막에 Push한 UIView를 제외하고, 모두 Pop합니다.
        /// </summary>
        /// <returns>UIView</returns>
        public UIView PopToRoot()
        {
            if (_views == null || _views.Count == 0)
            {
                Debug.LogWarning("UIView가 더이상 존재하지 않습니다.");
                return null;
            }

            if (_views.Count < 2)
            {
                Debug.LogWarning("UIView 개수가 2개 이상이어야 합니다.");
                return null;
            }

            _views.TryPop(out UIView hide);
            while (_views.Count > 1)
            {
                _views.Pop();
            }
            _views.TryPeek(out UIView show);
            _uiViewTransQueue.Enqueue((hide, show));
            return hide;
        }

        public void CollectViews()
        {
            _uiViewContainer.AddRange(this.GetComponentsInChildren<UIView>());
        }
    }
}