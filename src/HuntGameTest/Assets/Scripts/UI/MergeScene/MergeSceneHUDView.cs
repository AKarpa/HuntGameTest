using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.MergeScene
{
    public class MergeSceneHUDView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button playButton;

        public event Action ClickedBuyButton;
        public event Action ClickedPlayButton;
        
        private void Awake()
        {
            buyButton.ActionWithThrottle(delegate
            {
                ClickedBuyButton?.Invoke();
            });
            playButton.ActionWithThrottle(delegate
            {
                ClickedPlayButton?.Invoke();
            });
        }

        public void SetGold(int gold)
        {
            goldText.text = gold.ToString();
        }
    }
}
