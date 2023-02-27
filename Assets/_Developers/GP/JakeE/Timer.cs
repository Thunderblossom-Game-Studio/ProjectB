using System;
using System.Globalization;

namespace JE.Utilities
{
    public class Timer
    {
        private readonly float _timerDuration;
        private readonly float _defaultTime;
        private readonly bool _isLooping;
        
        private bool _isEnabled = true;

        private float _currentTime;
        private Action _onComplete;

        public Timer(float timerDuration, float defaultTime, Action onComplete)
        {
            _timerDuration = timerDuration;
            _onComplete += onComplete;
            _defaultTime = defaultTime;
            _currentTime = defaultTime;
        }

        public Timer(float timerDuration, Action onComplete, bool isLooping = true)
        {
            _timerDuration = timerDuration;
            _onComplete += onComplete;
            _isLooping = isLooping;
        }

        public void Tick(float deltaTime)
        {
            if (!_isEnabled) return;
            
            _currentTime += deltaTime;
            if (!(_currentTime >= _timerDuration)) return;
            _currentTime = 0;
            OnComplete();
        }

        public void SetTimer(float currentTime)
        {
            _currentTime = currentTime;
        }

        public void ResetTimer()
        {
            _currentTime = _defaultTime;
        }

        public void SubscribeMethod(Action subscribedMethod)
        {
            _onComplete += subscribedMethod;
        }

        public void UnSubscribeMethod(Action unSubscribedMethod)
        {
            _onComplete -= unSubscribedMethod;
        }

        public float GetRemainingTime()
        {
            return _timerDuration - _currentTime;
        }

        public float GetElapsedTime()
        {
            return _currentTime;
        }

        public float GetElapsedTimeNormalized()
        {
            return _currentTime / _timerDuration;
        }

        public float GetRemainingNormalized()
        {
            return _currentTime / _timerDuration;
        }

        public void EnableTimer(bool status)
        {
            _isEnabled = status;
        }

        private void OnComplete()
        {
            _onComplete?.Invoke();
            if (_isLooping) return;
            _isEnabled = false;
        }
    }
}