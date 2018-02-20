using UnityEngine;
using Dev.Krk.MemoryDraw.Data.Initializers;
using Dev.Krk.MemoryDraw.Data;

namespace Dev.Krk.MemoryDraw.Game.State
{
    public class ProgressController : MonoBehaviour
    {
        private readonly string FLOW = "Flow";

        [SerializeField]
        private FlowsDataInitializer flowsDataController;

        private int flow;

        private int map;

        public int Flow { get { return flow; } }

        public int Map { get { return map; } }

        void Start()
        {
            LoadData();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                SaveData();
        }

        void OnApplicationQuit()
        {
            SaveData();
        }
        
        private void SaveData()
        {
            PlayerPrefs.SetInt(FLOW, flow);
        }

        private void LoadData()
        {
            flow = PlayerPrefs.GetInt(FLOW);
        }

        public bool IsFlowCompleted()
        {
            return map == flowsDataController.Data.Flows[flow].Levels.Length;
        }

        public void NextMap()
        {
            map++;
        }

        public void NextFlow(int level)
        {
            int max = Mathf.Min(flowsDataController.Data.Flows.Length - 1, level + 1);
            if (flow < max) flow++;
            map = 0;
        }

        public void ResetFlow(int level)
        {
            int min = Mathf.Max(1, level - 1);
            if (flow > min) flow--;
            else flow = min;
            map = 0;
        }
    }
}