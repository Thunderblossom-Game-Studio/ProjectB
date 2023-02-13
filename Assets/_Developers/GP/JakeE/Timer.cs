using System;
using System.Globalization;

namespace JE.Utilities
{
    public class Timer
    {
        private float _currentTime;
        private float _timerDuration;
        private float _defaultTime;
        private Action _onComplete;

        public Timer(float timerDuration, float defaultTime, Action onComplete)
        {
            _timerDuration = timerDuration;
            _onComplete += onComplete;
            _defaultTime = defaultTime;
            _currentTime = defaultTime;
        }

        public Timer(float timerDuration, Action onComplete)
        {
            _timerDuration = timerDuration;
            _onComplete += onComplete;
        }

        public void Tick(float deltaTime)
        {
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

        public string GetRemainingTime(bool convertToString)
        {
            return (_timerDuration - _currentTime).ToString(CultureInfo.InvariantCulture); 
        }

        public string GetRemainingNormalized(bool convertToString)
        {
            return (_currentTime / _timerDuration).ToString(CultureInfo.InvariantCulture);
        }

        public float GetRemainingTime()
        {
            return _timerDuration - _currentTime;
        }

        public float GetRemainingNormalized()
        {
            return _currentTime / _timerDuration;
        }

        private void OnComplete()
        {
            _onComplete?.Invoke();
        }
    }
}