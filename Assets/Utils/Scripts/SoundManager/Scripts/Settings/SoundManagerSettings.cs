///-----------------------------------------------------------------
///   Author : Thibault PAGERIE                    
///   Date   : 19/02/2020 18:19
///-----------------------------------------------------------------

using Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.Enums;
using Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.SoundsClass;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.Settings
{

    [CreateAssetMenu(fileName = "SoundManagerSettings", menuName = "Utils/SoundManagerSettings")]
    public class SoundManagerSettings : ScriptableObject {
        [SerializeField] private string musicsEnumName = "";
        [SerializeField] private string ambientEnumName = "";
        [SerializeField] private string sfxEnumName = "";
        [SerializeField] private string playerEnumName = "";
        [SerializeField] private string uiEnumName = "";
        [SerializeField] private string snapshotsEnumName = "";
        [SerializeField] private string directoryPath = "";

        [Space]
        [Header("General Settings")]
        [SerializeField] private AudioMixerGroup masterAudioMixer = null;
        [SerializeField] private List<Snapshot> snapshots = null;

        [Space]
        [Header("Sounds Lists")]
        [SerializeField] private AudioMixerGroup musicAudioMixer = null;
        [SerializeField] private List<Music> musics = null;
        [SerializeField] private AudioMixerGroup ambientAudioMixer = null;
        [SerializeField] private List<AmbientSound> ambientSounds = null;

        [Space]
        [Header("SFX List")]
        [SerializeField] private AudioMixerGroup sfxAudioMixer = null;
        [SerializeField] private List<EnvironementSound> environementSounds = null;

        [Space]
        [Header("Player List")]
        [SerializeField] private AudioMixerGroup playerAudioMixer = null;
        [SerializeField] private List<PlayerSound> playerSounds = null;

        [Space]
        [Header("UI List")]
        [SerializeField] private AudioMixerGroup uiAudioMixer = null;
        [SerializeField] private List<UiSound> uiSounds = null;

        private bool changeInEnums = false;

        const string ENUMNAMESPACE = "Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.Enums";

        private Dictionary<MusicsEnum, AudioClip> musicsDictionary = new Dictionary<MusicsEnum, AudioClip>();
        private Dictionary<AmbientSoundsEnum, AudioClip> ambientsDictionary = new Dictionary<AmbientSoundsEnum, AudioClip>();
        private Dictionary<SoundEffectsEnum, EnvironementSound> sfxDictionary = new Dictionary<SoundEffectsEnum, EnvironementSound>();
        private Dictionary<PlayerSoundsEnum, PlayerSound> playerDictionary = new Dictionary<PlayerSoundsEnum, PlayerSound>();
        private Dictionary<UiSoundsEnum, UiSound> uiDictionary = new Dictionary<UiSoundsEnum, UiSound>();
        private Dictionary<SnapshotsEnum, Snapshot> snapshotDictionary = new Dictionary<SnapshotsEnum, Snapshot>();

        public Dictionary<MusicsEnum, AudioClip> MusicsDictionary => musicsDictionary;
        public Dictionary<AmbientSoundsEnum, AudioClip> AmbientsDictionary => ambientsDictionary;
        public Dictionary<SoundEffectsEnum, EnvironementSound> SFXDictionary => sfxDictionary;
        public Dictionary<PlayerSoundsEnum, PlayerSound> PlayerDictionary => playerDictionary;
        public Dictionary<UiSoundsEnum, UiSound> UiDictionary => uiDictionary;
        public Dictionary<SnapshotsEnum, Snapshot> SnapshotDictionary => snapshotDictionary;

        public AudioMixerGroup SfxAudioMixer => sfxAudioMixer;
        public AudioMixerGroup PlayerAudioMixer  => playerAudioMixer;
        public AudioMixerGroup UiAudioMixer => uiAudioMixer;
        public AudioMixerGroup MusicAudioMixer => musicAudioMixer;
        public AudioMixerGroup AmbientAudioMixer => ambientAudioMixer;
        public AudioMixerGroup MasterAudioMixer => masterAudioMixer;

        private void OnEnable()
        {
            if (!changeInEnums) return;
            changeInEnums = false;
            ParseEnum(musics);
            ParseEnum(ambientSounds);
            ParseEnum(environementSounds);
            ParseEnum(uiSounds);
            ParseEnum(playerSounds);
            ParseEnum(snapshots);
        }

#if UNITY_EDITOR
        public void SetEnums()
        {
            CreateEnum(musics, musicsEnumName);
            CreateEnum(ambientSounds, ambientEnumName);
            CreateEnum(environementSounds, sfxEnumName);
            CreateEnum(uiSounds, uiEnumName);
            CreateEnum(playerSounds, playerEnumName);
            CreateEnum(snapshots, snapshotsEnumName);

            AssetDatabase.Refresh();
            changeInEnums = true;
        }

        //-------------------------------------------------------------------//
        //                      CreateEnum overloads                         //
        //-----------------------------------------------------------------//

        private void CreateEnum (List <Music> list, string name)
        {
            List<string> enums = new List<string>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                enums.Add(list[i].soundName);
            }
            GenerateEnum.Generate(name, enums, directoryPath, ENUMNAMESPACE, false);
        }
        private void CreateEnum(List<AmbientSound> list, string name)
        {
            List<string> enums = new List<string>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                enums.Add(list[i].soundName);
            }
            GenerateEnum.Generate(name, enums, directoryPath, ENUMNAMESPACE, false);
        }
        private void CreateEnum(List<EnvironementSound> list, string name)
        {
            List<string> enums = new List<string>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                enums.Add(list[i].soundName);
            }
            GenerateEnum.Generate(name, enums, directoryPath, ENUMNAMESPACE,false);
        }
        private void CreateEnum(List<UiSound> list, string name)
        {
            List<string> enums = new List<string>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                enums.Add(list[i].soundName);
            }
            GenerateEnum.Generate(name, enums, directoryPath, ENUMNAMESPACE,false);
        }
        private void CreateEnum(List<PlayerSound> list, string name)
        {
            List<string> enums = new List<string>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                enums.Add(list[i].soundName);
            }
            GenerateEnum.Generate(name, enums, directoryPath, ENUMNAMESPACE,false);
        }
        private void CreateEnum(List<Snapshot> list, string name)
        {
            List<string> enums = new List<string>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                enums.Add(list[i].snapshotName);
            }
            GenerateEnum.Generate(name, enums, directoryPath, ENUMNAMESPACE,false);
        }

