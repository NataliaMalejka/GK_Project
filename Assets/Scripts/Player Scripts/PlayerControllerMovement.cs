using System.Collections;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class PlayerControllerMovement : MonoBehaviour
    {
        public float speed;

        [Header("UI Systems")]
        [SerializeField] private BatterySystem batterySystem;

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();

            if (batterySystem == null)
            {
                batterySystem = GetComponent<BatterySystem>();
                if (batterySystem == null)
                {
                    Debug.LogError("BatterySystem component not found on Player!");
                }
            }
        }

        [System.Obsolete]
        private void Update()
        {
            Vector2 dir = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
            {
                dir.x = -1;
                animator.SetInteger("Direction", 3);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
                animator.SetInteger("Direction", 2);
            }

            if (Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
                animator.SetInteger("Direction", 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
                animator.SetInteger("Direction", 0);
            }

            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().velocity = speed * dir;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Player collided with: " + other.name);

            if (other.CompareTag("Battery"))
            {
                batterySystem.AddBattery();
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Pot"))
            {
                Pot pot = other.GetComponent<Pot>();
                pot?.DestroySelf();
            }

            if (other.TryGetComponent<IPickup>(out var pickup))
            {
                Debug.Log("Found IPickup on " + other.name);
                pickup.Collect(gameObject);
            }
        }
    }
}
