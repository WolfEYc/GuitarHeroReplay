using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GuitarHero
{
    [RequireComponent(typeof(AudioSource))]
    public class GuitarRecorder : MonoBehaviour
    {
        [SerializeField] GuitarPlayer player;
        [SerializeField] InputAction[] guitarInputs;
        [SerializeField] InputAction recordInput;
        
        [SerializeField] GuitarString[] guitarStrings;

        AudioSource _audioSource;
        
        public bool Recording { get; private set; }

        public UnityEvent onRecordingStarted, onRecordingStopped;

        void Awake()
        {
            for (int i = 0; i < guitarInputs.Length; i++)
            {
                guitarInputs[i].performed += guitarStrings[i].GuitarInput;
                guitarInputs[i].Enable();
            }

            recordInput.performed += _ => RecordToggle();
            recordInput.Enable();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = player.replay.song;
        }

        void RecordToggle()
        {
            
            if (Recording)
            {
                StopRecording();
                
            }
            else
            {
                StartRecording();
            }
        }

        public void StopRecording()
        {
            if(!Recording) return;
            onRecordingStopped.Invoke();
            SaveImpacts();
            _audioSource.Stop();
            Recording = false;
        }

        public void StartRecording()
        {
            if(Recording) return;
            onRecordingStarted.Invoke();
            _audioSource.Play();
            Recording = true;
        }
        
        public void SaveImpacts()
        {
            for (int i = 0; i < guitarStrings.Length; i++)
            {
                player.replay.SaveImpacts(i, guitarStrings[i].recordedImpacts);
            }
        }
    }
}
