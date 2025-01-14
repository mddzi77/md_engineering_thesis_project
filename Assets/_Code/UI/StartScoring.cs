using Game.Checker;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartScoring : MonoBehaviour
    {
        [SerializeField] private Button startScoringButton;
        
        private void Start()
        {
            startScoringButton.onClick.AddListener(Score);
        }

        private void Score()
        {
            CheckerManager.Instance.CheckTopography();
        }
    }
}