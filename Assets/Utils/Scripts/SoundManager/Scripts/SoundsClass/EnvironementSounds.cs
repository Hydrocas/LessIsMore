///-----------------------------------------------------------------
///   Author : Thibault PAGERIE                    
///   Date   : 04/03/2020 15:48
///-----------------------------------------------------------------

using Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.Enums;
using System;
using UnityEngine;

namespace Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.SoundsClass
{
    [Serializable]
    public class EnvironementSound : SFX
    {
        [HideInInspector] public SoundEffectsEnum index;
    }
}