using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uinavigation;

namespace example.uinavigation
{
    public class BasicExampleController : MonoBehaviour
    {
        private List<Button> _childrenButtons = new List<Button>();

        private void Start()
        {
            int idx = 0;
            _childrenButtons.AddRange(this.GetComponentsInChildren<Button>());

            foreach(var button in _childrenButtons)
            {
                var currentNavigator = UIContextManager.Instance.Navigators[idx];
                button.onClick.AddListener(() => OnClickTab(currentNavigator));
                idx++;
                if (idx >= UIContextManager.Instance.Navigators.Count)
                    break;
            }

            UIContextManager.Instance.Navigators.ForEach(item => item.gameObject.SetActive(false));
        }

        private void OnClickTab(UINavigation navigator)
        {
            navigator.gameObject.SetActive(true);
            UIContextManager.Instance.Navigators.Where(x => x != navigator)
                .ToList().ForEach(item =>
                {
                    item.gameObject.SetActive(false);
                });
        }
    }
}
