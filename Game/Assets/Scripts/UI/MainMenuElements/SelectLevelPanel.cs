using System;
using System.Collections.Generic;
using Core.Extension;
using Services.FactoryServices;
using Services.SaveLoadServices;
using Services.SoundServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MainMenuElements
{
    public class SelectLevelPanel : MonoBehaviour
    {
        public event Action OnExit; 
        
        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private GameObject _panel;
        [SerializeField]
        private Button _exitBtn;
        
        private IUIFactory _uiFactory;
        private ISaveLoadService _saveLoadService;

        private List<SelectLevelBtn> _selectLevelBtnList;
        private SoundService _soundService;

        private readonly int _levelsAmount = 8;

        public void Init(IUIFactory uiFactory, ISaveLoadService saveLoadService, SoundService soundService)
        {
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
            _soundService = soundService;
        }

        private void Start()
        {
            _selectLevelBtnList = new List<SelectLevelBtn>();
            CreateButtons();
            _exitBtn.onClick.AddListener(Hide);
            _canvasGroup.SetActive(false);
        }

        public void Show() =>
            _canvasGroup.SetActive(true);

        private void Hide()
        {
            _soundService.PlayClickSfx();
            _canvasGroup.SetActive(false);
            OnExit?.Invoke();
        }

        private void CreateButtons()
        {
            for (int i = 0; i < _levelsAmount; i++)
            {
                SelectLevelBtn selectLevelBtn = _uiFactory.CreateSelectLevelButton(_panel.transform);
                selectLevelBtn.SetSoundService(_soundService);
                selectLevelBtn.SetLevelIndex(i + 1);
                
                if (i <= _saveLoadService.GetProgress().OpenLevels - 1) 
                    selectLevelBtn.SetEnableButton();
                
                selectLevelBtn.OnSelectLevel += LoadSelectedLevel;
                
                _selectLevelBtnList.Add(selectLevelBtn);
            }
        }

        private void LoadSelectedLevel(int index)
        {
            _saveLoadService.GetProgress().SaveCurrentLevel(index);
            _saveLoadService.Save();
            SceneManager.LoadScene(1);
        }

        private void OnDestroy()
        {
            _exitBtn.onClick.RemoveListener(Hide);

            foreach (var selectLevelBtn in _selectLevelBtnList) 
                selectLevelBtn.OnSelectLevel -= LoadSelectedLevel;
        }
    }
}