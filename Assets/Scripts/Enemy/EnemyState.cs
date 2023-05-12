using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyState : MonoBehaviour
    {
        private EnemyStates _currentState;
        
        public event Action<EnemyStates> OnStateChangedEvent;

        public EnemyStates GetCurrentState() => _currentState;
        
        public void ChangeState(EnemyStates states)
        {
            _currentState = states;
            OnStateChangedEvent?.Invoke(_currentState);
        }
        
        private void Start() => ChangeState(EnemyStates.Patrol);
    }
}