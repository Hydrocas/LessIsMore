///-----------------------------------------------------------------
/// Author : Luca GERBER
/// Date : 04/12/2019 16:08
///-----------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace Com.IsartDigital.Common {
    /// <summary>
    /// Mettre la camera dans un GameObject vide & faire tous les d�placement sur ce CameraHolder
    /// </summary>
    public class CameraEffect : MonoBehaviour
    {
        [Header("KnockbackEffect")]
        [SerializeField] private float knockbackStrenght = 10f;
        [SerializeField] private float knockbackReturnStrenght = 20f;

        private Coroutine shakeCoroutine = null;
        private Coroutine knockbackCoroutine = null;

        private Vector3 originalPos = Vector3.zero;

        private void Start()
        {
            originalPos = transform.localPosition;
        }
        public void DoScreenShake(float duration, float strength)
        {
            if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(Shake(duration,strength));
        }

        public void DoKnockback(Vector2 direction)
        {
            if (knockbackCoroutine != null) StopCoroutine(knockbackCoroutine);
            knockbackCoroutine = StartCoroutine(Knockback(direction));
        }

        private IEnumerator Shake(float duration, float strength)
        {
            float elapsed = 0f;

            while(elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * strength;
                float y = Random.Range(-1f, 1f) * strength;

                transform.localPosition = new Vector3(x, y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.localPosition = originalPos;

            shakeCoroutine = null;
        }

        private IEnumerator Knockback(Vector2 direction)
        {
            Vector3 targetPos = new Vector3(originalPos.x + direction.x, originalPos.y + direction.y, originalPos.z);


            while(Vector3.Distance(transform.localPosition, targetPos)>=0.001f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, knockbackStrenght * Time.deltaTime);

                yield return null;
            }

            while (Vector3.Distance(transform.localPosition, originalPos) >=0.001f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPos, knockbackReturnStrenght * Time.deltaTime);

                yield return null;
            }

            transform.localPosition = originalPos;

            knockbackCoroutine = null;
        }
    }
}