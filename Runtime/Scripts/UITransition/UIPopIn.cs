using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace uinavigation
{
    public class UIPopIn : UITransitionBase
    {
        private Tween tween;

        private RectTransform _rectTransform;

        protected override void Initialize()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override UniTask HideAnim(float duration)
        {
            tween = _rectTransform.DOScale(0, duration).SetAutoKill();
            return tween.AsyncWaitForCompletion().AsUniTask();
        }

        protected override void HideWithoutAnim()
        {
            _rectTransform.localScale = Vector3.zero;
        }

        protected override void KillAnim()
        {
            if (tween.active) tween.Kill(true);
        }

        protected override UniTask ShowAnim(float duration)
        {
            tween = _rectTransform.DOScale(1, duration).SetAutoKill();
            return tween.AsyncWaitForCompletion().AsUniTask();
        }

        protected override void ShowWithoutAnim()
        {
            _rectTransform.localScale = Vector3.one;
        }
    }
}