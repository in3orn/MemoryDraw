using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dev.Krk.MemoryDraw.Progress
{
    public abstract class BinaryDataLoader<DataType> : MonoBehaviour
    {
        [SerializeField]
        private string fileName = "*.mdp";


        private string filePath;


        void Start()
        {
            filePath = GetFilePath(fileName);
        }


        public DataType Load()
        {
            if (File.Exists(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = File.Open(filePath, FileMode.Open);
                DataType data = (DataType)formatter.Deserialize(file);
                file.Close();

                return data;
            }
            return CreateEmpty();
        }

        protected abstract DataType CreateEmpty();

        public void Save(DataType data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(filePath);
            formatter.Serialize(file, data);
            file.Close();
        }

        private string GetFilePath(string fileName)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return Application.persistentDataPath + @"\Resources\Save\" + fileName;
            }
            else
            {
                return Path.Combine(Application.streamingAssetsPath, fileName);
            }
        }
    }
}
