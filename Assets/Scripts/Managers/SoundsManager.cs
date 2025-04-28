using UnityEngine;

public enum Sounds
{
    CollectCoin,
    Die,
    ButtonClick
}

public class SoundsManager : PersistentSingleton<SoundsManager>
{
    [SerializeField] private AudioClip collectCoin;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip buttonClick;

    private AudioSource audiosource;
    
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.CollectCoin:
                audiosource.PlayOneShot(collectCoin);
                break;
            case Sounds.Die:
                audiosource.PlayOneShot(die);
                break;
            case Sounds.ButtonClick:
                audiosource.PlayOneShot(buttonClick);
                break;
            default:
                break;
        }
    }
}
