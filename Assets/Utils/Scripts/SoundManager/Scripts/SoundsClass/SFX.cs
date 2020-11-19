///-----------------------------------------------------------------
///   Author : Thibault PAGERIE                    
///   Date   : 04/03/2020 15:48
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.SoundsClass
{
    [Serializable]
    public class SFX : ASound {

        [Serializable]
        public class AudioSourceSettings
        {
            [Range(0f, 1f)] public float volume = 1f;
            [Range(0f, 256)] public int priority = 128;
            [Range(-3f, 3f)] public float pitch = 0f;

            [Header("3D Sounds Settings")]
            [Range(0f, 5f)] public float dopplerLevel;
            [Range(0, 360)] public int spread;
            public AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;
            [Min(1f)] public float minDistance = 1f;
            [Min(1f)] public float maxDistance = 500F;
        }


        public bool useArray = false;
        public List<AudioClip> clips;

        public AudioSourceSettings audioSourceSettings;
    }
}