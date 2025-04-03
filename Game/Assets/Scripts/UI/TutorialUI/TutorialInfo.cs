using System;
using System.Collections;
using System.Collections.Generic;
using Data.TurretsData;
using GameLogic.Turrets.TurretSpawn;
using Services.SaveLoadServices;
using UI.BuyTurretUI;
using UI.UpgradeUI;
using UnityEngine;

namespace UI.TutorialUI
{
    public class TutorialInfo : MonoBehaviour
    {
        [SerializeField] 
        private List<GameObject> _tutorialSteps;

        private ISaveLoadService _saveLoadService;
        private TurretSpawner _turretSpawner;
        private BuyTurretMenu _buyTurretMenu;
        private UpgradeTurretMenu _upgradeTurretMenu;

        private int _index = 0;

        private bool _stepOneIsDone = false;
        private bool _stepTwoIsDone = false;
        private bool _stepThreeIsDone = false;
        private bool _stepFourIsDone = false;
        private bool _stepFiveIsDone = false;
        private bool _stepSixIsDone = false;
        public void Init(TurretSpawner turretSpawner, BuyTurretMenu buyTurretMenu, UpgradeTurretMenu upgradeTurretMenu)
        {
            _turretSpawner = turretSpawner;
            _buyTurretMenu = buyTurretMenu;
            _upgradeTurretMenu = upgradeTurretMenu;
        }

        private void Start()
        {
            _turretSpawner.OnShopOpened += StepFirstActivated;
            _buyTurretMenu.OnTurretSelection += StepTwoActivated;
            _turretSpawner.OnTurretPurchased += StepThirdActivated;
            _turretSpawner.OnTurretSelected += StepFourActivated;
            _upgradeTurretMenu.OnSelectUpgrade += StepFiveActivated;
            _upgradeTurretMenu.OnClickUpgrade += StepSixActivated;
        }
        public void SetSaveLoadService(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            
            if (_saveLoadService.GetProgress().TutorialIsDone) 
                Destroy(gameObject);
            
            _tutorialSteps[_index].SetActive(true);
        }

        private void StepFirstActivated()
        {
            if (!_stepOneIsDone)
                TutorialStepsActive();
            _stepOneIsDone = true;
        }

        private void StepTwoActivated()
        {
            if (!_stepTwoIsDone)
                TutorialStepsActive();
            _stepTwoIsDone = true;
        }

        private void StepThirdActivated()
        {
            if (!_stepThreeIsDone)
                TutorialStepsActive();
            _stepThreeIsDone = true;
        }

        private void StepFourActivated()
        {
            if (!_stepFourIsDone) 
                TutorialStepsActive();
            _stepFourIsDone = true;
        }

        private void StepFiveActivated()
        {
            if (!_stepFiveIsDone) 
                TutorialStepsActive();
            _stepFiveIsDone = true;
        }

        private void StepSixActivated(TurretData turretData)
        {
            if (!_stepSixIsDone)
                TutorialStepsActive();
            _stepSixIsDone = true;
            
            StepSevenActivated();
        }

        private void StepSevenActivated()
        {
            _saveLoadService.GetProgress().CompleteTutorial();
            _saveLoadService.Save();

            StartCoroutine(ShowDefenceInfoCo());
        }

        private IEnumerator ShowDefenceInfoCo()
        {
            yield return new WaitForSeconds(5f);
            _tutorialSteps[^1].SetActive(false);
        }

        private void TutorialStepsActive()
        {
            _index += 1;
            int previouslyIndex = _index - 1;
                    
            for (int i = 0; i < _tutorialSteps.Count; i++)
            {
                if (previouslyIndex >= 0) 
                    _tutorialSteps[previouslyIndex].SetActive(false);

                if (_index <= _tutorialSteps.Count)
                {
                    _tutorialSteps[_index].SetActive(true);
                }
            }
        }

        private void OnDestroy()
        {
            _turretSpawner.OnShopOpened -= StepFirstActivated;
            _buyTurretMenu.OnTurretSelection -= StepTwoActivated;
            _turretSpawner.OnTurretPurchased -= StepThirdActivated;
            _turretSpawner.OnTurretSelected -= StepFourActivated;
            _upgradeTurretMenu.OnSelectUpgrade -= StepFiveActivated;
            _upgradeTurretMenu.OnClickUpgrade -= StepSixActivated;
        }
    }
}