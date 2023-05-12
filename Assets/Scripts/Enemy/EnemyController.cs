using System;
using System.Threading;
using Character;
using Cysharp.Threading.Tasks;
using Shared;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float attackInterval;
        private bool _canAttack = true;
        private Health _health;
        private EnemyMovement _enemyMovement;
        private EnemyState _enemyState;
        private PlayerController _playerController;
        private readonly CancellationTokenSource _attackCancellation = new();

        [Inject]
        private void Construct(PlayerController player) => _playerController = player;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _health.OnDiedEvent += Die;
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyState = GetComponent<EnemyState>();
        }

        private async void Update()
        {
            switch (_enemyState.GetCurrentState())
            {
                case EnemyStates.Patrol:
                    Patrol();
                    break;
                case EnemyStates.Chase:
                    Chase();
                    break;
                case EnemyStates.Attack:
                    await Attack();
                    break;
            }
        }

        private void Patrol() => _enemyMovement.Patrol();

        private void Chase()
        {
            if (IsInAttackRange())
            {
                _enemyState.ChangeState(EnemyStates.Attack);
            }

            else
            {
                _enemyState.ChangeState(EnemyStates.Chase);
                _attackCancellation.Cancel();
            }
        }

        private bool IsInAttackRange()
        {
            var playerPos = _playerController.transform.position;
            var distance = Vector3.Distance(playerPos, transform.position);
            return distance <= attackRange;
        }

        private async UniTask Attack()
        {
            if (_playerController.TryGetComponent(out Health health))
            {
                if (health.IsDead())
                {
                    _enemyState.ChangeState(EnemyStates.Idle);
                    return;
                }
            }

            if (!_canAttack)
            {
                _enemyState.ChangeState(EnemyStates.Chase);
            }
            else
            {
                await Attack_task(_attackCancellation.Token);
            }
        }

        private async UniTask Attack_task(CancellationToken cancellationToken)
        {
            _canAttack = false;
            await UniTask
                .Delay(TimeSpan.FromSeconds(attackInterval), DelayType.Realtime, cancellationToken: cancellationToken)
                .SuppressCancellationThrow();
            _canAttack = true;
        }

        private async void Die(int health)
        {
            _enemyState.ChangeState(EnemyStates.Dead);
            await Die_task();
        }

        private async UniTask Die_task()
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(100), DelayType.Realtime);
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