using System;
using System.IO;
using UnityEngine.Analytics;
using UnityEngine;


[Serializable] public class Report {
    public string version = "0.0.6";
    public string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    public double epoch = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    public string eventKey;
    public string eventValue;
    public string userId;

    public Report(string eventKey, string eventValue) {
        this.eventKey = eventKey;
        this.eventValue = eventValue;
        this.userId = AnalyticsSessionInfo.userId;
    }
}
public class Analytics
{
    private const string dir = "Assets/Logs/";
    private const string path = dir + "userLog.json";
    private static StreamWriter writer = null;

    public static void LogEvent(string eventKey, string eventValue) {
        if (!File.Exists(path)) {
            Directory.CreateDirectory(dir);
            writer = new StreamWriter(File.Create(path));
        } else {
            writer = new StreamWriter(File.OpenWrite(path));
        }
        writer.WriteLine(JsonUtility.ToJson(new Report(eventKey, eventValue)));
        writer.Flush();
        writer.Close();
    }
}
