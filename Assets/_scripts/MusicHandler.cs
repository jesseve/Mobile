using UnityEngine;
using System.Collections;

public class MusicHandler : MonoBehaviour {

    public static MusicHandler instance;

    private AudioSource source;
    
    public AudioClip menu;
    public AudioClip stage;

    public bool muted;

    void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
        source = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Mute music
    /// </summary>
    public void Mute() {
        muted = !muted;
        if (muted)
            source.volume = 0;
        else
            source.volume = 1;         
    }    

    /// <summary>
    /// Switches the music between stage and menu music
    /// </summary>
    public void Switch() {        
        source.Stop();
        if (source.clip == menu)
            source.clip = stage;
        else
            source.clip = menu;
        source.Play();
    }

    private IEnumerator FadeIn(AudioClip clip, float time) {
        if (muted) yield break;
        source.volume = 0;
        source.clip = clip;
        source.Play();
        while (source.volume < 1) {
            if (muted) break;
            source.volume += 0.01f;
            yield return new WaitForSeconds(time * 0.01f);            
        }        
        source.volume = 1;
        
    }

    private IEnumerator FadeOut(AudioClip clip, float time)
    {
        if (muted) yield break;
        source.volume = 1;
        source.clip = clip;
        source.Play();
        while (source.volume > 0)
        {
            if (muted) break;
            source.volume -= 0.01f;
            yield return new WaitForSeconds(time * 0.01f);            
        }
        source.volume = 0;

    }

    public IEnumerator MenuStageSwitch(float time) {
        if (muted) yield break;
        source.volume = 1;
        while (source.volume > 0)
        {
            if (muted) break;
            source.volume -= 0.01f;
            yield return new WaitForSeconds(time * 0.01f);            
        }
        AudioClip next = source.clip == menu ? stage : menu;
        yield return StartCoroutine(FadeIn(next, time));       
    }
}
