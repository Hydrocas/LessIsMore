///-----------------------------------------------------------------
///   Author : Thibault PAGERIE                    
///   Date   : 05/03/2020 15:58
///-----------------------------------------------------------------

using Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.Enums;
using System;
using UnityEngine;

namespace Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.SoundsClass
{
    [Serializable]
    public class UiSound : ASound
    {
        [HideInInspector] public UiSoundsEnum index;
    }
}