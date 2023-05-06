using UnityEngine;

[CreateAssetMenu(fileName = "sfx_newMusicEvent", menuName = "Audio/New Music Event")]
public class MusicAudioEvent : AudioEvent
{
    #region config
    public AudioClip clip;

    #endregion

    public override GameObject Play(AudioSource audioSourceParam = null)
    {
        if (clip == null)
        {
            Debug.Log("Missing sound clips for " + this);
            return null;
        }

        var source = audioSourceParam;
        if (source == null)
        {
            var _obj = new GameObject("Music SoundSource", typeof(AudioSource));
            source = _obj.GetComponent<AudioSource>();
        }


        source.clip = clip;
        source.loop = loop;
        source.outputAudioMixerGroup = mixGroup;
        source.volume = Random.Range(volume.x, volume.y);
        source.pitch = Random.Range(pitch.x, pitch.y);

        source.Play();

        //return configurations if we want to modify them externally
        return source.gameObject;
    }

    public void Stop(AudioSource audioSourceParam)
    {
        //Destroy after playing
        DestroyImmediate(audioSourceParam.gameObject);
    }
}