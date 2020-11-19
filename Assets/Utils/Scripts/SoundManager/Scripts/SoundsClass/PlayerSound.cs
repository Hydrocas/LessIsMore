///-----------------------------------------------------------------
///   Author : Thibault PAGERIE                    
///   Date   : 04/03/2020 19:57
///-----------------------------------------------------------------

using Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.Enums;
using System;
using UnityEngine;

namespace Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.SoundsClass
{
    [Serializable]
    public class PlayerSound : SFX {

        [HideInInspector] public PlayerSoundsEnum index;
    }
}