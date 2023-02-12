using System;
using UnityEngine;

namespace Shared
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int health;
        private bool _isDead;
        public event Action<int> OnTakenDamageEvent;
        public event Action<int> OnDiedEvent;

        public bool IsDead() => _isDead;

        public void TakeDamage(int amount)
        {
            if (_isDead) return;
            health -= amount;
            OnTakenDamageEvent?.Invoke(health);
            if (health <= 0)
            {
                OnDiedEvent?.Invoke(health);
                _isDead = true;
            }
        }
    }
}