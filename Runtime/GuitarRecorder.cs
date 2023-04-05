using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GuitarHero
{
    [RequireComponent(typeof(AudioSource))]
    public class GuitarRecorder : MonoBehaviour
    {
        [SerializeField] GuitarReplay replaySave;
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
            _audioSource.clip = replaySave.song;
        }

        void RecordToggle()
        {
            Recording = !Recording;
            if (Recording)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }

        public void StopRecording()
        {
            if(!Recording) return;
            onRecordingStopped.Invoke();
            SaveImpacts();
            _audioSource.Stop();
        }

        public void StartRecording()
        {
            if(Recording) return;
            onRecordingStarted.Invoke();
            _audioSource.Play();
        }
        
        public void SaveImpacts()
        {
            for (int i = 0; i < guitarStrings.Length; i++)
            {
                replaySave.SaveImpacts(i, guitarStrings[i].recordedImpacts);
            }
        }
    }
}
