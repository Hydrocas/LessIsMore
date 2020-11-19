///-----------------------------------------------------------------
///   Author : Thibault PAGERIE                    
///   Date   : 19/02/2020 23:02
///-----------------------------------------------------------------

using Com.Pageriethibault.Assets.Utils.Scripts.SoundManager;
using UnityEditor;
using UnityEngine;

namespace Com.Pageriethibault.Assets.Utils{

    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerScriptEditor : Editor {

        public override void OnInspectorGUI()
        {
            SoundManager myScript = (SoundManager)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Test Music"))
            {
                myScript.DebugPlayMusic();
            }

            if (GUILayout.Button("Test Ambient"))
            {
                myScript.DebugPlayAmbient();
            }
        }
    }
}