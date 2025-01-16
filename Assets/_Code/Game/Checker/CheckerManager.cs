using System.Collections.Generic;
using Game.Transistors;
using MdUtils;
using TheLayers;
using UI;
using UnityEngine;

namespace Game.Checker
{
    public class CheckerManager : MonoSingleton<CheckerManager>
    {
        [SerializeField] private Level levelFile;
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

            foreach (var levelData in levelFile.LevelDatas)
            {
                if (levelData.type == ComponentType.PTransistor)
                {
                    for (int i = 0; i < pTransistors.Count; i++)
                    {
                        var transistor = pTransistors[i];
                        if (
                            ((levelData.pin1.Equals(transistor.Pin1.ID) && levelData.pin2.Equals(transistor.Pin2.ID)) ||
                             (levelData.pin1.Equals(transistor.Pin2.ID) &&
                              levelData.pin2.Equals(transistor.Pin1.ID))) &&
                            levelData.W == (int)transistor.Width && levelData.L == (int)transistor.Length
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
                            ((levelData.pin1.Equals(transistor.Pin1.ID) && levelData.pin2.Equals(transistor.Pin2.ID)) ||
                             (levelData.pin1.Equals(transistor.Pin2.ID) &&
                              levelData.pin2.Equals(transistor.Pin1.ID))) &&
                            levelData.W == (int)transistor.Width && levelData.L == (int)transistor.Length
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