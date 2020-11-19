using Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.Enums;
using Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.Settings;
using Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.SoundsClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.SoundsClass.SFX;

namespace Com.Pageriethibault.Assets.Utils.Scripts.SoundManager
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundManagerSettings settings = null;
        [SerializeField] private SoundManager prefab = null;

        [Space]
        [SerializeField] private AudioSource musicsSource = null;
        [SerializeField] private AudioSource ambientsSource = null;
        [SerializeField] private AudioSource uiSource = null;

        [Space]
        [Header("Debug")]
        [SerializeField] private MusicsEnum music = default;
        [SerializeField] private AmbientSoundsEnum ambientSound = default;

        static private Dictionary<MusicsEnum, AudioClip> musicsDictionary = new Dictionary<MusicsEnum, AudioClip>();
        static private Dictionary<AmbientSoundsEnum, AudioClip> ambientsDictionary = new Dictionary<AmbientSoundsEnum, AudioClip>();
        static private Dictionary<SoundEffectsEnum, EnvironementSound> sfxDictionary = new Dictionary<SoundEffectsEnum, EnvironementSound>();
        static private Dictionary<PlayerSoundsEnum, PlayerSound> playerDictionary = new Dictionary<PlayerSoundsEnum, PlayerSound>();
        static private Dictionary<UiSoundsEnum, UiSound> uiDictionary = new Dictionary<UiSoundsEnum, UiSound>();
        static private Dictionary<SnapshotsEnum, Snapshot> snapshotDictionary = new Dictionary<SnapshotsEnum, Snapshot>();

        static private AudioClip currentMusic = null;
        static private AudioClip currentAmbient = null;
        static private GameObject soundsPool = null;
        static private SoundManager instance = null;
        private static Coroutine ambientCoroutine = null;
        private static Coroutine musicCoroutine = null;

        private float musicStartVolume = 0f;
        private float ambientStartVolume = 0f;


        private void Awake()
        {
            if (musicsDictionary.Count != 0 &&
                ambientsDictionary.Count != 0 &&
                sfxDictionary.Count != 0 &&
                playerDictionary.Count != 0 &&
                uiDictionary.Count != 0 &&
                snapshotDictionary.Count != 0)
                return;

            settings.BuildDictionnaries();
            musicsDictionary = new Dictionary<MusicsEnum, AudioClip>(settings.MusicsDictionary);
            ambientsDictionary = new Dictionary<AmbientSoundsEnum, AudioClip>(settings.AmbientsDictionary);
            sfxDictionary = new Dictionary<SoundEffectsEnum, EnvironementSound>(settings.SFXDictionary);
            playerDictionary = new Dictionary<PlayerSoundsEnum, PlayerSound>(settings.PlayerDictionary);
            uiDictionary = new Dictionary<UiSoundsEnum, UiSound>(settings.UiDictionary);
            snapshotDictionary = new Dictionary<SnapshotsEnum, Snapshot>(settings.SnapshotDictionary);
        }

        private void Start()
        {
            instance = this;
            soundsPool = new GameObject("soundsPool");
            soundsPool.transform.parent = this.transform;
            AudioSource newSource = this.gameObject.AddComponent<AudioSource>();
            musicStartVolume = musicsSource.volume;
            ambientStartVolume = ambientsSource.volume;
            musicsSource.outputAudioMixerGroup = settings.MusicAudioMixer;
            uiSource.outputAudioMixerGroup = settings.UiAudioMixer;
            ambientsSource.outputAudioMixerGroup = settings.AmbientAudioMixer;
        }

        /// <summary>
        /// Play a Sound based on a SoundEffectEnum. if position is null, the sound won't be spacialised
        /// </summary>
        /// <param name="soundEnum">the enum in the list</param>
        /// <param name="position">the position where the sound will be spacialised</param>
        static public void PlaySound(SoundEffectsEnum soundEnum, Transform position = null)
        {
            SFX sound = sfxDictionary[soundEnum];
            if (sound == null)
            {
                Debug.LogWarning("The Sound " + soundEnum.ToString() + " is null and can't be played. Please check the Settings");
                return;
            }

            PlaySoundSpacialised(sound, instance.settings.SfxAudioMixer, position);
        }

        /// <summary>
        /// Play a Sound based on a SoundEffectEnum. if position is null, the sound won't be spacialised
        /// </summary>
        /// <param name="soundEnum">the enum in the list</param>
        /// <param name="position">the position where the sound will be spacialised</param>
        static public void PlaySound(PlayerSoundsEnum soundEnum, Transform position = null)
        {
            SFX sound = playerDictionary[soundEnum];
            if (sound == null)
            {
                Debug.LogWarning("The Sound " + soundEnum.ToString() + " is null and can't be played. Please check the Settings");
                return;
            }

            PlaySoundSpacialised(sound, instance.settings.PlayerAudioMixer, position);
        }

        /// <summary>
        /// Play a Sound based on a SoundEffectEnum. if position is null, the sound won't be spacialised
        /// </summary>
        /// <param name="soundEnum">the enum in the list</param>
        static public void PlaySound(UiSoundsEnum soundEnum)
        {
            AudioClip clip = uiDictionary[soundEnum].clip;
            if (clip == null)
            {
                Debug.LogWarning("The Sound " + soundEnum.ToString() + " is null and can't be played. Please check the Settings");
                return;
            }

            instance.uiSource.clip = clip;
            instance.uiSource.Play();
        }

        /// <summary>
        /// Return the sound in the Enum list
        /// </summary>
        /// <param name="soundEnum">The enum</param>
        /// <returns></returns>
        static public AudioClip GetSoundAt(SoundEffectsEnum soundEnum)
        {
            AudioClip clip;

            if (sfxDictionary[soundEnum].useArray) clip = sfxDictionary[soundEnum].clips[0];
            else clip = sfxDictionary[soundEnum].clip;

            return clip;
        }

        static public AudioClip GetSoundAt(PlayerSoundsEnum soundEnum)
        {
            AudioClip clip;

            if (playerDictionary[soundEnum].useArray) clip = playerDictionary[soundEnum].clips[0];
            else clip = playerDictionary[soundEnum].clip;

            return clip;
        }

        static public AudioClip GetSoundAt(UiSoundsEnum soundEnum)
        {
            return uiDictionary[soundEnum].clip;
        }

        /// <summary>
        /// Play a sound based on a struct
        /// </summary>
        /// <param name="sound">the SFX struc with all parameters</param>
        /// <param name="position">the position of the sound, if null, the sound will have no spacialisation</param>
        static private void PlaySoundSpacialised(SFX sound, AudioMixerGroup audioMixerGroup, Transform position = null)
        {
            GameObject soundObject;
            AudioSource audioSource;
            if (soundsPool.transform.childCount != 0)
            {
                soundObject = soundsPool.transform.GetChild(0).gameObject;
                audioSource = soundObject.GetComponent<AudioSource>();
                soundObject.transform.parent = null;
            }

            else
            {
                soundObject = new GameObject("TempSoundPlayer");
                audioSource = soundObject.AddComponent<AudioSource>();
            }

            AudioSourceSettings audioSourceSettings = sound.audioSourceSettings;
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = audioMixerGroup;

            if (sound.useArray) audioSource.clip = GetRandomSoundInArray(sound.clips);
            else audioSource.clip = sound.clip;
            audioSource.volume = audioSourceSettings.volume;
            audioSource.pitch = audioSourceSettings.pitch;

            if (position == null)
            {
                audioSource.spatialBlend = 0f;
            }

            else
            {
                audioSource.spatialBlend = 1f;
                audioSource.dopplerLevel = audioSourceSettings.dopplerLevel;
                audioSource.spread = audioSourceSettings.spread;
                audioSource.rolloffMode = audioSourceSettings.rolloffMode;
                audioSource.minDistance = audioSourceSettings.minDistance;
                audioSource.maxDistance = audioSourceSettings.maxDistance;
                soundObject.transform.position = position.position;
            }

            audioSource.Play();
            instance.StartCoroutine(BackToPool(soundObject, audioSource.clip.length));
        }

        /// <summary>
        /// Set a new Snapshot for the master Sound
        /// </summary>
        /// <param name="snapshot">The Snapshot Enum name</param>
        /// <param name="time">Time to transition</param>
        static public void SetSnapshot(SnapshotsEnum snapshot, float time)
        {
            snapshotDictionary[snapshot].audioMixerSnapshot.TransitionTo(time);
        }

        static private AudioClip GetRandomSoundInArray(List<AudioClip> audioClips)
        {
            return audioClips[UnityEngine.Random.Range(0, audioClips.Count - 1)];
        }

        static private IEnumerator BackToPool(GameObject element, float time)
        {
            yield return new WaitForSeconds(time);
            element.transform.parent = soundsPool.transform;
        }

        /// <summary>
        /// Play a music based on an enum
        /// </summary>
        /// <param name="music"></param>
        /// <param name="fadeSound"></param>
        /// <param name="time"></param>
        public static void PlayMusic (MusicsEnum music, bool fadeSound = true, float time = 1f)
        {
            if (fadeSound)
            {
                FadeMusicIn(music, time);
                return;
            }
            AudioClip clip = musicsDictionary[music];
            if (currentMusic == clip) return;
            currentMusic = clip;
            instance.musicsSource.clip = currentMusic;
            instance.musicsSource.Play();
        }

        /// <summary>
        /// Play a music based on an enum
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="fadeSound"></param>
        /// <param name="time"></param>
        public static void PlayMusic(AmbientSoundsEnum ambient, bool fadeSound = true, float time = 1f)
        {
            if (fadeSound)
            {
                FadeMusicIn(ambient, time);
                return;
            }
            AudioClip clip = ambientsDictionary[ambient];
            if (currentMusic == clip) return;
            currentMusic = clip;
            instance.musicsSource.clip = currentMusic;
            instance.musicsSource.Play();
        }

        public static void KillMusic()
        {
            if (instance.musicsSource.isPlaying)
            {
                instance.musicsSource.Stop();
                currentMusic = null;
            }

            else Debug.LogWarning("you're trying to kill a music, but no one is playing");
        }

        public static void KillAmbiant()
        {
            if (instance.ambientsSource.isPlaying)
            {
                instance.ambientsSource.Stop();
                currentAmbient = null;
            }

            else Debug.LogWarning("you're trying to kill an ambiant, but no one is playing");
        }

        /// <summary>
        /// Fade a new music in
        /// </summary>
        /// <param name="musicsEnum">The name of the music enum</param>
        /// <param name="time">Time to fade in</param>
        public static void FadeMusicIn(MusicsEnum musicsEnum, float time = 1f)
        {
            AudioClip clip = musicsDictionary[musicsEnum];
            if (currentMusic == clip) return;
            if (musicCoroutine != null)
            {
                instance.StopCoroutine(musicCoroutine);
                currentMusic = clip;
                instance.musicsSource.clip = currentMusic;
                instance.musicsSource.volume = instance.musicStartVolume;
                instance.musicsSource.Play();
                musicCoroutine = null;
            }

            else
            {
                musicCoroutine = instance.StartCoroutine(instance.FadeMusic(time, currentMusic, clip));
                currentMusic = clip;
            }
        }

        /// <summary>
        /// Fade a new ambiant in
        /// </summary>
        /// <param name="musicsEnum">The name of the ambient enum</param>
        /// <param name="time">Time to fade in</param>
        public static void FadeMusicIn(AmbientSoundsEnum ambientenum, float time = 1f)
        {
            AudioClip clip = ambientsDictionary[ambientenum];
            if (currentAmbient == clip) return;
            if (ambientCoroutine != null)
            {
                instance.StopCoroutine(ambientCoroutine);
                currentAmbient = clip;
                instance.ambientsSource.clip = currentAmbient;
                instance.ambientsSource.volume = instance.ambientStartVolume;
                instance.ambientsSource.Play();
                ambientCoroutine = null;
            }

            else
            {
                ambientCoroutine = instance.StartCoroutine(instance.FadeAmbient(time, currentAmbient, clip));
                currentAmbient = clip;
            }
        }

        private IEnumerator FadeMusic(float time, AudioClip aClip = null, AudioClip bClip = null)
        {
            AudioClip secondClip = bClip;
            float elapsedTime = 0f;
            float startVolume = musicsSource.volume;

            if (aClip == null)
            {
                musicsSource.clip = bClip;
                musicsSource.Play();
                while(elapsedTime < time)
                {
                    elapsedTime += Time.deltaTime;
                    musicsSource.volume = Mathf.Lerp(0f, startVolume, elapsedTime / time);
                    yield return null;
                }
                musicsSource.volume = startVolume;
            }

            else if (bClip = null)
            {
                while (elapsedTime < time)
                {
                    elapsedTime += Time.deltaTime;
                    musicsSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / time);
                    yield return null;
                }
                musicsSource.Stop();
                musicsSource.volume = startVolume;
            }

            else
            {
                GameObject soundObject;
                AudioSource audioSource;
                soundObject = new GameObject("TempFadeOut");
                audioSource = soundObject.AddComponent<AudioSource>();

                //audioSource.GetCopyOf(source);
                
                audioSource.outputAudioMixerGroup = musicsSource.outputAudioMixerGroup;
                audioSource.volume = musicsSource.volume;
                audioSource.clip = musicsSource.clip;
                audioSource.playOnAwake = musicsSource.playOnAwake;
                audioSource.loop = musicsSource.loop;
                audioSource.priority = musicsSource.priority;
                audioSource.pitch = musicsSource.pitch;
                audioSource.Play();
                audioSource.time = musicsSource.time;
                musicsSource.clip = secondClip;
                musicsSource.volume = 0f;
                musicsSource.Play();

                while (elapsedTime < time)
                {
                    elapsedTime += Time.deltaTime;
                    audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / time);
                    musicsSource.volume = Mathf.Lerp(0f, startVolume, elapsedTime / time);
                    yield return null;
                }
                musicsSource.volume = startVolume;
                Destroy(soundObject);
            }

                musicCoroutine = null;
        }

        private IEnumerator FadeAmbient(float time, AudioClip aClip = null, AudioClip bClip = null)
        {
            AudioClip secondClip = bClip;
            float elapsedTime = 0f;
            float startVolume = ambientsSource.volume;

            if (aClip == null)
            {
                ambientsSource.clip = bClip;
                ambientsSource.Play();
                while (elapsedTime < time)
                {
                    elapsedTime += Time.deltaTime;
                    ambientsSource.volume = Mathf.Lerp(0f, startVolume, elapsedTime / time);
                    yield return null;
                }
                ambientsSource.volume = startVolume;
            }

            else if (bClip = null)
            {
                while (elapsedTime < time)
                {
                    elapsedTime += Time.deltaTime;
                    ambientsSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / time);
                    yield return null;
                }
                ambientsSource.Stop();
                ambientsSource.volume = startVolume;
            }

            else
            {
                GameObject soundObject;
                AudioSource audioSource;
                soundObject = new GameObject("TempFadeOut");
                audioSource = soundObject.AddComponent<AudioSource>();

                //audioSource.GetCopyOf(source);

                audioSource.outputAudioMixerGroup = ambientsSource.outputAudioMixerGroup;
                audioSource.volume = ambientsSource.volume;
                audioSource.clip = ambientsSource.clip;
                audioSource.playOnAwake = ambientsSource.playOnAwake;
                audioSource.loop = ambientsSource.loop;
                audioSource.priority = ambientsSource.priority;
                audioSource.pitch = ambientsSource.pitch;
                audioSource.Play();
                audioSource.time = ambientsSource.time;
                ambientsSource.clip = secondClip;
                ambientsSource.volume = 0f;
                ambientsSource.Play();

                while (elapsedTime < time)
                {
                    elapsedTime += Time.deltaTime;
                    audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / time);
                    ambientsSource.volume = Mathf.Lerp(0f, startVolume, elapsedTime / time);
                    yield return null;
                }
                ambientsSource.volume = startVolume;
                Destroy(soundObject);
            }

            ambientCoroutine = null;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        //===================================DEBUG==========================================

        public void DebugPlayMusic()
        {
            FadeMusicIn(music);
        }

        public void DebugPlayAmbient()
        {
            FadeMusicIn(ambientSound);
        }
    }
}
