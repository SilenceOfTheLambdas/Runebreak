using UnityEngine;

namespace Character
{
    public class GroundCheck : MonoBehaviour
    {
        private PlayerMovementController _playerMovementController;

        private void Start()
        {
            _playerMovementController = GetComponentInParent<PlayerMovementController>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _playerMovementController.IsPlayerGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _playerMovementController.IsPlayerGrounded = false;
            }
        }
    }
}
