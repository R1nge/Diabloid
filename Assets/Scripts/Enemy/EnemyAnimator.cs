using Shared;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int OnAttack = Animator.StringToHash("OnAttack");
        private static readonly int OnHurt = Animator.StringToHash("OnHurt");
        private static readonly int OnDeath = Animator.StringToHash("OnDeath");
        private EnemyController _enemyController;
        private Health _health;

        private void Awake()
        {
            _enemyController = GetComponent<EnemyController>();
            _enemyController.OnStateChangedEvent += PlayAttackAnimation;
            _health = GetComponent<Health>();
            _health.OnTakenDamageEvent += PlayHurtAnimation;
            _health.OnDiedEvent += PlayDeathAnimation;
        }

        private void PlayAttackAnimation(EnemyState state)
        {
            if (state == EnemyState.Attack)
            {
                animator.SetTrigger(OnAttack);
            }
            else
            {
                animator.ResetTrigger(OnAttack);
            }
        }

        private void PlayHurtAnimation()
        {
            animator.SetTrigger(OnHurt);
        }

        private void PlayDeathAnimation()
        {
            animator.SetTrigger(OnDeath);
        }

        private void Update() => animator.SetBool(IsWalking, agent.velocity.magnitude > 0);

        private void OnDestroy()
        {
            _enemyController.OnStateChangedEvent -= PlayAttackAnimation;
            _health.OnTakenDamageEvent -= PlayHurtAnimation;
            _health.OnDiedEvent -= PlayDeathAnimation;
        }
    }
}