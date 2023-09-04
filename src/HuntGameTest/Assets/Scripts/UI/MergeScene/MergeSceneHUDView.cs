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
        [SerializeField] private TextMeshProUGUI buyPriceText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button playButton;

        public event Action ClickedBuyButton;
        public event Action ClickedPlayButton;

        public void SetBuyPrice(int gold)
        {
            buyPriceText.text = gold.ToString();
        }
        
        public void SetGold(int gold)
        {
            goldText.text = gold.ToString();
        }

        public void SetLevel(int level)
        {
            levelText.text = $"Level: {level}";
        }
        
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
    }
}
