using System.Collections;
using System.Collections.Generic;
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

        private ConcurrentQueue<Action> _unityMainThreadQueue = new ConcurrentQueue<Action>();
        private float _jobStackedValue = 0;

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

        private async void DoSomeJob()
        {
            while (true)
            {
                _jobStackedValue = _jobStackedValue + 0.01f;
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