using System;
using System.Collections.Generic;
using Game.Checker;
using MdUtils;
using TriInspector;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private CheckManager checkManager;
        [SerializeField] private List<LevelData> levels;
        
        [ShowInInspector]
        public int CurrentLevel=> _currentLevelIndex + 1;
        public LevelData CurrentLevelData => levels[_currentLevelIndex];
        public int TotalLevels => levels.Count;
        public int UnlockedLevel => _unlockedLevelIndex + 1;
        public List<LevelData> Levels => levels;
        
        private int _currentLevelIndex;
        private int _unlockedLevelIndex;
        
        private new void Awake()
        {
            base.Awake();
            _currentLevelIndex = PlayerPrefs.GetInt("current_level", 0);
            _unlockedLevelIndex = PlayerPrefs.GetInt("unlocked_level", 0);
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.Save();
        }

        public void StartCheck()
        {
            checkManager.SetLevel(levels[_currentLevelIndex]);
            checkManager.CheckTopography();
        }
        
        [ContextMenu("Next Level")]
        public void NextLevel()
        {
            if (_currentLevelIndex >= levels.Count)
            {
                Debug.Log("All levels completed!");
                return;
            }

            if (_currentLevelIndex == _unlockedLevelIndex)
            {
                Debug.Log("Level is locked!");
                return;
            }
            _currentLevelIndex++;
            PlayerPrefs.SetInt("current_level", _currentLevelIndex);
        }
        
        [ContextMenu("Unlock Next Level")]
        public void UnlockNextLevel()
        {
            if (_currentLevelIndex != _unlockedLevelIndex)  return;
            // if (_unlockedLevelIndex >= levels.Count - 1)
            // {
            //     Debug.Log("All levels are unlocked!");
            //     return;
            // }
            _unlockedLevelIndex++;
            PlayerPrefs.SetInt("unlocked_level", _unlockedLevelIndex);
        }
        
        public void PreviousLevel()
        {
            if (_currentLevelIndex < 0)
            {
                Debug.Log("This is the first level!");
                return;
            }
            _currentLevelIndex--;
            PlayerPrefs.SetInt("current_level", _currentLevelIndex);
        }
        
        public void Reset()
        {
            _currentLevelIndex = 0;
            PlayerPrefs.SetInt("current_level", _currentLevelIndex);
            checkManager.SetLevel(levels[_currentLevelIndex]);
        }
    }
}