using TheLayers;
using UnityEngine;

namespace Game.Checker
{
    public class CheckerManager : MonoBehaviour
    {
        [SerializeField] private BoxCollider detector;
        [SerializeField] private DesignRuleChecker designRuleChecker;
        
        private LayersManager _layersManager;

        private void Start()
        {
            _layersManager = LayersManager.Instance;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            
        }
        
        [ContextMenu("Check Design Rules")]
        public void CheckDesignRules()
        {
            designRuleChecker.StartCheck();
        }
    }
}