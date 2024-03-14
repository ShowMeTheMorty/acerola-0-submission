using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class SFX
    {
        public List<AudioClip> clips;
        public float volume = 1f;
        public float pitchVary = 0f;
    }

    public enum SFX_TYPE
    {
        SFX_STEP_DRY,
        SFX_STEP_SOFT,
        SFX_STEP_GRAVEL,
        SFX_STEP_WET,
        LANDING,
        BARKING
    }

    
    public SFX SFX_STEP_DRY;
    public SFX SFX_STEP_SOFT;
    public SFX SFX_STEP_GRAVEL;
    public SFX SFX_STEP_WET;
    public SFX LANDING;
    public SFX BARKING;

    public AudioSource audioSourceTemplate;
    private AudioSource musicSource;
    public static AudioManager singleton;

    public float musicFade;

    internal AudioClip desiredClip;
    internal float desiredClipVolume;
    internal bool currentlyPlaying;

    void Start()
    {
        singleton = this;
        musicSource = GetComponent<AudioSource>();
    }

    public void SetDesiredMusicClip (AudioClip track, float volume)
    {
        desiredClip = track;
        desiredClipVolume = volume;
        if (!currentlyPlaying) currentlyPlaying = true;
    }

    void Update ()
    {
        if (currentlyPlaying)
        {
            if (desiredClip == musicSource.clip) musicSource.volume = musicSource.volume * (1f - musicFade) + desiredClipVolume * musicFade;
            else if (musicSource.volume < 0.05) 
            {
                if (desiredClip == null) 
                {
                    musicSource.Stop();
                    musicSource.volume = 0f;
                    currentlyPlaying = false;
                }
                else musicSource.clip = desiredClip;
            }
            else musicSource.volume = musicSource.volume * (1f - musicFade);
            if (!musicSource.isPlaying && currentlyPlaying) musicSource.Play();
        }
        

    }

    public AudioSource CreateSoundEffect (SFX_TYPE type, Vector3 location, float volumeMult = 1f)
    {
        switch (type)
        {
            case SFX_TYPE.SFX_STEP_DRY: return CreateSoundEffect(SFX_STEP_DRY, location, volumeMult);
            case SFX_TYPE.SFX_STEP_SOFT: return CreateSoundEffect(SFX_STEP_SOFT, location, volumeMult);
            case SFX_TYPE.SFX_STEP_GRAVEL: return CreateSoundEffect(SFX_STEP_GRAVEL, location, volumeMult);
            case SFX_TYPE.SFX_STEP_WET: return CreateSoundEffect(SFX_STEP_WET, location, volumeMult);
            case SFX_TYPE.LANDING: return CreateSoundEffect(LANDING, location, volumeMult);
            case SFX_TYPE.BARKING: return CreateSoundEffect(BARKING, location, volumeMult);
            default: return CreateSoundEffect(LANDING, location, volumeMult);
        }
    }

    public AudioSource CreateSoundEffect(SFX sfx, Vector3 location, float volumeMult=1f)
    {
        AudioClip chosenClip = PickRandomly(sfx.clips);
        float pitch = UnityEngine.Random.Range(1f-sfx.pitchVary, 1f+sfx.pitchVary);
        AudioSource thatNewSound = Instantiate(audioSourceTemplate, location, Quaternion.identity);
        thatNewSound.clip = chosenClip;
        thatNewSound.volume = sfx.volume * volumeMult;
        thatNewSound.pitch = pitch;
        thatNewSound.Play();
        Destroy(thatNewSound.gameObject, chosenClip.length * pitch);
        return thatNewSound;
    }

    private T PickRandomly<T>(List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count - 1)];
    }
}
