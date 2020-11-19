///-----------------------------------------------------------------
///   Author : Thibault PAGERIE                    
///   Date   : 19/02/2020 18:14
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.SoundsClass {

    [Serializable]
    public abstract class ASound
    {
        [Header("Sound Settings", order = 1)]
        public string soundName;
        public AudioClip clip;
    }
}