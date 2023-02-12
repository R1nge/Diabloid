using System.Collections;
using Shared;
using UnityEngine;

namespace Character
{
    public class CharacterAttack : MonoBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private int damage;
        [SerializeField] private float attackInterval;
        [SerializeField] private float attackDistance;
        [SerializeField] private LayerMask ignoreLayer;
        private Camera _camera;
        private bool _canAttack = true;
        //TODO: use state machine
        private CharacterAnimator _characterAnimator;

        private void Awake()
        {
            _camera = Camera.main;
            //TODO: use state machine
            _characterAnimator = GetComponent<CharacterAnimator>();
        }

        private void Update()
        {
            if (!_canAttack) return;
            if (Input.GetMouseButtonDown(1))
            {
                Ray mousePosition = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mousePosition, out var hit, rayDistance, ~ignoreLayer))
                {
                    if (Vector3.Distance(transform.position, hit.point) <= attackDistance)
                    {
                        if (hit.transform.TryGetComponent(out Health health))
                        {
                            //TODO: use state machine
                            _characterAnimator.PlayAttackAnimation();
                            health.TakeDamage(damage);
                            StartCoroutine(Attack_c());
                        }
                    }
                }
            }
        }

        private IEnumerator Attack_c()
        {
            _canAttack = false;
            yield return new WaitForSeconds(attackInterval);
            _canAttack = true;
        }
    }
}