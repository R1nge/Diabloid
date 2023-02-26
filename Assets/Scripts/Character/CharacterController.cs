using System.Collections;
using System.Diagnostics;
using Shared;
using UnityEngine;

namespace Character
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private LayerMask groundLayer;
        private CharacterMovement _characterMovement;
        private CharacterAttack _characterAttack;
        private Health _health;
        private Camera _camera;

        private void Awake()
        {
            _characterMovement = GetComponent<CharacterMovement>();
            _characterAttack = GetComponent<CharacterAttack>();
            _health = GetComponent<Health>();
            _health.OnDiedEvent += Die;
            _camera = Camera.main;
        }

        private void Update()
        {
            GetInputPc();
            GetInputMobile();
        }

        [Conditional("PLATFORM_STANDALONE")]
        private void GetInputPc()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MoveToMouse();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                TryAttack();
                Stop();
            }
        }

        [Conditional("PLATFORM_ANDROID")]
        private void GetInputMobile()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MoveToMouse();
                TryAttack();
                Stop();
            }
        }

        private void MoveToMouse() => _characterMovement.MoveTo(GetClickPosition());

        private void Stop() => _characterMovement.Stop();

        private void TryAttack()
        {
            Ray mousePosition = _camera.ScreenPointToRay(Input.mousePosition);
            _characterAttack.TryAttack(mousePosition);
        }

        private Vector3 GetClickPosition()
        {
            Ray mousePosition = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mousePosition, out var hit, rayDistance, groundLayer))
            {
                return hit.point;
            }

            return transform.position;
        }

        private void Die(int health) => StartCoroutine(Die_c());

        private IEnumerator Die_c()
        {
            yield return new WaitForSeconds(.1f);
            foreach (var component in gameObject.GetComponents<Component>())
            {
                if (component is not Transform)
                {
                    Destroy(component);
                }
            }
        }

        private void OnDestroy() => _health.OnDiedEvent -= Die;
    }
}