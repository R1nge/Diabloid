using Shared;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float timeBeforeSelectionOfNextWaypoint;
        [SerializeField] private Transform[] waypoints;
        private int _currentWaypointIndex;
        private float _timeBeforeSelectionOfNextWaypoint;
        private NavMeshAgent _agent;
        private EnemyController _enemyController;
        private PlayerReference _playerReference;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _enemyController = GetComponent<EnemyController>();
            _enemyController.OnStateChangedEvent += OnStateChanged;
            _playerReference = FindObjectOfType<PlayerReference>();
            _timeBeforeSelectionOfNextWaypoint = timeBeforeSelectionOfNextWaypoint;
        }

        private void OnStateChanged(EnemyState newState)
        {
            switch (newState)
            {
                case EnemyState.Chase:
                    Chase();
                    break;
            }
        }

        public void Patrol()
        {
            if (waypoints.Length == 0) return;
            if (_timeBeforeSelectionOfNextWaypoint <= 0)
            {
                _timeBeforeSelectionOfNextWaypoint = timeBeforeSelectionOfNextWaypoint;
                SelectNextPatrolPosition();
            }
            else
            {
                _timeBeforeSelectionOfNextWaypoint -= Time.deltaTime;
            }
        }

        private void SelectNextPatrolPosition()
        {
            if (_currentWaypointIndex == waypoints.Length - 1)
            {
                _currentWaypointIndex = 0;
            }
            else
            {
                _currentWaypointIndex++;
            }

            _agent.SetDestination(waypoints[_currentWaypointIndex].position);
        }

        private void Chase() => _agent.SetDestination(_playerReference.GetPlayerTransform().position);

        private void OnDestroy() => _enemyController.OnStateChangedEvent -= OnStateChanged;
    }
}