#endif

        //-------------------------------------------------------------------//
        //                      ParseEnum overloads                         //
        //-----------------------------------------------------------------//


        private void ParseEnum(List<Music> myMusics)
        {

            for (int i = myMusics.Count - 1; i >= 0; i--)
            {
                myMusics[i].index = (MusicsEnum)Enum.Parse(typeof(MusicsEnum), GenerateEnum.CleanString(myMusics[i].soundName));
            }
            Debug.Log(musicsEnumName + " is pased with a total of " + myMusics.Count + " elements.");
        }
        private void ParseEnum(List<AmbientSound> myAmbient)
        {

            for (int i = myAmbient.Count - 1; i >= 0; i--)
            {
                myAmbient[i].index = (AmbientSoundsEnum)Enum.Parse(typeof(AmbientSoundsEnum), GenerateEnum.CleanString(myAmbient[i].soundName));
            }
            Debug.Log(ambientEnumName + " is pased with a total of " + myAmbient.Count + " elements.");
        }
        private void ParseEnum(List<EnvironementSound> sfx)
        {
            for (int i = sfx.Count - 1; i >= 0; i--)
            {
                sfx[i].index = (SoundEffectsEnum)Enum.Parse(typeof(SoundEffectsEnum), GenerateEnum.CleanString(sfx[i].soundName));
            }
            Debug.Log(sfxEnumName + " is pased with a total of " + sfx.Count + " elements.");
        }
        private void ParseEnum(List<UiSound> uis)
        {
            for (int i = uis.Count - 1; i >= 0; i--)
            {
                uis[i].index = (UiSoundsEnum)Enum.Parse(typeof(UiSoundsEnum), GenerateEnum.CleanString(uis[i].soundName));
            }
            Debug.Log(uiEnumName + " is pased with a total of " + uis.Count + " elements.");
        }
        private void ParseEnum(List<PlayerSound> playerSounds)
        {
            for (int i = playerSounds.Count - 1; i >= 0; i--)
            {
                playerSounds[i].index = (PlayerSoundsEnum)Enum.Parse(typeof(PlayerSoundsEnum), GenerateEnum.CleanString(playerSounds[i].soundName));
            }
            Debug.Log(playerEnumName + " is pased with a total of " + playerSounds.Count + " elements.");
        }
        private void ParseEnum(List<Snapshot> snapshots)
        {
            for (int i = snapshots.Count - 1; i >= 0; i--)
            {
                snapshots[i].index = (SnapshotsEnum)Enum.Parse(typeof(SnapshotsEnum), GenerateEnum.CleanString(snapshots[i].snapshotName));
            }
            Debug.Log(snapshotsEnumName + " is pased with a total of " + snapshots.Count + " elements.");
        }

        public void BuildDictionnaries()
        {
            int i;
            for (i = musics.Count - 1; i >= 0; i--)
            {
                musicsDictionary.Add(musics[i].index, musics[i].clip);
            }

            for (i = ambientSounds.Count - 1; i >= 0; i--)
            {
                ambientsDictionary.Add(ambientSounds[i].index, ambientSounds[i].clip);
            }

            for (i = environementSounds.Count - 1; i >= 0; i--)
            {
                sfxDictionary.Add(environementSounds[i].index, environementSounds[i]);
            }

            for (i = playerSounds.Count - 1; i >= 0; i--)
            {
                playerDictionary.Add(playerSounds[i].index, playerSounds[i]);
            }

            for (i = uiSounds.Count - 1; i >= 0; i--)
            {
                uiDictionary.Add(uiSounds[i].index, uiSounds[i]);
            }

            for (i = snapshots.Count - 1; i >= 0; i--)
            {
                snapshotDictionary.Add(snapshots[i].index, snapshots[i]);
            }
        }
    }
}