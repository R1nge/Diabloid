﻿using Character;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float timeBeforeSelectionOfNextWaypoint;
        [SerializeField] private Transform[] waypoints;
        private int _currentWaypointIndex;
        private float _timeBeforeSelectionOfNextWaypoint;
        private NavMeshAgent _agent;
        private EnemyState _enemyState;
        private PlayerController _playerController;

        [Inject]
        private void Construct(PlayerController player) => _playerController = player;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _enemyState = GetComponent<EnemyState>();
            _enemyState.OnStateChangedEvent += OnStateChanged;
            _timeBeforeSelectionOfNextWaypoint = timeBeforeSelectionOfNextWaypoint;
        }

        private void OnStateChanged(EnemyStates newStates)
        {
            switch (newStates)
            {
                case EnemyStates.Chase:
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

        private void Chase() => _agent.SetDestination(_playerController.transform.position);

        private void OnDestroy() => _enemyState.OnStateChangedEvent -= OnStateChanged;
    }
}