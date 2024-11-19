using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DistractorProject.UserStudy
{
    public class UserStudySetup : MonoBehaviour
    {
        public Study[] trials = Array.Empty<Study>();
        
        
        private void Awake()
        {
            for (var i = 0; i < trials.Length; i++)
            {
                var trial = trials[i];
                var noiseLevelValues = Mathf.Min(Count((uint)trial.conditions.noiseLevels),
                    Enum.GetNames(typeof(NoiseLevel)).Length);
                var loadLevels = Mathf.Min(Count((uint)trial.conditions.loadLevels),
                    Enum.GetNames(typeof(LoadLevel)).Length);


                var options = loadLevels * noiseLevelValues;

                Debug.Log($"Options: {options}");

                var optionList = new List<(LoadLevel, NoiseLevel)>();


                var loadLevel = GenerateLoadLevels(trial.conditions.loadLevels);
                var noiseLevel = GenerateNoiseLevels(trial.conditions.noiseLevels);


                foreach (var level in loadLevel)
                {
                    foreach (var noise in noiseLevel)
                    {
                        optionList.Add((level, noise));
                    }
                }

                foreach (var tuple in optionList)
                {
                    Debug.Log($"{tuple.Item1}, {tuple.Item2}");
                }

                trial.conditionList = optionList;
                trials[i] = trial;
            }
        }

        private void StartStudy(Study study)
        {
            var participantStartIndex = Random.Range(0, study.conditionList.Count);
            
            
        }

        private void AdvanceTrial(Study study, ref int currentIndex, in int startIndex)
        {
            currentIndex = (currentIndex + 1) % study.conditionList.Count;

            if (currentIndex != startIndex)
            {
                StartNextTrial(study.conditionList[currentIndex]);
                return;
            }

            EndStudy();

        }

        private void EndStudy()
        {
            throw new NotImplementedException();
        }

        private void StartNextTrial((LoadLevel, NoiseLevel) studyCondition)
        {
            throw new NotImplementedException();
        }


        private List<NoiseLevel> GenerateNoiseLevels(NoiseLevel noiseLevel)
        {
            var result = new List<NoiseLevel>();

            AddNoiseLevel(noiseLevel, NoiseLevel.None, ref result);
            AddNoiseLevel(noiseLevel, NoiseLevel.Low, ref result);
            AddNoiseLevel(noiseLevel, NoiseLevel.Audio, ref result);
            AddNoiseLevel(noiseLevel, NoiseLevel.Visual, ref result);
            AddNoiseLevel(noiseLevel, NoiseLevel.High, ref result);
            return result;
        }
        
        private List<LoadLevel> GenerateLoadLevels(LoadLevel loadLevel)
        {
            var result = new List<LoadLevel>();
            
            AddLoadLevel(loadLevel, LoadLevel.Low, ref result);
            AddLoadLevel(loadLevel, LoadLevel.High, ref result);
            return result;
        }
        
        private static void AddLoadLevel(LoadLevel noiseLevel, LoadLevel targetNoiseLevel, ref List<LoadLevel> result)
        {
            if ((noiseLevel & targetNoiseLevel) == targetNoiseLevel)
            {
                result.Add(targetNoiseLevel);
            }
        }

        private static void AddNoiseLevel(NoiseLevel noiseLevel, NoiseLevel targetNoiseLevel, ref List<NoiseLevel> result)
        {
            if ((noiseLevel & targetNoiseLevel) == targetNoiseLevel)
            {
                result.Add(targetNoiseLevel);
            }
        }


        private static uint Count(uint enumValue)
        {
            var v = enumValue;
            v = v - ((v >> 1) & 0x55555555); // reuse input as temporary
            v = (v & 0x33333333) + ((v >> 2) & 0x33333333); // temp
            var c = ((v + (v >> 4) & 0xF0F0F0F) * 0x1010101) >> 24; // count
            return c;
        }
    }


    [Serializable]
    public struct Study
    {
        public int selectionsPerTrial;
        public int repeatsPerTrial;
        public Condition conditions;

        public List<(LoadLevel, NoiseLevel)> conditionList;

    }
}