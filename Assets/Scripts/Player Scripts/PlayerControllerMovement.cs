using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class PlayerControllerMovement : MonoBehaviour
    {
        public float speed;
        public float pushableDetectRadius = 2f;  // radius to find pushables

        private Vector2 currentMoveDir; 

        [Header("UI Systems")]
        [SerializeField] private BatterySystem batterySystem;

        private Animator animator;
        private Rigidbody2D rb2d;

        private void Start()
        {
            animator = GetComponent<Animator>();
            rb2d = GetComponent<Rigidbody2D>();

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
            currentMoveDir = dir; // update currentMoveDir here

            animator.SetBool("IsMoving", dir.magnitude > 0);

            rb2d.velocity = speed * dir;

            HandlePushables();
        }

       private void HandlePushables()
        {
            // Find all pushable objects within radius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pushableDetectRadius);
            List<PushableObject> pushables = new List<PushableObject>();

            foreach (var col in colliders)
            {
                PushableObject pushable = col.GetComponent<PushableObject>();
                if (pushable != null)
                {
                    pushables.Add(pushable);
                }
            }

            // Find closest pushable
            PushableObject closestPushable = null;
            float closestDist = Mathf.Infinity;

            foreach (var pushable in pushables)
            {
                Vector2 toPushable = (pushable.transform.position - transform.position);
                float angle = Vector2.Angle(currentMoveDir, toPushable);

                // Sprawdź, czy pushable jest mniej więcej "przed" graczem (kąt mniejszy niż 45 stopni)
                if (angle > 45f)
                    continue;  // ignoruj obiekty poza osią patrzenia

                float dist = toPushable.magnitude;
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestPushable = pushable;
                }
            }

            // Lock all pushables by freezing all constraints
            foreach (var pushable in pushables)
            {
                Rigidbody2D prb = pushable.GetComponent<Rigidbody2D>();
                if (prb != null)
                {
                    prb.constraints = RigidbodyConstraints2D.FreezeAll;
                    prb.linearVelocity = Vector2.zero; // velocity zamiast linearVelocity
                }
            }

            // Unlock constraints and push the closest pushable
            if (closestPushable != null && currentMoveDir.sqrMagnitude > 0)
            {
                Rigidbody2D prb = closestPushable.GetComponent<Rigidbody2D>();
                if (prb != null)
                {
                    prb.constraints = RigidbodyConstraints2D.FreezeRotation;

                    float pushSpeed = speed * 0.5f;  // tweak push speed multiplier
                    Vector2 targetVelocity = currentMoveDir * pushSpeed;

                    prb.linearVelocity = Vector2.Lerp(prb.linearVelocity, targetVelocity, Time.deltaTime * 10f);
                }
            }
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
