using Services.SoundServices;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UIRootElements
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] 
        private Button _settingsBtn;
        
        private SettingsPanel _settingsPanel;
        private SoundService _soundService;


        private void Start()
        {
            _settingsBtn.onClick.AddListener(ShowSettingsMenu);
        }

        public void SetPreferences(SettingsPanel settingsPanel, SoundService soundService)
        {
            _settingsPanel = settingsPanel;
            _soundService = soundService;
            print(_soundService);
        }
        
        private void ShowSettingsMenu()
        {
            _soundService.PlayClickSfx();
            _settingsPanel.Show();
        }

        private void OnDestroy()
        {
            _settingsBtn.onClick.RemoveListener(ShowSettingsMenu);
        }

    }
}