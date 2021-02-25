using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class PanelsController : Singleton<PanelsController>
    {
        [SerializeField]
        private GameObject[] _panels;

        public void PanelsDeactivation(GameObject excludedPanel)
        {
            foreach (var panel in _panels)
            {
                if(panel == null) continue;

                if(panel == excludedPanel) continue;

                if (panel.activeSelf)
                {
                    panel.SetActive(false);
                }
            }
        }
    }
}
