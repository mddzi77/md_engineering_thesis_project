using System.Collections.Generic;
using Game.Transistors;
using MdUtils;
using TheLayers;
using UI;
using UnityEngine;

namespace Game.Checker
{
    public class CheckManager : MonoBehaviour
    {
        [SerializeField] private DesignRuleChecker designRuleChecker;
        [SerializeField] private TopographyValidator topographyValidator;
        
        private LevelData _levelDataFile;
        private LayersManager _layersManager;

        private void Start()
        {
            _layersManager = LayersManager.Instance;
        }
        
        public void SetLevel(LevelData levelData)
        {
            _levelDataFile = levelData;
        }
        
        [ContextMenu("Check Design Rules")]
        public void CheckDesignRules()
        {
            designRuleChecker.StartCheck();
        }
        
        [ContextMenu("Check Topography")]
        public void CheckTopography()
        {
            topographyValidator.ValidationCompleted += OnValidationCompleted;
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
        
        private void OnValidationCompleted(List<NTransistor> nTransistors, List<PTransistor> pTransistors)
        {
            topographyValidator.ValidationCompleted -= OnValidationCompleted;

            if (nTransistors.Count == 0 && pTransistors.Count == 0)
            {
                Restore();
                return;
            }

            foreach (var component in _levelDataFile.Components)
            {
                if (component.type == ComponentType.PTransistor)
                {
                    for (int i = 0; i < pTransistors.Count; i++)
                    {
                        var transistor = pTransistors[i];
                        if (
                            ((component.pin1.Equals(transistor.Pin1.ID) && component.pin2.Equals(transistor.Pin2.ID)) ||
                             (component.pin1.Equals(transistor.Pin2.ID) &&
                              component.pin2.Equals(transistor.Pin1.ID))) &&
                            component.W == (int)transistor.Width && component.L == (int)transistor.Length
                        )
                        {
                            pTransistors.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < nTransistors.Count; i++)
                    {
                        var transistor = nTransistors[i];
                        if (
                            ((component.pin1.Equals(transistor.Pin1.ID) && component.pin2.Equals(transistor.Pin2.ID)) ||
                             (component.pin1.Equals(transistor.Pin2.ID) &&
                              component.pin2.Equals(transistor.Pin1.ID))) &&
                            component.W == (int)transistor.Width && component.L == (int)transistor.Length
                        )
                        {
                            nTransistors.RemoveAt(i);
                        }
                    }
                }
            }
            
            if (nTransistors.Count == 0 && pTransistors.Count == 0)
            {
                ScoreWindow.Instance.SetScoreText("All components are placed correctly!");
            }
            else
            {
                ScoreWindow.Instance.SetScoreText("Some components are not placed correctly!");
            }
            
            Restore();
        }
    }
}