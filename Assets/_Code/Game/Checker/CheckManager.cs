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
            topographyValidator.ValidationStopped += OnValidationStopped;
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
        
        private void OnValidationStopped()
        {
            topographyValidator.ValidationStopped -= OnValidationStopped;
            topographyValidator.ValidationCompleted -= OnValidationCompleted;
            Restore();
            ScoreWindow.Instance.SetScoreText("There should both Vdd and Vss nodes!", false);
        }
        
        private void OnValidationCompleted(List<NTransistor> nTransistors, List<PTransistor> pTransistors)
        {
            topographyValidator.ValidationCompleted -= OnValidationCompleted;
            topographyValidator.ValidationStopped -= OnValidationStopped;
            var pTrans = new List<PTransistor>(pTransistors);
            var nTrans = new List<NTransistor>(nTransistors);

            if (nTrans.Count == 0 && pTrans.Count == 0)
            {
                Restore();
                ScoreWindow.Instance.SetScoreText("None transistors were found!", false);
                return;
            }

            foreach (var component in _levelDataFile.Components)
            {
                if (component.type == ComponentType.PTransistor)
                {
                    for (int i = 0; i < pTrans.Count; i++)
                    {
                        var transistor = pTrans[i];
                        if (
                            ((component.pin1.Equals(transistor.Pin1.ID) && component.pin2.Equals(transistor.Pin2.ID)) ||
                             (component.pin1.Equals(transistor.Pin2.ID) &&
                              component.pin2.Equals(transistor.Pin1.ID))) &&
                            component.W == (int)transistor.Width && component.L == (int)transistor.Length
                        )
                        {
                            pTrans.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < nTrans.Count; i++)
                    {
                        var transistor = nTrans[i];
                        if (
                            ((component.pin1.Equals(transistor.Pin1.ID) && component.pin2.Equals(transistor.Pin2.ID)) ||
                             (component.pin1.Equals(transistor.Pin2.ID) &&
                              component.pin2.Equals(transistor.Pin1.ID))) &&
                            component.W == (int)transistor.Width && component.L == (int)transistor.Length
                        )
                        {
                            nTrans.RemoveAt(i);
                        }
                    }
                }
            }
            
            var correct = nTrans.Count == 0 && pTrans.Count == 0;
            Restore();
            
            if (correct)
            {
                ScoreWindow.Instance.SetScoreText("All components are placed correctly!", true);
            }
            else
            {
                ScoreWindow.Instance.SetScoreText("Some components are not placed correctly!", false);
            }
        }
    }
}