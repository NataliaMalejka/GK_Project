using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Glowing : MonoBehaviour
{
    [Tooltip("Transform gracza")]
    public Transform player;

    [Tooltip("Odległość, przy której obiekt staje się w pełni widoczny")]
    [Range(0.1f, 20f)]
    public float nearDistance = 3f;

    [Tooltip("Odległość, przy której obiekt znika całkowicie")]
    [Range(0.1f, 20f)]
    public float farDistance = 8f;

    private Material materialInstance;
    private static readonly int PlayerDistanceID = Shader.PropertyToID("_PlayerDistance");
    private static readonly int NearDistanceID = Shader.PropertyToID("_NearDistance");
    private static readonly int FarDistanceID = Shader.PropertyToID("_FarDistance");

    void Awake()
    {
        // Sprawdzenie czy wartości są poprawne
        if (nearDistance >= farDistance)
        {
            Debug.LogWarning("Glowing: nearDistance powinna być mniejsza niż farDistance. Automatyczna korekta.");
            nearDistance = Mathf.Max(0.1f, farDistance - 1f);
        }
        
        // Inicjalizacja materiału - używamy instacji materiału
        var renderer = GetComponent<Renderer>();
        materialInstance = new Material(renderer.sharedMaterial);
        renderer.material = materialInstance;
    }

    void OnEnable()
    {
        // Aktualizacja parametrów shadera
        if (materialInstance != null)
        {
            materialInstance.SetFloat(NearDistanceID, nearDistance);
            materialInstance.SetFloat(FarDistanceID, farDistance);
        }
    }

    void Update()
    {
        if (player == null)
        {
            // Próba automatycznego znalezienia gracza, jeśli nie jest ustawiony
            if (Camera.main != null && Camera.main.transform.parent != null)
            {
                player = Camera.main.transform.parent;
                Debug.Log("Glowing: Automatycznie przypisano transform kamery jako player.");
            }
            else
            {
                return;
            }
        }

        if (materialInstance != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            materialInstance.SetFloat(PlayerDistanceID, distance);
        }
    }

    // Aktualizacja parametrów shadera, gdy wartości zostały zmienione w inspektorze
    void OnValidate()
    {
        if (nearDistance >= farDistance)
        {
            farDistance = nearDistance + 0.1f;
        }
    }

    // Czyszczenie zasobów przy zniszczeniu obiektu
    void OnDestroy()
    {
        if (materialInstance != null)
        {
            if (Application.isPlaying)
            {
                Destroy(materialInstance);
            }
            else
            {
                DestroyImmediate(materialInstance);
            }
        }
    }
}