using System;
using DeliveryRushExam.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeliveryRushExam.UI
{
    public class OrderButtonView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text rewardText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Button completeButton;

        private OrderData orderData;
        private Action<string> onCompleteClicked;

        private int lastDisplayedTime = -1;

        public void Setup(OrderData order, Action<string> completeCallback)
        {
            orderData = order;
            onCompleteClicked = completeCallback;

            if (completeButton == null)
            {
                completeButton = GetComponent<Button>();
            }

            completeButton.onClick.RemoveAllListeners();
            completeButton.onClick.AddListener(HandleClick);

            titleText.text = $"Deliver to {order.customerName}";
            rewardText.text = $"+{order.rewardPoints} pts / +{order.rewardCoins} coins";

            RefreshTimer();
        }

        private void Update()
        {
            RefreshTimer();
        }

        private void RefreshTimer()
        {
            if (orderData == null)
            {
                return;
            }

            int currentTime = Mathf.CeilToInt(orderData.remainingTime);

            if (currentTime == lastDisplayedTime)
            {
                return;
            }

            lastDisplayedTime = currentTime;
            timerText.text = $"Time {currentTime}";
        }

        private void HandleClick()
        {
            if (orderData != null)
            {
                onCompleteClicked?.Invoke(orderData.id);
            }
        }
    }
}