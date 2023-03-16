using System;
using System.Collections;
using Character;
using Shared;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float attackInterval;
        private EnemyState _currentState;
        private bool _canAttack = true;
        private Health _health;
        private EnemyMovement _enemyMovement;
        private PlayerController _playerController;

        public event Action<EnemyState> OnStateChangedEvent;

        [Inject]
        private void Construct(PlayerController player) => _playerController = player;

        public void ChangeState(EnemyState state)
        {
            _currentState = state;
            OnStateChangedEvent?.Invoke(_currentState);
        }

        private void Awake()
        {
            _health = GetComponent<Health>();
            _health.OnDiedEvent += Die;
            _enemyMovement = GetComponent<EnemyMovement>();
        }

        private void Start() => ChangeState(EnemyState.Patrol);

        private bool IsInAttackRange()
        {
            var playerPos = _playerController.transform.position;
            var distance = Vector3.Distance(playerPos, transform.position);
            return distance <= attackRange;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case EnemyState.Patrol:
                    Patrol();
                    break;
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
            }
        }

        private void Patrol() => _enemyMovement.Patrol();

        private void Chase() => ChangeState(IsInAttackRange() ? EnemyState.Attack : EnemyState.Chase);

        private void Attack()
        {
            if (_playerController.TryGetComponent(out Health health))
            {
                if (health.IsDead())
                {
                    ChangeState(EnemyState.Idle);
                    return;
                }
            }

            if (!_canAttack)
            {
                ChangeState(EnemyState.Chase);
            }
            else
            {
                StartCoroutine(Attack_c());
            }
        }

        private IEnumerator Attack_c()
        {
            _canAttack = false;
            yield return new WaitForSeconds(attackInterval);
            _canAttack = true;
        }

        private void Die(int health)
        {
            ChangeState(EnemyState.Dead);
            StartCoroutine(Die_c());
        }

        private IEnumerator Die_c()
        {
            yield return new WaitForSeconds(.1f);
            foreach (var component in gameObject.GetComponents<Component>())
            {
                if (component is Transform) continue;
                if (component is Animator) continue;
                Destroy(component);
            }
        }

        private void OnDestroy() => _health.OnDiedEvent -= Die;
    }
}