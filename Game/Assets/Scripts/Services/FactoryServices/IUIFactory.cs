using Services.ResourceRepositoryService;
using UI;
using UI.BuyTurretUI;
using UI.EndGameUI;
using UI.MainMenuElements;
using UI.TutorialUI;
using UI.UIRootElements;
using UI.UpgradeUI;
using UnityEngine;

namespace Services.FactoryServices
{
    public interface IUIFactory
    {
        public void CreateRootUI();
        public BuyTurretMenu CreateBuyTurretMenu();
        public UpgradeTurretMenu CreateUpgradeMenu();
        public TutorialInfo CreateTutorialInfo();
        public SliderToEndLevel CreateLevelEndSlider();
        public EndGameWindow CreateEndGameWindow();
        public void CreateGoldCounterUI(ResourceRepository resourceRepository);
        SoundSettingsMenu CreateSoundSettingsMenu();
        SelectLevelBtn CreateSelectLevelButton(Transform parent);
        public SettingsPanel CreateSettingsPanel();
    }
}