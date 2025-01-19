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
        
        private void Start()
        {
            _currentLevelIndex = PlayerPrefs.GetInt("current_level", 0);
            _unlockedLevelIndex = PlayerPrefs.GetInt("unlocked_level", 0);
            checkManager.SetLevel(levels[_currentLevelIndex]);
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.Save();
        }

        public void StartCheck()
        {
            checkManager.SetLevel(levels[_currentLevelIndex]);
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