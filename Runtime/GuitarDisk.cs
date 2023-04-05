using UnityEngine;
using Wolfey.Extensions;
using WolfeyGamedev.Pooling;

namespace GuitarHero
{
    [RequireComponent(typeof(PooledObject))]
    public class GuitarDisk : MonoBehaviour
    {
        GuitarString _guitarString;

        PooledObject _pooledObj;
        Transform _transform;
        float _hitTime;
        bool _impacted;
        float _startTime;
        
        void Awake()
        {
            _transform = transform;
            _pooledObj = GetComponent<PooledObject>();
        }

        public void DiskSpawned(float hitTime, GuitarString guitarString)
        {
            _hitTime = hitTime;
            _guitarString = guitarString;
            _startTime = _guitarString.LocalTime;
            _impacted = false;
        }

        void Update()
        {
            
            float lerpTime = _guitarString.LocalTime.Remap(_startTime , _hitTime, 0f, 1f);

            if (!_impacted && _hitTime - _guitarString.LocalTime < _guitarString.impactDelay)
            {
                _impacted = true;
                _guitarString.impactEffectTrigger.Invoke();
            }
            
            if (lerpTime < 1f)
            {
                _transform.position = Vector3.Lerp(
                    _guitarString.startTransform.position,
                    _guitarString.endTransform.position,
                    lerpTime
                );
                
                return;
            }
            
            _guitarString.instantImpactEffectTrigger.Invoke();
            Stop();
        }

        public void Stop()
        {
            _pooledObj.Release();
            _guitarString.OnStop -= Stop;
        }
    }
}
