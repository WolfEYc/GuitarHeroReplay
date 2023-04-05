using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GuitarHero
{
    [RequireComponent(typeof(AudioSource))]
    public class GuitarPlayer : MonoBehaviour
    {
        [SerializeField] GuitarReplay replay;
        [SerializeField] float impactDelay;
        [SerializeField] GuitarString[] guitarStrings;
        [SerializeField] InputAction playAction;
        
        public UnityEvent onPlay, onStop;

        AudioSource _audioSource;
        
        void Awake()
        {
            playAction.performed += _ => Play();
            playAction.Enable();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = replay.song;
        }

        public void Play()
        {
            onPlay.Invoke();
            for(int i = 0; i < guitarStrings.Length; i++)
            {
                guitarStrings[i].Play(replay.GetImpacts(i), impactDelay);
            }
            _audioSource.Play();
        }

        public void Stop()
        {
            for(int i = 0; i < guitarStrings.Length; i++)
            {
                guitarStrings[i].Stop();
            }
            onStop.Invoke();
            _audioSource.Stop();
        }
    }
}
