using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using uinavigation;
using Cysharp.Threading.Tasks;
using System.Collections.Concurrent;
using System;

namespace example.uinavigation
{
    public class AsyncExampleJob : MonoBehaviour
    {
        [SerializeField] private UINavigation _targetNavigation;
        [SerializeField] private Slider _progressBar;

        private static AsyncExampleJob _instance;
        public static AsyncExampleJob Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<AsyncExampleJob>();
                return _instance;
            }
        }

        private ConcurrentQueue<Action> _unityMainThreadQueue = new ConcurrentQueue<Action>();
        private float _jobStackedValue = 0;

        private bool _jobPaused = false;
        public bool JobPaused
        {
            get => _jobPaused;
            set => _jobPaused = value;
        }

        private bool _jobStarted = false;
        public bool JobStarted
        {
            get => _jobStarted;
            set => _jobStarted = value;
        }

        public void InitJob()
        {
            _jobStackedValue = 0;
        }

        private void Start()
        {
            Task.Run(DoSomeJob);
        }

        private void Update()
        {
            if (_unityMainThreadQueue.Count > 0)
            {
                _unityMainThreadQueue.TryDequeue(out Action action);
                action.Invoke();
            }
        }

        /// <summary>
        /// 어떠한 복잡한 일을 비동기적으로 처리하면서 동시에 UIView 처리를 수행하는 샘플 코드
        /// </summary>
        private async void DoSomeJob()
        {
            await UniTask.WaitUntil(() => _jobStarted);

            while (true)
            {
                if (_jobPaused)
                {
                    await UniTask.WaitForFixedUpdate();
                    continue;
                }

                _jobStackedValue = _jobStackedValue + 0.005f;
                _unityMainThreadQueue.Enqueue(() => UpdateProgressBar(_jobStackedValue));

                if (_jobStackedValue >= 1f)
                {
                    var view = _targetNavigation.CurrentView;
                    var targetView = (view as AsyncExampleView).TargetUIView;
                    if (targetView != null)
                        await _targetNavigation.PushUIViewAsync(targetView.gameObject.name);
                    else
                        break;
                    _jobStackedValue = 0;
                }

                await UniTask.WaitForFixedUpdate();
            }
            Debug.Log("Finish all jobs");
        }

        private void UpdateProgressBar(float amount)
        {
            _progressBar.value = amount;
        }
    }
}