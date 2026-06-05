using System;
using System.Collections.Generic;
using DeliveryRushExam.Data;
using UnityEngine;

namespace DeliveryRushExam.Core
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private float spawnInterval = 2.5f;
        [SerializeField] private int maxActiveOrders = 6;
        [SerializeField] private bool verboseLogs;

        [Header("Scene References")]
        [SerializeField] private ScoreManager scoreManager;

        private readonly List<OrderData> activeOrders = new();

        private readonly string[] customerNames =
        {
            "Alex",
            "Taylor",
            "Sam",
            "Jordan",
            "Casey",
            "Morgan",
            "Riley",
            "Avery"
        };

        private float spawnTimer;
        private int nextOrderId;
        private bool isRunning;

        public IReadOnlyList<OrderData> ActiveOrders => activeOrders;

        public event Action OrdersChanged;

        private void Awake()
        {
            if (scoreManager == null)
            {
                scoreManager = FindFirstObjectByType<ScoreManager>();
            }
        }

        private void Update()
        {
            if (!isRunning)
            {
                return;
            }

            UpdateSpawnTimer();

            int expiredOrders = UpdateOrders(Time.deltaTime);

            if (expiredOrders > 0)
            {
                OrdersChanged?.Invoke();
            }

            LogOrders(expiredOrders);
        }

        public void StartOrders()
        {
            activeOrders.Clear();
            nextOrderId = 0;

            // Fuerza spawn inmediato como en la versión original.
            spawnTimer = spawnInterval;

            isRunning = true;

            OrdersChanged?.Invoke();
        }

        public void StopOrders()
        {
            isRunning = false;

            activeOrders.Clear();

            OrdersChanged?.Invoke();
        }

        public void CompleteOrder(string orderId)
        {
            for (int i = 0; i < activeOrders.Count; i++)
            {
                if (activeOrders[i].id != orderId)
                {
                    continue;
                }

                OrderData order = activeOrders[i];

                activeOrders.RemoveAt(i);

                scoreManager.AddCompletedOrder(order);

                OrdersChanged?.Invoke();

                return;
            }
        }

        private void UpdateSpawnTimer()
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer < spawnInterval)
            {
                return;
            }

            spawnTimer = 0f;

            TrySpawnOrder();
        }

        private int UpdateOrders(float deltaTime)
        {
            int expiredCount = 0;

            for (int i = activeOrders.Count - 1; i >= 0; i--)
            {
                OrderData order = activeOrders[i];

                order.remainingTime -= deltaTime;

                if (order.remainingTime > 0f)
                {
                    continue;
                }

                activeOrders.RemoveAt(i);
                expiredCount++;
            }

            return expiredCount;
        }

        private void LogOrders(int expiredCount)
        {
            if (!verboseLogs)
            {
                return;
            }

            Debug.Log($"Active orders: {activeOrders.Count} expired: {expiredCount}");
        }

        private void TrySpawnOrder()
        {
            if (activeOrders.Count >= maxActiveOrders)
            {
                return;
            }

            string id = $"ORDER_{nextOrderId}";

            string customer =
                customerNames[UnityEngine.Random.Range(0, customerNames.Length)];

            int points = UnityEngine.Random.Range(80, 151);

            int coins = UnityEngine.Random.Range(4, 12);

            float limit = UnityEngine.Random.Range(7f, 14f);

            activeOrders.Add(
                new OrderData(
                    id,
                    customer,
                    points,
                    coins,
                    limit));

            nextOrderId++;

            OrdersChanged?.Invoke();
        }
    }
}