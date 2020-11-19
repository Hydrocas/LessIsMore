///-----------------------------------------------------------------
///   Author : Thibault PAGERIE                    
///   Date   : 19/02/2020 18:14
///-----------------------------------------------------------------

using Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.Enums;
using System;
using UnityEngine;

namespace Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.SoundsClass
{
    [Serializable]
    public class Music : ASound{
        [HideInInspector] public MusicsEnum index;
    }
}