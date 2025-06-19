using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PropsAltar : MonoBehaviour, IFixedUpdateObserver
{
    public List<SpriteRenderer> runes;
    public float lerpSpeed;

    private Color curColor;
    private Color targetColor;

    [SerializeField] private float TimeToTeleport;
    [SerializeField] private float currentTime = 0;

    private bool inPortal = false;

    private void Awake()
    {
        targetColor = runes[0].color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IPlayer player = other.GetComponent<IPlayer>();

        if (player != null)
        {
            targetColor.a = 1.0f;
            inPortal = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IPlayer player = other.GetComponent<IPlayer>();

        if (player != null)
        {
            targetColor.a = 0.0f;
            inPortal = false;
        }
    }

    private void OnEnable()
    {
        FixedUpdateManager.AddToList(this);
    }

    private void OnDisable()
    {
        FixedUpdateManager.RemoveFromList(this);
    }

    public void ObserveFixedUpdate()
    {
        curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.fixedDeltaTime);

        foreach (var r in runes)
        {
            r.color = curColor;
        }

        if(inPortal)
        {
            currentTime += Time.fixedDeltaTime;

            if (currentTime >= TimeToTeleport)
            {
                GameManager.Instance.Teleport();
                currentTime = 0;
            }

            Debug.Log(currentTime);
        }
    }
}
