using UnityEngine;
using WolfeyGamedev.Pooling;

namespace GuitarHero
{
    
    public class ParticleOneShot : MonoBehaviour
    {
        [SerializeField] PooledObject pooledObject;
        
        void OnParticleSystemStopped()
        {
            pooledObject.Release();
        }
    }
}
