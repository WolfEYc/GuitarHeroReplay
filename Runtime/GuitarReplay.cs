using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GuitarHero
{
    [CreateAssetMenu(order = 0, menuName = "GuitarHero/GuitarReplay", fileName = "NewGuitarReplay")]
    public class GuitarReplay : ScriptableObject
    {
        public AudioClip song;
        [SerializeField] GuitarReplayData replayData = new();
        
        
        string FilePath => Path.Join(Application.persistentDataPath, name);
        
        public float[] GetImpacts(int idx)
        {
            return idx switch
            {
                0 => replayData.impacts0,
                1 => replayData.impacts1,
                2 => replayData.impacts2,
                3 => replayData.impacts3,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void SaveImpacts(int idx, List<float> impacts)
        {
            var impactArray = impacts.ToArray();
            switch (idx)
            {
                case 0:
                    replayData.impacts0 = impactArray;
                    break;
                case 1:
                    replayData.impacts1 = impactArray;
                    break;
                case 2:
                    replayData.impacts2 = impactArray;
                    break;
                case 3:
                    replayData.impacts3 = impactArray;
                    break;
            }
        }
        
        void SaveReplayData()
        {
            File.WriteAllText(
                FilePath,
                JsonUtility.ToJson(replayData));
        }
        void LoadReplayData()
        {
            try
            {
                replayData = JsonUtility.FromJson<GuitarReplayData>(File.ReadAllText(FilePath));
            }
            catch (FileNotFoundException)
            {
                Debug.Log($"Replay file not found for {name}, creating new file");
                SaveReplayData();
            }
        }

        void OnEnable()
        {
            if (name.Length > 0f)
            {
                LoadReplayData();
            }
        }

        void OnDisable()
        {
            if (name.Length > 0f)
            {
                SaveReplayData();
            }
        }
        
        
    }

    [Serializable]
    public class GuitarReplayData
    {
        public float[] impacts0;
        public float[] impacts1;
        public float[] impacts2;
        public float[] impacts3;
    }
}
