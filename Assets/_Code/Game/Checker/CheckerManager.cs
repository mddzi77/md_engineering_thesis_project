using TheLayers;
using UnityEngine;

namespace Game.Checker
{
    public class CheckerManager : MonoBehaviour
    {
        [SerializeField] private BoxCollider detector;
        [SerializeField] private DesignRuleChecker designRuleChecker;
        [SerializeField] private TopographyValidator topographyValidator;
        
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
        
        [ContextMenu("Check Topography")]
        public void CheckTopography()
        {
            topographyValidator.StartValidation().Forget();
        }
    }
}