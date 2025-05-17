using UnityEngine;

public class LaserReflector : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.up * 0.5f);
    }
}