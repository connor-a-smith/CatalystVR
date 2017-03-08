using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserMetrics : MonoBehaviour
{

    [SerializeField] private string saveLocationPath;
    [SerializeField] private float minutesBetweenAutoSaves = 60.0f;

    const string DIR_NAME = "CAVEkiosk User Metrics";
    const string DAILY_DIR_NAME = "Daily Summaries";
    const string OVERALL_FILE_NAME = "All-Time Metrics";

    private string totalPath;

    private SerializeableUserMetrics dailySummary;
    private SerializeableUserMetrics overallSummary;

    private string topLevelDirectoryPath;
    private string overallSummaryFilePath;
    private string dailySummaryDirectoryPath;

    private System.DateTime previouslyLoggedTime;

    // Use this for initialization
    void Start()
    {

        InitializeDirectories();
        StartCoroutine(SaveRegularly());

    }

    public void InitializeDirectories()
    {
        topLevelDirectoryPath = saveLocationPath + "/" + DIR_NAME;
        dailySummaryDirectoryPath = topLevelDirectoryPath + "/" + DAILY_DIR_NAME;
        overallSummaryFilePath = topLevelDirectoryPath + "/" + OVERALL_FILE_NAME + ".json";

        if (!Directory.Exists(topLevelDirectoryPath))
        {
            Directory.CreateDirectory(topLevelDirectoryPath);
        }

        if (!Directory.Exists(dailySummaryDirectoryPath))
        {
            Directory.CreateDirectory(dailySummaryDirectoryPath);
        }
    }

    public void SaveUserMetrics()
    {

        SaveDailySummary();
        SaveOverallSummary();

    }

    public string GetDailySummaryPath()
    {
        return dailySummaryDirectoryPath + "/" + System.DateTime.Now.ToShortDateString() + ".json";

    }

    private void SaveDailySummary()
    {
        string serializedMetrics = JsonUtility.ToJson(dailySummary);
        string filePath = GetDailySummaryPath();

        File.WriteAllText(filePath, serializedMetrics);        
    }

    private void SaveOverallSummary()
    {
        string serializedMetrics = JsonUtility.ToJson(overallSummary);

        File.WriteAllText(overallSummaryFilePath, serializedMetrics);
    }

    private void LoadDailySummary()
    {
        string filePath = GetDailySummaryPath();

        if (File.Exists(filePath))
        {

            string jsonString = File.ReadAllText(filePath);
            dailySummary = JsonUtility.FromJson<SerializeableUserMetrics>(jsonString);

        }
        else
        {

            dailySummary = new SerializeableUserMetrics();

        }
    }

    private void LoadOverallSummary()
    {

        if (File.Exists(overallSummaryFilePath))
        {

            string jsonString = File.ReadAllText(overallSummaryFilePath);
            overallSummary = JsonUtility.FromJson<SerializeableUserMetrics>(jsonString);

        }
        else
        {
            overallSummary = new SerializeableUserMetrics();
        }
    }

    public IEnumerator SaveRegularly()
    {

        previouslyLoggedTime = System.DateTime.Now;

        while (true)
        {
            
            if (previouslyLoggedTime.Day != System.DateTime.Now.Day)
            {
                SaveDailySummary();
                dailySummary = new SerializeableUserMetrics();
            }

            // Save every hour, when the minutes are 0.
            if (System.DateTime.Now.Minute == 0)
            {
                SaveUserMetrics();
            }

            previouslyLoggedTime = System.DateTime.Now;

            yield return null;

        }
    }


    [System.Serializable]
    private class SerializeableUserMetrics
    {

        public string totalUptime;
        public string userActivityTime;
        public string idleTime;

    }
}
