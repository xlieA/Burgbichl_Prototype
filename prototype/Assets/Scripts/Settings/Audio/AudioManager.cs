using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private AudioSource bgm;
    private AudioSource sfxButton;
    private AudioSource sfxShovel;
    private AudioSource sfxPickaxe;
    private AudioSource sfxShine;
    private AudioSource sfxRing;
    private AudioSource sfxTalking;

    public AudioMixer mixer;

    //Music Volume

    public float volumeMaster = 0.7f;
    public float volumeMusic = 0.7f;
    public float volumeSfx = 0.7f;

    private static AudioManager _instance = null;
    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        ChangeVolumeWithValues(volumeMaster, volumeMusic, volumeSfx);
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        bgm = this.GetComponents<AudioSource>()[0];
        sfxButton = this.GetComponents<AudioSource>()[1];
        sfxShovel = this.GetComponents<AudioSource>()[2];
        sfxPickaxe = this.GetComponents<AudioSource>()[3];
        sfxShine = this.GetComponents<AudioSource>()[4];
        sfxRing = this.GetComponents<AudioSource>()[5];
        sfxTalking = this.GetComponents<AudioSource>()[6];

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == "Intro")
        {
            // Stop background music only in the Intro scene
            if (bgm.isPlaying)
            {
                bgm.Stop();
            }

            sfxRing.loop = true;
            sfxRing.Play();
        }
        else
        {
            // Only play background music if it's not already playing
            if (!bgm.isPlaying)
            {
                bgm.loop = true;
                bgm.Play();
            }

            if (scene.name != "Intro" && sfxRing.isPlaying)
            {
                sfxRing.Stop();
            }
        }
    }

    private void Start()
    {
        //set volume
        StartCoroutine(SetVolume());
    }

    public void ChangeVolumeSettings()
    {
        SetMasterVolume(volumeMaster);
        SetMusicVolume(volumeMusic);
        SetSfxVolume(volumeSfx);

        SaveVolume();
    }

    public void ChangeVolumeWithValues(float masterVol, float musicVol, float sfxVol)
    {
        SetMasterVolume(masterVol);
        SetMusicVolume(musicVol);
        SetSfxVolume(sfxVol);

        SaveVolume();
    }

    private void SetMasterVolume(float value)
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(value + float.Epsilon) * 40);
        volumeMaster = value;
    }

    private void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(value + float.Epsilon) * 40);
        volumeMusic = value;
    }

    private void SetSfxVolume(float value)
    {
        mixer.SetFloat("SfxVol", Mathf.Log10(value + float.Epsilon) * 40);
        volumeSfx = value;
    }

    public void SetSavedVolume()
    {
        StartCoroutine(SetVolume());
    }

    private IEnumerator SetVolume()
    {
        yield return new WaitForEndOfFrame();
        if (!GetSavedVolume())
        {
            yield return new WaitForEndOfFrame();
            mixer.SetFloat("MasterVol", Mathf.Log10(volumeMaster + float.Epsilon) * 40);
            mixer.SetFloat("MusicVol", Mathf.Log10(volumeMusic + float.Epsilon) * 40);
            mixer.SetFloat("SfxVol", Mathf.Log10(volumeSfx + float.Epsilon) * 40);
        }

        SaveVolume();
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("MasterVolume", volumeMaster);
        PlayerPrefs.SetFloat("MusicVolume", volumeMusic);
        PlayerPrefs.SetFloat("SfxVolume", volumeSfx);

        PlayerPrefs.Save();
    }

    public bool GetSavedVolume()
    {
        bool saveExists = false;

        if (PlayerPrefs.HasKey("MasterVolume") && PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("SfxVolume"))
        {
            saveExists = true;
            volumeMaster = PlayerPrefs.GetFloat("MasterVolume");
            volumeMusic = PlayerPrefs.GetFloat("MusicVolume");
            volumeSfx = PlayerPrefs.GetFloat("SfxVolume");
        }

        return saveExists;
    }

    public void Button()
    {
        sfxButton.Play();
    }

    public void Shovel()
    {
        sfxShovel.Play();
    }

    public void Pickaxe()
    {
        sfxPickaxe.Play();
    }

    public void Shine()
    {
        sfxShine.Play();
    }

    public void StopRing()
    {
        sfxRing.Stop();
    }

    public void StartTalking()
    {
        sfxTalking.Play();
    }

    public void StopTalking()
    {
        sfxTalking.Stop();
    }

    public bool IsTalking()
    {
        return sfxTalking ? true : false;
    }
}

