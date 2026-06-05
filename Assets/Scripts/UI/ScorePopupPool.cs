using System.Collections.Generic;
using UnityEngine;

namespace DeliveryRushExam.UI
{
    public class ScorePopupPool : MonoBehaviour
    {
        [SerializeField] private ScorePopupView popupPrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private int initialSize = 10;

        private readonly Queue<ScorePopupView> availablePopups = new();

        private void Awake()
        {
            for (int i = 0; i < initialSize; i++)
            {
                CreatePopup();
            }
        }

        public ScorePopupView Get()
        {
            if (availablePopups.Count == 0)
            {
                CreatePopup();
            }

            return availablePopups.Dequeue();
        }

        private void CreatePopup()
        {
            ScorePopupView popup = Instantiate(popupPrefab, container);

            popup.gameObject.SetActive(false);

            popup.LifetimeFinished += ReturnToPool;

            availablePopups.Enqueue(popup);
        }

        private void ReturnToPool(ScorePopupView popup)
        {
            popup.gameObject.SetActive(false);

            availablePopups.Enqueue(popup);
        }
    }
}