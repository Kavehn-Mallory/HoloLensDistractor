﻿using System;

namespace DistractorProject.SceneManagement
{
    public class LoadingProgress : IProgress<float>
    {
        public event Action<float> Progressed;

        private const float Ratio = 1f;
        
        public void Report(float value)
        {
            Progressed?.Invoke(value / Ratio);
        }
    }
}