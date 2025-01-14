using MdUtils;
using TheLayers;
using UnityEngine;

namespace Game.Checker
{
    public class CheckerManager : MonoSingleton<CheckerManager>
    {
        [SerializeField] private DesignRuleChecker designRuleChecker;
        [SerializeField] private TopographyValidator topographyValidator;
        
        private LayersManager _layersManager;

        private void Start()
        {
            _layersManager = LayersManager.Instance;
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
        
        [ContextMenu("Check All")]
        public void CheckAll()
        {
            CheckDesignRules();
            CheckTopography();
        }
        
        [ContextMenu("Restore")]
        public void Restore()
        {
            topographyValidator.Restore();
        }
    }
}