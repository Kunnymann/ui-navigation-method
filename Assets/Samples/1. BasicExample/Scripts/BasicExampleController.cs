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
        [SerializeField] private List<BasicExampleContainer> _containers; 

        private void Awake()
        {
            foreach(var container in _containers)
                container.TabButton.onClick.AddListener(() => OnClickTab(container.Manager));
        }

        private void Start()
        {
            foreach (var container in _containers)
                container.Manager.Active = false;

        }

        private void OnClickTab(BasicExampleManager manager)
        {
            manager.Active = true;
            _containers.Select(controller => controller.Manager)
                .Where(item => item != manager)
                .ToList()
                .ForEach(m => m.Active = false);
        }
    }
}
