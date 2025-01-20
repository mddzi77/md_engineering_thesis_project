using System.Collections.Generic;
using Game;
using MainCamera;
using MdUtils;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Requirements
{
    public class RequirementsWindow : MonoSingleton<RequirementsWindow>
    {
        [SerializeField] private GameObject requirementsWindow;
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private Transform componentsParent;
        [SerializeField] private Button okButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;
        
        [AssetsOnly]
        [SerializeField] private GameObject componentPrefab;
        
        private List<ComponentView> _components = new();
        
        private void Start()
        {
            okButton.onClick.AddListener(CloseWindow);
            nextButton.onClick.AddListener(OnNext);
            prevButton.onClick.AddListener(OnPrev);
            OpenWindow();
        }
        
        public void OpenWindow()
        {
            requirementsWindow.SetActive(true);
            CameraMovement.Disable();
            SetupValues();
        }
        
        public void CloseWindow()
        {
            requirementsWindow.SetActive(false);
            CameraMovement.Enable();
        }

        private void SetupValues()
        {
            header.text = $"Level {GameManager.Instance.CurrentLevel}";
            
            var isPrev = GameManager.Instance.CurrentLevel > 1;
            prevButton.gameObject.SetActive(isPrev);
            
            var isNext = GameManager.Instance.CurrentLevel < GameManager.Instance.TotalLevels;
            var isNextLocked = GameManager.Instance.CurrentLevel >= GameManager.Instance.UnlockedLevel;
            nextButton.gameObject.SetActive(isNext);
            nextButton.interactable = !isNextLocked;
            
            ManageComponents();
        }

        private void ManageComponents()
        {
            var components = GameManager.Instance.CurrentLevelData.Components;
            if (components.Count > _components.Count)
            {
                for (var i = _components.Count; i < components.Count; i++)
                {
                    var component = Instantiate(componentPrefab, componentsParent).GetComponent<ComponentView>();
                    _components.Add(component);
                }
            }
            else if (components.Count < _components.Count)
            {
                for (var i = _components.Count - 1; i >= components.Count; i--)
                {
                    var comp = _components[i];
                    _components.RemoveAt(i);
                    Destroy(comp.gameObject);
                }
            }

            for(int i = 0; i < components.Count; i++)
            {
                var data = components[i];
                _components[i].SetValues(data);
            }
        }

        private void OnNext()
        {
            GameManager.Instance.NextLevel();
            SetupValues();
        }
        
        private void OnPrev()
        {
            GameManager.Instance.PreviousLevel();
            SetupValues();
        }
    }
}