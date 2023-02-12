using System;
using UnityEngine;

namespace Shared
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int health;
        private bool _isDead;
        public event Action<bool> OnTakenDamageEvent;
        public event Action<bool> OnDiedEvent;

        public bool IsDead() => _isDead;

        public void TakeDamage(int amount)
        {
            if (_isDead) return;
            health -= amount;
            OnTakenDamageEvent?.Invoke(_isDead);
            if (health <= 0)
            {
                OnDiedEvent?.Invoke(_isDead);
                _isDead = true;
            }
        }
    }
}