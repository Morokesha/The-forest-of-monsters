using System;
using Core.Extension;
using DG.Tweening;
using Services.SoundServices;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenuElements
{
    public class SoundSettingsMenu : MonoBehaviour
    {
        public event Action<bool> OnSoundAction; 
        public event Action<bool> OnMusicAction;
        public event Action OnExit;
        
        [SerializeField] private Button _soundsBtn;
        [SerializeField] private Button _musicBtn;
        [SerializeField] private Button _exitBtn;

        [SerializeField] private Image _soundSlider;
        [SerializeField] private Image _musicSlider;
        [SerializeField] private CanvasGroup _canvasGroup;

        private bool _soundActivated = true;
        private bool _musicActivated = true;

        private readonly float _sliderPosX = 50f;
        
        private SoundService _soundService;

        public void Init(SoundService soundService) =>
            _soundService = soundService;

        private void OnEnable()
        {
            _canvasGroup.SetActive(false);
            
            _soundsBtn.onClick.AddListener(OnClickSound);
            _musicBtn.onClick.AddListener(OnClickMusic);
            _exitBtn.onClick.AddListener(OnClickExit);
        }
        
        public void SetSliderPositionOnLoad(bool soundActivated, bool musicActivated)
        {
            _soundActivated = soundActivated;
            _musicActivated = musicActivated;
            
            AnimationSlider(_soundSlider, _soundActivated);
            AnimationSlider(_musicSlider, _musicActivated);
        }
        public void Show() => 
            _canvasGroup.SetActive(true);
        
        private void OnClickMusic()
        {
            _soundService.PlayClickSfx();
            
            if (_musicActivated)
            {
                _musicActivated = false;
                AnimationSlider(_musicSlider, _musicActivated);
                OnMusicAction?.Invoke(_musicActivated);
                return;
            }
            
            if (_musicActivated == false)
            {
                _musicActivated = true;
                AnimationSlider(_musicSlider, _musicActivated);
                OnMusicAction?.Invoke(_musicActivated);
            }
        }

        private void OnClickSound()
        {
            _soundService.PlayClickSfx();
            
            if (_soundActivated)
            {
                _soundActivated = false;
                AnimationSlider(_soundSlider, _soundActivated);
                OnSoundAction?.Invoke(_soundActivated);
                return;
            }

            if (_soundActivated == false)
            {
                _soundActivated = true;
                AnimationSlider(_soundSlider, _soundActivated);
                OnSoundAction?.Invoke(_soundActivated);
            } 
        }

        private void OnClickExit()
        {
            _soundService.PlayClickSfx();
            
            _canvasGroup.SetActive(false);
            OnExit?.Invoke();
        }

        private void AnimationSlider(Image slider, bool active)
        {
            if (!active)
            {
                Vector3 currentPos = slider.rectTransform.localPosition;
                currentPos.x -= _sliderPosX;
                
                slider.color = Color.grey;
                slider.rectTransform.DOLocalMove(currentPos, 0.2f);
            }
            else
            {
                Vector3 currentPos = slider.rectTransform.localPosition;
                currentPos.x = _sliderPosX/2;
                
                slider.color = Color.green;
                slider.rectTransform.DOLocalMove(currentPos, 0.2f);
            }
        }

        private void OnDestroy()
        {
            _soundsBtn.onClick.RemoveListener(OnClickSound);
            _musicBtn.onClick.RemoveListener(OnClickMusic);
            _exitBtn.onClick.RemoveListener(OnClickExit);
        }
    }
}