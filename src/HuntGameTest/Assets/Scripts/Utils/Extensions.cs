using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public static class Extensions
    {
        public static void ActionWithThrottle(this Button button, Action action, float throttleMilliseconds = 200f)
        {
            button.OnClickAsObservable().ThrottleFirst(TimeSpan.FromMilliseconds(throttleMilliseconds)).Subscribe(
                delegate
                {
                    action?.Invoke();
                }).AddTo(button);
        }
    }
}