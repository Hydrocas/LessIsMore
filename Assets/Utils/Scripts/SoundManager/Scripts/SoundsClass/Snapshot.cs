///-----------------------------------------------------------------
///   Author : Thibault PAGERIE                    
///   Date   : 06/03/2020 19:11
///-----------------------------------------------------------------

using Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.Enums;
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.SoundsClass
{
    [Serializable]
    public class Snapshot {
        public string snapshotName;
        public AudioMixerSnapshot audioMixerSnapshot;

        [HideInInspector] public SnapshotsEnum index;
    }
}