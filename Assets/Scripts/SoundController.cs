using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }
    public MusicAudioEvent MainMusic { get => _mainMusic; private set => _mainMusic = value; }
    public SFXAudioEvent PlayerGunShot { get => _playerGunShot; private set => _playerGunShot = value; }
    public SFXAudioEvent MobDeath { get => _mobDeath; private set => _mobDeath = value; }
    public SFXAudioEvent Teleport { get => _teleport; private set => _teleport = value; }

    [SerializeField] MusicAudioEvent _mainMusic;
    [SerializeField] SFXAudioEvent _playerGunShot;
    [SerializeField] SFXAudioEvent _mobDeath;
    [SerializeField] SFXAudioEvent _teleport;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}