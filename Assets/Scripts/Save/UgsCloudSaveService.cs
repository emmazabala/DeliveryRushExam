using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryRushExam.Data;
using UnityEngine;

#if DELIVERY_RUSH_UGS
using Unity.Services.CloudSave;
#endif

namespace DeliveryRushExam.Save
{
    public class UgsCloudSaveService : ISaveService
    {
        private const string ProgressKey = "delivery_rush_progress";

        public async Task<PlayerProgressData> LoadAsync()
        {
#if DELIVERY_RUSH_UGS
            var data =
                await CloudSaveService.Instance.Data.LoadAsync(
                    new HashSet<string> { ProgressKey });

            if (!data.TryGetValue(ProgressKey, out string json))
            {
                return new PlayerProgressData();
            }

            return UnityEngine.JsonUtility
                .FromJson<PlayerProgressData>(json);
#else
            await Task.Yield();
            return new PlayerProgressData();
#endif
        }

        public async Task SaveAsync(PlayerProgressData progressData)
        {
#if DELIVERY_RUSH_UGS
            Debug.Log("Saving to Cloud Save...");
            progressData.TouchSaveDate();

            string json =
                UnityEngine.JsonUtility.ToJson(progressData);

            await CloudSaveService.Instance.Data.ForceSaveAsync(
                new Dictionary<string, object>
                {
                    { ProgressKey, json }
                });
            Debug.Log("Cloud Save completed.");
#else
            await Task.Yield();
#endif
        }
    }
}