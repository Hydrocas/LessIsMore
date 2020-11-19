///-----------------------------------------------------------------
/// Author : Luca GERBER
/// Date : 21/01/2020 16:36
///-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Common
{
    public class TimeManager : MonoBehaviour {

        [HideInInspector] public float timeRatio = 0;

        [Header("Slow Motion")]
        [SerializeField, Range(.03f, .2f)] private float slowFactor = .05f;

        private float timer = 0;
        private float elapsedTime = 0;
        private float accelerationElapsedTime = 0;
        private bool isWaiting = false;

        // event cast every secounds
        public event Action OnTick;
        public event Action OnAccelerationTick;

        [Header("Tick")]
        [SerializeField, Range(.1f, 2f)] private float speed = 1;
        [SerializeField, Range(.3f, 3f)] private float tickTime = 1f;
        [SerializeField, Range(0, 100)] private float timeBeforeAcceleration = 10f;

        //[Space]
        //[SerializeField] private int nMaxAcceleration = 10;

        public static TimeManager Instance => instance;
        private static TimeManager instance = null;

        private Action DoAction;

        //private bool isFTUE = false;
        //public int nAcceleration = 0;

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        private void Start()
        {
            SetModeVoid();
        }


        private void Update()
        {
            DoAction();
        }

        private void SetModeVoid()
        {
            DoAction = DoActionVoid;
            elapsedTime = 0;
            accelerationElapsedTime = 0;
        }

        private void DoActionVoid() { }

        private void SetModePlay()
        {
            DoAction = DoActionPlay;
        }

        private void DoActionPlay()
        {
            timer += Time.unscaledDeltaTime;
            elapsedTime += Time.deltaTime * speed;

            if (elapsedTime >= tickTime)
            {
                elapsedTime = 0;

                OnTick?.Invoke();
            }

            timeRatio = elapsedTime / tickTime;
        }


        private void AccelerationTime()
        {
            accelerationElapsedTime += Time.deltaTime * speed;

            if(accelerationElapsedTime >= timeBeforeAcceleration)
            {
                accelerationElapsedTime = 0;
                //nAcceleration++;
                OnAccelerationTick?.Invoke();
            }
        }

        public void DoSlowMotion(float duration = 0, float slowStrength = 0)
        {
            float lSlowFactor = slowStrength != 0 ? slowStrength : slowFactor;
            // 20 times slower == t * .05 <=> t / 20  
            Time.timeScale = lSlowFactor;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            if (duration != 0) Invoke("DoNormalMotion", duration * Time.unscaledDeltaTime);
        }
        public void DoNormalMotion()
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        public void HitStop(float duration)
        {
            if (isWaiting) return;

            Time.timeScale = 0;

            StartCoroutine(Wait(duration));
        }

        private IEnumerator Wait(float duration)
        {
            isWaiting = true;

            yield return new WaitForSecondsRealtime(duration);

            Time.timeScale = 1;
            isWaiting = false;
        }

        private void OnDestroy()
        {
            if (this == instance) instance = null;
        }
    }
}