using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using WolfeyGamedev.Pooling;

namespace GuitarHero
{
    public class GuitarString : MonoBehaviour
    {
        [SerializeField] GameObjectPool diskPool;
        [SerializeField] GameObjectPool impactParticlePool;
        [SerializeField] GuitarRecorder recorder;
        
        public float impactDelay;
        
        public Transform startTransform;
        public Transform endTransform;
        
        public UnityEvent impactEffectTrigger;
        public UnityEvent instantImpactEffectTrigger;

        public event Action OnStop;

        public List<float> recordedImpacts;
        
        float[] _impacts;
        float _startTime;
        float _impactDelay;
        int _currentImpactIdx;
        
        public float LocalTime => Time.time - _startTime;
        float NextImpactTime => _impacts[_currentImpactIdx];
        
        
        void Awake()
        {
            enabled = false;
            recorder.onRecordingStarted.AddListener(RecordingStarted);
            impactEffectTrigger.AddListener(SpawnImpactParticles);
        }

        void SpawnImpactParticles()
        {
            impactParticlePool.Get().Transform.position = endTransform.position;
        }
        
        void FixedUpdate()
        {
            if(!ShouldSpawn(NextImpactTime)) return;
            
            SpawnDisk(NextImpactTime);
        }

        bool ShouldSpawn(float impactTime)
        {
            return LocalTime + _impactDelay > impactTime;
        }
        
        void SpawnDisk(float impactTime)
        {
            var pooledDisk = diskPool.Get().GetComponent<GuitarDisk>();
            pooledDisk.DiskSpawned(impactTime - impactDelay, this);

            OnStop += pooledDisk.Stop;
            
            _currentImpactIdx++;
            
            if(_currentImpactIdx < _impacts.Length) return;
            enabled = false;
        }
        
        void RecordingStarted()
        {
            recordedImpacts.Clear();
            _startTime = Time.time;
        }
        
        public void Play(float[] impacts, float newimpactDelay)
        {
            if (impacts.Length == 0)
            {
                Debug.Log("No impacts on this guitar string", this);
                return;
            }
            
            _currentImpactIdx = 0;
            _startTime = Time.time;
            _impactDelay = newimpactDelay;
            _impacts = impacts;
            enabled = true;
        }
        
        public void Stop()
        {
            enabled = false;
            OnStop?.Invoke();
        }
        
        public void GuitarInput(InputAction.CallbackContext ctx)
        {
            impactEffectTrigger.Invoke();

            if (recorder.Recording)
            {
                recordedImpacts.Add(LocalTime);
            }
        }
    }
}
