using Shared;
using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int OnAttack = Animator.StringToHash("OnAttack");
        private static readonly int OnHurt = Animator.StringToHash("OnHurt");
        private static readonly int OnDeath = Animator.StringToHash("OnDeath");
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _health.OnTakenDamageEvent += PlayHurtAnimation;
            _health.OnDiedEvent += PlayDeathAnimation;
        }

        public void PlayAttackAnimation()
        {
            animator.SetTrigger(OnAttack);
        }

        private void PlayHurtAnimation(int health)
        {
            if (health <= 0) return;
            animator.SetTrigger(OnHurt);
        }

        private void PlayDeathAnimation(int health)
        {
            animator.SetTrigger(OnDeath);
        }

        private void Update() => animator.SetBool(IsWalking, agent.velocity.magnitude > 0);

        private void OnDestroy()
        {
            _health.OnTakenDamageEvent -= PlayHurtAnimation;
            _health.OnDiedEvent -= PlayDeathAnimation;
        }
    }
}