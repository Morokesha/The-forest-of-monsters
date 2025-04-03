using System;

namespace Services.ResourceRepositoryService
{
    public class ResourceRepository
    {
        public event Action<int> GoldChanged;
        private int _gold;

        public void AddGold(int amount)
        {
            _gold += amount;
            GoldChanged?.Invoke(_gold);
        }

        public void SpentGold(int amount)
        {
            _gold -= amount;
            GoldChanged?.Invoke(_gold);
        }

        public int GetGold() => 
            _gold;

    }
}