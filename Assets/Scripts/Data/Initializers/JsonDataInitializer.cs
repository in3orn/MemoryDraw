using UnityEngine;
using System.Collections;
using System.IO;

namespace Dev.Krk.MemoryDraw.Data.Initializers
{
    public class JsonDataInitializer<DataType> : ResourcesInitializer
    {
        [SerializeField]
        private string fileName = "*.json";


        private DataType data;


        public DataType Data { get { return data; } }


        public override void Init()
        {
            StartCoroutine(InitData());
        }

        private IEnumerator InitData()
        {
            WWW www = LoadFile(fileName);
            yield return www;

            data = JsonUtility.FromJson<DataType>(www.text);

            initialized = true;
            if (OnInitialized != null) OnInitialized();
        }

        private WWW LoadFile(string fileName)
        {
            string filePath = GetFilePath(fileName);
            return new WWW(filePath);
        }

        private string GetFilePath(string fileName)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return "jar:file://" + Application.dataPath + "!/assets/" + fileName;
            }
            else
            {
                return "file://" + Path.Combine(Application.streamingAssetsPath, fileName);
            }
        }
    }
}
