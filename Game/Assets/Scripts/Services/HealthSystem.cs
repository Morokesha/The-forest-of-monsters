using System;

namespace Services
{
    public class HealthSystem
    {
        public event Action OnDeath;
        public event Action<int> OnHealthChanged;
        
        public int MaxHealth;
        private int _currentHealth;


        public void SetMaxHealth(int amount)
        {
            MaxHealth = amount;
            _currentHealth = MaxHealth;
            OnHealthChanged?.Invoke(_currentHealth);
        }
        
        public void TakeDamage(int damage)
        {
            if (_currentHealth >= 0)
            {
                _currentHealth -= damage;
                OnHealthChanged?.Invoke(_currentHealth);
            }
            if(_currentHealth <= 0)
                OnDeath?.Invoke();
        }
    }
}