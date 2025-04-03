using System;
using Services.SoundServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI.MainMenuElements
{
    public class SelectLevelBtn : MonoBehaviour
    {
        public event Action<int> OnSelectLevel;
        
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        private Button _button;
        
        private SoundService _soundService;
        
        private int _levelIndex;

        private void Start()
        {
            _button = GetComponent<Button>();

            if (YandexGame.EnvironmentData.language == "en")
                _text.SetText("Level " + _levelIndex);
            else
                _text.SetText("Уровень " + _levelIndex);
            
            _button.onClick.AddListener(SelectLevel);
        }
        public void SetLevelIndex(int index) => 
            _levelIndex = index;

        public void SetEnableButton()
        {
            _text.color = Color.white;
            
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }
        
        public void SetSoundService(SoundService soundService) =>
            _soundService = soundService;

        private void SelectLevel()
        {
            _soundService.PlayClickSfx();
            OnSelectLevel?.Invoke(_levelIndex);
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(SelectLevel);

    }
}