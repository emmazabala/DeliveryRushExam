using TMPro;
using UnityEngine;

namespace DeliveryRushExam.UI
{
    public class ScorePopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private float lifetime = 1.1f;

        private CanvasGroup canvasGroup;
        private float age;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Setup(string message)
        {
            age = 0f;
            messageText.text = message;

            if (canvasGroup)
            {
                canvasGroup.alpha = 1f;
            }
        }

        private void Update()
        {
            age += Time.deltaTime;

            if (canvasGroup)
            {
                canvasGroup.alpha = 1f - age / lifetime;
            }

            if (age >= lifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}