using UnityEngine;

public enum Sounds
{
    CollectCoin,
    CollectEnemyCoin,
    CollectMixtures,
    Explosion,
    GetDmg
}

public class SoundsManager : PersistentSingleton<SoundsManager>
{
    [SerializeField] private AudioClip collectCoin;
    [SerializeField] private AudioClip collectEnemyCoin;
    [SerializeField] private AudioClip collectMixtures;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip getDmg;


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
            case Sounds.CollectEnemyCoin:
                audiosource.PlayOneShot(collectEnemyCoin);
                break;
            case Sounds.CollectMixtures:
                audiosource.PlayOneShot(collectMixtures);
                break;
            case Sounds.Explosion:
                audiosource.PlayOneShot(explosion);
                break;
            case Sounds.GetDmg:
                audiosource.PlayOneShot(getDmg);
                break;
            default:
                break;
        }
    }
}
