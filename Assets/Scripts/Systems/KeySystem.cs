using UnityEngine;
using UnityEngine.UI;

/** 
 * 
 * @author Krzysztof Gach
 * @version 1.0
 */
public class KeySystem : MonoBehaviour, IGameSystem
{
    [SerializeField] private int keyCount = 0;
    [SerializeField] private Text keyCountText;

    public void Initialize()
    {
        UpdateUI();
    }

    public void AddKey()
    {
        keyCount++;
        UpdateUI();
    }

    public bool UseKey(Gate gate)
    {
        if (keyCount > 0 && gate != null && !gate.IsOpen)
        {
            keyCount--;
            UpdateUI();
            gate.Open();
            return true;
        }
        return false;
    }

    public int GetKeyCount() => keyCount;

    public void UpdateUI()
    {
        if (keyCountText != null)
        {
            keyCountText.text = $"Keys: {keyCount}";
        }
    }
}
