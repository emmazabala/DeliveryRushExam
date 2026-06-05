using System;
using System.Threading.Tasks;
using DeliveryRushExam.Data;
using DeliveryRushExam.UGS;
using UnityEngine;

namespace DeliveryRushExam.Save
{
    public class SaveManager : MonoBehaviour
    {
        public PlayerProgressData CurrentProgress { get; private set; }
            = new PlayerProgressData();

        public event Action<PlayerProgressData> ProgressLoaded;
        private ISaveService saveService;
        
        [SerializeField] private UgsInitializer ugsInitializer;

        private async void Start()
        {
            saveService = ServiceLocator.Get<ISaveService>();

            if (ugsInitializer != null)
            {
                await ugsInitializer.InitializeAsync();
            }

            await LoadProgressAsync();
        }

        public async Task LoadProgressAsync()
        {
            CurrentProgress = await saveService.LoadAsync();
            
            Debug.Log(
                $"Loaded Progress | " +
                $"BestScore: {CurrentProgress.bestScore} | " +
                $"Coins: {CurrentProgress.totalCoins} | " +
                $"Orders: {CurrentProgress.completedOrders}");

            ProgressLoaded?.Invoke(CurrentProgress);
        }

        public async Task SaveMatchResultAsync(int score, int coins, int completedOrders)
        {
            CurrentProgress.bestScore =
                Mathf.Max(CurrentProgress.bestScore, score);

            CurrentProgress.totalCoins += coins;

            CurrentProgress.completedOrders += completedOrders;

            CurrentProgress.unlockedLevel = Mathf.Max(CurrentProgress.unlockedLevel, 1 + CurrentProgress.completedOrders / 10);
            
            Debug.Log(
                $"Saving Progress | " +
                $"BestScore: {CurrentProgress.bestScore} | " +
                $"Coins: {CurrentProgress.totalCoins}");

            await saveService.SaveAsync(CurrentProgress);
        }
    }
}