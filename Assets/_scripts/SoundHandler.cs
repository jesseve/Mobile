using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundHandler : MonoBehaviour {

    public static SoundHandler instance;

    private AudioSource source;

    public Image buttonImage;

    public Sprite muteOn, muteOff;

    public AudioClip[] comboSounds;
    public AudioClip damage, button, purchase, shield, pause;

    public bool muted;

    void Start()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
        source = GetComponent<AudioSource>();
        buttonImage.sprite = muted ? muteOn : muteOff;
    }

    /// <summary>
    /// Mutes the sounds
    /// </summary>
    public void Mute() {
        muted = !muted;
        if (muted)
        {
            buttonImage.sprite = muteOn;
            source.volume = 0;
        }
        else
        {
            buttonImage.sprite = muteOff;
            source.volume = 1;
        }
    }        

    /// <summary>
    /// Plays the sound depending on players combo
    /// Only plays 10, 20, 30 etc up to 70
    /// </summary>
    /// <param name="comboCount"></param>
    public void ComboSound(int comboCount) {
        if (comboCount % 10 != 0 || comboCount < 10) return;
        comboCount /= 10;
        if (comboCount >= comboSounds.Length)
            comboCount = comboSounds.Length - 1;
        audio.PlayOneShot(comboSounds[comboCount]);
    }

    /// <summary>
    /// Plays the damage sound
    /// </summary>
    public void DamageSound() {
        audio.PlayOneShot(damage);
    }

    /// <summary>
    /// Plays the sound of button pressed
    /// </summary>
    public void ButtonSound() {
        audio.PlayOneShot(button);
    }

    /// <summary>
    /// Plays the cash register sound
    /// </summary>
    public void PurchaseSound() {
        audio.PlayOneShot(purchase);
    }

    /// <summary>
    /// Plays the shield sound
    /// </summary>
    public void ShieldSound() {
        audio.PlayOneShot(shield);
    }

    /// <summary>
    /// Plays the pause sound
    /// </summary>
    public void PauseSound() {
        audio.PlayOneShot(pause);
    }
}
