using Shared;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterHealthUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _health.OnTakenDamageEvent += UpdateHealthUI;
        }

        private void UpdateHealthUI(int health)
        {
            if (health < 0) return;
            slider.value = health;
        }

        private void OnDestroy()
        {
            _health.OnTakenDamageEvent -= UpdateHealthUI;
        }
    }
}