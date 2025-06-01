using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Laser component that shoots and reflects a laser beam.
 * Can operate in constant or interval mode.
 *
 * @author Krzysztof Gach
 * @version 1.3
 */
public class Laser : MonoBehaviour
{
    [Tooltip("Maximum distance per ray segment (used as a safety limit)")]
    [SerializeField] private float defDistanceRay = 100f;

    [SerializeField] private bool isConstantLaser = true;

    [Tooltip("Toggle interval in milliseconds (only used if not constant)")]
    [SerializeField] private int intervalMs = 1000;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private ExplosionTilemapController explosionController;

    [SerializeField] private bool isActive = false;

    [Tooltip("Layers that the laser should ignore")]
    [SerializeField] private LayerMask ignoredLayers;

    private bool isPowered = false;
    private Coroutine toggleRoutine;

    public bool IsActive => isActive;
    public bool IsPowered => isPowered;

    private const int MAX_REFLECTION_LIMIT = 100;

    private void Start()
    {
        if (isActive)
            Activate();
        else
            SetLaserState(false);
    }

    private void Awake()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (isActive)
            ShootLaser();
    }

    private void SetLaserState(bool state)
    {
        isActive = state;

        if (lineRenderer != null)
            lineRenderer.enabled = isActive;
    }

    public void Activate()
    {
        if (isPowered) return;

        isPowered = true;

        if (isConstantLaser)
            SetLaserState(true);
        else
            toggleRoutine = StartCoroutine(ToggleLaserInterval());
    }

    public void Deactivate()
    {
        isPowered = false;
        SetLaserState(false);

        if (toggleRoutine != null)
        {
            StopCoroutine(toggleRoutine);
            toggleRoutine = null;
        }
    }

    private IEnumerator ToggleLaserInterval()
    {
        var wait = new WaitForSeconds(intervalMs / 1000f);

        while (isPowered)
        {
            SetLaserState(!isActive);
            yield return wait;
        }

        SetLaserState(false);
    }

    private void ShootLaser()
    {
        Vector2 startPos = firePoint.position;
        Vector2 direction = firePoint.right;
        List<Vector2> positions = new() { startPos };

        int bounceCount = 0;
        LayerMask raycastMask = ~ignoredLayers;

        while (bounceCount < MAX_REFLECTION_LIMIT)
        {
            RaycastHit2D hit = Physics2D.Raycast(startPos, direction, defDistanceRay, raycastMask);
            if (hit)
            {
                positions.Add(hit.point);

                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Laser hit the player!");
                    break;
                }

                if (hit.collider.CompareTag("Barrel"))
                {
                    Debug.Log("Laser hit a barrel!");

                    if (explosionController != null)
                    {
                        Vector3 tileCenter = new Vector3(
                            Mathf.Floor(hit.point.x) + 0.5f,
                            Mathf.Floor(hit.point.y) + 0.5f,
                            0f
                        );

                        explosionController.ExplodeAt(tileCenter);
                    }

                    Destroy(hit.collider.gameObject);
                    break;
                }

                if(hit.collider.CompareTag("Crate"))
                {
                    Debug.Log("Laser hit a crate!");
                    hit.collider.GetComponent<Crate>()?.DestroySelf();
                    break;
                }

                if (hit.collider.TryGetComponent(out LaserReflector reflector))
                {
                    direction = Vector2.Reflect(direction, hit.normal);
                    startPos = hit.point + direction * 0.01f;
                    bounceCount++;
                }
                else break;
            }
            else
            {
                positions.Add(startPos + direction * defDistanceRay);
                break;
            }
        }

        lineRenderer.positionCount = positions.Count;
        for (int i = 0; i < positions.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(positions[i].x, positions[i].y, -0.000001f));
        }
    }
}
