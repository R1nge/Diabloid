using System;
using Shared;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float attackRange;
        [SerializeField] private int damage;
        [SerializeField] private Transform[] waypoints;
        private int _currentWaypointIndex;
        private NavMeshAgent _agent;
        private PlayerReference _playerReference;
        private EnemyState _currentState;
        private bool _isDead;
        private Health _health;

        public event Action<EnemyState> OnStateChangedEvent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _playerReference = FindObjectOfType<PlayerReference>();
            _health = GetComponent<Health>();
            _health.OnDiedEvent += Die;
        }

        public void ChangeState(EnemyState state)
        {
            if (_isDead) return;
            _currentState = state;
            OnStateChangedEvent?.Invoke(_currentState);
        }

        private void Update()
        {
            switch (_currentState)
            {
                case EnemyState.Idle:
                    Idle();
                    break;
                case EnemyState.Patrol:
                    Patrol();
                    break;
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.Dead:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsInAttackRange()
        {
            var playerPos = _playerReference.GetPlayerTransform().position;
            var distance = Vector3.Distance(playerPos, transform.position);
            return distance <= attackRange;
        }

        private void Idle()
        {
        }

        private void Patrol()
        {
            _agent.SetDestination(waypoints[_currentWaypointIndex].position);
        }

        private void Chase()
        {
            _agent.SetDestination(_playerReference.GetPlayerTransform().position);
            if (IsInAttackRange())
            {
                ChangeState(EnemyState.Attack);
            }
        }

        private void Attack()
        {
            if (_playerReference.GetPlayerTransform().TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }
        }

        private void Die()
        {
            ChangeState(EnemyState.Dead);
            _isDead = true;
        }

        private void OnDestroy() => _health.OnDiedEvent -= Die;
    }
}