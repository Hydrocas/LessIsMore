///-----------------------------------------------------------------
///   Author : Thibault PAGERIE                    
///   Date   : 19/02/2020 19:30
///-----------------------------------------------------------------

using Com.Pageriethibault.Assets.Utils.Scripts.SoundManager.Settings;
using UnityEditor;
using UnityEngine;

namespace Com.Pageriethibault.Assets.Utils
{
    [CustomEditor(typeof(SoundManagerSettings))]
    public class SoundManagerSettingsScriptEditor : Editor {

#if UNITY_EDITOR

        public override void OnInspectorGUI()
        {
            SoundManagerSettings myScript = (SoundManagerSettings)target;
            DrawDefaultInspector();

            if (GUILayout.Button("Build Enums"))
            {
                myScript.SetEnums();
            }
        }

#endif
    }
}