using UnityEngine;

namespace Misc
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private Texture2D cursor;
        [SerializeField] private LayerMask ignoreLayer;
        private Camera _camera;

        private void Awake() => _camera = Camera.main;

        private void Update()
        {
            Ray mousePosition = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mousePosition, out var hit, Mathf.Infinity, ~ignoreLayer))
            {
                if (hit.transform.TryGetComponent(out CursorFlag _))
                {
                    Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
                }
                else
                {
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
    }
}