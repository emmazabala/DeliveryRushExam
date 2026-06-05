using UnityEngine;

namespace DeliveryRushExam.Save
{
    public class SaveServicesInstaller : MonoBehaviour
    {
        public enum SaveMode
        {
            Local,
            Cloud
        }

        [SerializeField]
        private SaveMode saveMode = SaveMode.Local;

        private void Awake()
        {
            Debug.Log($"Save Mode Selected: {saveMode}");
            
            ISaveService saveService;

            switch (saveMode)
            {
                case SaveMode.Cloud:
                    saveService = new UgsCloudSaveService();
                    break;

                default:
                    saveService = new LocalSaveService();
                    break;
            }

            ServiceLocator.Register<ISaveService>(saveService);
        }
    }
}