///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 16/11/2019 08:36
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Common {
    public delegate void ColliderChildEvent(Collider other);

    public class ColliderChild : MonoBehaviour {

        public event ColliderChildEvent OnCollisionTrigger;
        private void OnTriggerEnter(Collider other)
        {
            OnCollisionTrigger?.Invoke(other);
        }
    }
}