using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace GameFramework.Samples.PersistentData
{
    public class PerformanceTest : MonoBehaviour
    {
        // 我们以 一百，一万， 一百万的次数来测试 Json 和 Binary 这两种存储方式的性能情况
        public List<int> testTimes = new List<int>() {100, 10000, 1000000};
        private StringBuilder builder = new StringBuilder();
        private List<PerformanceInfo> infos = new List<PerformanceInfo>();

        private void OnEnable()
        {
            infos.Clear();
            foreach (int times in testTimes)
            {
                infos.Add(TestData1(times));
                infos.Add(TestData2(times));
            }
            
            builder.Clear();
            builder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\n", "TestDataName", "StorageMode", "TestTimes", "SetDataTime", "SaveTime", "LoadTime", "GetDataTime", "StorageSize");
            builder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\n", "type", "type", "int", "milliseconds", "milliseconds", "milliseconds", "milliseconds", "byte");
            
            foreach (PerformanceInfo info in infos)
            {
                PerformanceItem jsonItem = info.items[0];
                PerformanceItem binaryItem = info.items[1];
                builder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\n", info.name, "Json", info.times, jsonItem.SetDataTime, jsonItem.SaveTime, jsonItem.LoadTime, jsonItem.GetDataTime, jsonItem.StorageSize);
                builder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\n", info.name, "Binary", info.times, binaryItem.SetDataTime, binaryItem.SaveTime, binaryItem.LoadTime, binaryItem.GetDataTime, binaryItem.StorageSize);
            }
            
            // 为了方便观察，我将测试数据导出成表格的形式进行分析
            FileUtils.WriteAllText(PersistentSetting.Instance.GetSavePath($"PerformanceTest_{string.Join('-', testTimes)}.txt"), builder.ToString());
        }

        private PerformanceInfo TestData1(int times)
        {
            List<PerformanceData1> list = new List<PerformanceData1>();
            for (int i = 0; i < times; i++)
            {
                list.Add(new PerformanceData1()
                {
                    t = new TransformData()
                    {
                        position = new CustomVector3(Random.insideUnitSphere * 1000f),
                        eulerAngles = new CustomVector3(Random.insideUnitSphere * 360f),
                        localScale = new CustomVector3(Random.insideUnitSphere * 100f)
                    },
                });
            }

            return TestData(list);
        }

        private PerformanceInfo TestData2(int times)
        {
            List<PerformanceData2> list = new List<PerformanceData2>();
            for (int i = 0; i < times; i++)
            {
                PerformanceData2 data = new PerformanceData2();
                data = new PerformanceData2()
                {
                    nickName = "DefaultNickName",
                    sxe = 1,
                    age = 18,
                    properties = new Dictionary<int, int>()
                };

                for (int j = 0; j < 10; j++)
                {
                    data.properties.Add(j, 10);
                }

                list.Add(data);
            }

            return TestData(list);
        }

        private PerformanceInfo TestData<T>(List<T> list)
        {
            int count = list.Count;
            string typeName = typeof(T).Name;
            string jsonStorageName = $"JsonStorage{typeName}_{count}";
            string binaryStorageName = $"BinaryStorage{typeName}_{count}";
            string dataKey = "data";

            PersistentManager.Instance.Delete(jsonStorageName);
            PersistentManager.Instance.Delete(binaryStorageName);

            PersistentSetting.Instance.StorageMode = StorageMode.Json;
            PerformanceInfo info = new PerformanceInfo()
            {
                name = typeName,
                times = count,
                items = new PerformanceItem[] {new PerformanceItem(), new PerformanceItem()}
            };
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                PersistentManager.Instance.SetData(jsonStorageName, dataKey + i, list[i]);
            }

            stopwatch.Stop();
            info.items[0].SetDataTime = stopwatch.ElapsedMilliseconds;
            Debug.Log($"[Json {typeName}] => SetData : {count} Milliseconds : {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            PersistentManager.Instance.Save(jsonStorageName);
            stopwatch.Stop();
            info.items[0].SaveTime = stopwatch.ElapsedMilliseconds;
            Debug.Log($"[Json {typeName}] => Save : {count} Milliseconds : {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            PersistentManager.Instance.Load(jsonStorageName);
            stopwatch.Stop();
            info.items[0].LoadTime = stopwatch.ElapsedMilliseconds;
            Debug.Log($"[Json {typeName}] => Load : {count} Milliseconds : {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            for (int i = 0; i < count; i++)
            {
                T data = PersistentManager.Instance.GetData<T>(jsonStorageName, dataKey + i);
            }

            stopwatch.Stop();
            info.items[0].GetDataTime = stopwatch.ElapsedMilliseconds;
            Debug.Log($"[Json {typeName}] => GetData : {count} Milliseconds : {stopwatch.ElapsedMilliseconds}");
            info.items[0].StorageSize = new FileInfo(PersistentSetting.Instance.GetSavePath(jsonStorageName)).Length;

            PersistentSetting.Instance.StorageMode = StorageMode.Binary;
            stopwatch.Restart();
            for (int i = 0; i < count; i++)
            {
                PersistentManager.Instance.SetData(binaryStorageName, dataKey + i, list[i]);
            }

            stopwatch.Stop();
            info.items[1].SetDataTime = stopwatch.ElapsedMilliseconds;
            Debug.Log($"[Binary {typeName}] => SetData : {count} Milliseconds : {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            PersistentManager.Instance.Save(binaryStorageName);
            stopwatch.Stop();
            info.items[1].SaveTime = stopwatch.ElapsedMilliseconds;
            Debug.Log($"[Binary {typeName}] => Save : {count} Milliseconds : {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            PersistentManager.Instance.Load(binaryStorageName);
            stopwatch.Stop();
            info.items[1].LoadTime = stopwatch.ElapsedMilliseconds;
            Debug.Log($"[Binary {typeName}] => Load : {count} Milliseconds : {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            for (int i = 0; i < count; i++)
            {
                T data = PersistentManager.Instance.GetData<T>(binaryStorageName, dataKey + i);
            }

            stopwatch.Stop();
            info.items[1].GetDataTime = stopwatch.ElapsedMilliseconds;
            Debug.Log($"[Binary {typeName}] => GetData : {count} Milliseconds : {stopwatch.ElapsedMilliseconds}");
            info.items[1].StorageSize = new FileInfo(PersistentSetting.Instance.GetSavePath(binaryStorageName)).Length;

            Debug.Log("------------------------------------------------------------------------------");
            return info;
        }
    }

    public class PerformanceInfo
    {
        public string name;
        public int times;
        public PerformanceItem[] items;
    }

    public class PerformanceItem
    {
        public long SetDataTime;
        public long SaveTime;
        public long LoadTime;
        public long GetDataTime;
        public long StorageSize;
    }

    [Serializable]
    public struct PerformanceData1
    {
        public TransformData t; // 存放的是 Transform 的数据
    }

    [Serializable]
    public class PerformanceData2 // 存放了角色的基本数据
    {
        public string nickName;
        public byte sxe;
        public int age;
        public Dictionary<int, int> properties;
    }
}