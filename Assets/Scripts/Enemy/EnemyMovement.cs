using Shared;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;
        private int _currentWaypointIndex;
        private NavMeshAgent _agent;
        private EnemyController _enemyController;
        private PlayerReference _playerReference;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _enemyController = GetComponent<EnemyController>();
            _enemyController.OnStateChangedEvent += OnStateChanged;
            _playerReference = FindObjectOfType<PlayerReference>();
        }

        private void OnStateChanged(EnemyState newState)
        {
            if (newState == EnemyState.Chase)
            {
                Chase();
            }
            else if (newState == EnemyState.Patrol)
            {
                Patrol();
            }
        }

        private void Patrol()
        {
            _agent.SetDestination(waypoints[_currentWaypointIndex].position);
        }

        private void Chase()
        {
            _agent.SetDestination(_playerReference.GetPlayerTransform().position);
        }

        private void OnDestroy()
        {
            _enemyController.OnStateChangedEvent -= OnStateChanged;
        }
    }
}