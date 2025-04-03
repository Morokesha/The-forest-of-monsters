using Services.ResourceRepositoryService;
using TMPro;
using UnityEngine;

namespace UI.UIRootElements
{
    public class GoldCounterUI : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _goldText;

        private ResourceRepository _resourceRepository;

        public void Init(ResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;

            _resourceRepository.GoldChanged += GoldUpdate;
            
            GoldUpdate(_resourceRepository.GetGold());
        }

        private void GoldUpdate(int amount) =>
            _goldText.SetText(amount.ToString());
    }
}