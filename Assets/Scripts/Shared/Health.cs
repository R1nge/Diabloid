using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Shared
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int health;
        public event Action OnTakenDamageEvent;
        public event Action OnDiedEvent;
        
        public void TakeDamage(int amount)
        {
            health -= amount;
            OnTakenDamageEvent?.Invoke();
            if (health <= 0)
            {
                OnDiedEvent?.Invoke();
            }
        }
    }
}