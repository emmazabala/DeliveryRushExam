using System.Threading.Tasks;
using DeliveryRushExam.Data;
using UnityEngine;

namespace DeliveryRushExam.Save
{
    public class LocalSaveService : ISaveService
    {
        private const string ProgressKey = "delivery_rush_progress";

        public Task<PlayerProgressData> LoadAsync()
        {
            if (!PlayerPrefs.HasKey(ProgressKey))
            {
                Debug.Log("Local Save: No save found.");
                return Task.FromResult(new PlayerProgressData());
            }

            string json = PlayerPrefs.GetString(ProgressKey);

            Debug.Log($"Local Save Loaded JSON: {json}");

            PlayerProgressData data =
                JsonUtility.FromJson<PlayerProgressData>(json);

            return Task.FromResult(data ?? new PlayerProgressData());
        }

        public Task SaveAsync(PlayerProgressData progressData)
        {
            progressData.TouchSaveDate();

            string json = JsonUtility.ToJson(progressData);

            Debug.Log($"Local Save Writing JSON: {json}");

            PlayerPrefs.SetString(ProgressKey, json);
            PlayerPrefs.Save();

            Debug.Log("PlayerPrefs.Save() executed.");

            return Task.CompletedTask;
        }
    }
}
