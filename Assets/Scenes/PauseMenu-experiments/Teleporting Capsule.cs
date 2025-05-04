using UnityEngine;

public class TeleportingCapsule : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out var capsule))
        {
            Vector3 pos;
            pos.x = -2;
            pos.y = 5;
            pos.z = -1;
            capsule.MovePosition(pos);

            Quaternion rot;
            rot.w = 0;
            rot.x = 0;
            rot.y = 0;
            rot.z = -42;
            rot = rot.normalized;
            capsule.MoveRotation(rot);

            //capsule.angularVelocity = Vector3.zero;
            capsule.linearVelocity = Vector3.zero;

        }
    }
}
