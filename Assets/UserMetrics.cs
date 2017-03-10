using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserMetrics : MonoBehaviour
{

    private string dateFormat = "MM.dd.yyyy";
    private string timeFormat = "HH:mm:ss";


    [SerializeField]
    private System.Environment.SpecialFolder saveLocationFolder;

    private string saveLocationPath;

    [SerializeField]
    private int minutesBetweenAutoSaves = 60;

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

    private GameManager.State prevState;

    // Use this for initialization
    void Start()
    {
        prevState = GameManager.gameState;

        saveLocationPath = System.Environment.GetFolderPath(saveLocationFolder);

        InitializeDirectories();
        LoadUserMetrics();
        StartCoroutine(SaveRegularly());

    }

    private void Update()
    {
        GameManager.State gameState = GameManager.gameState;

        if (gameState != prevState && gameState == GameManager.State.ACTIVE)
        {

            AddUserActivity();

        }

        if (gameState == GameManager.State.IDLE)
        {

            AddUptime(Time.deltaTime, false);

        }
        else
        {

            AddUptime(Time.deltaTime, true);

        }

        prevState = gameState;


    }

    public void AddUptime(float deltaTime, bool isActiveTime)
    {

        dailySummary.AddUptime(deltaTime, isActiveTime);
        overallSummary.AddUptime(deltaTime, isActiveTime);

    }

    public void AddUserActivity()
    {

        dailySummary.AddTimeActivated();
        overallSummary.AddTimeActivated();
    }

    public void InitializeDirectories()
    {
        topLevelDirectoryPath = saveLocationPath + "/" + DIR_NAME;
        dailySummaryDirectoryPath = topLevelDirectoryPath + "/" + DAILY_DIR_NAME;
        overallSummaryFilePath = topLevelDirectoryPath + "/" + OVERALL_FILE_NAME + ".json";

        if (!Directory.Exists(topLevelDirectoryPath))
        {
            Directory.CreateDirectory(topLevelDirectoryPath);
            Debug.Log("Creating directory at " + topLevelDirectoryPath);
        }

        if (!Directory.Exists(dailySummaryDirectoryPath))
        {
            Directory.CreateDirectory(dailySummaryDirectoryPath);
        }
    }

    public void SaveUserMetrics()
    {
        Debug.Log("Saving");

        SaveDailySummary();
        SaveOverallSummary();

    }

    public void LoadUserMetrics()
    {

        LoadDailySummary();
        LoadOverallSummary();

    }

    public string GetDailySummaryPath()
    {
        return dailySummaryDirectoryPath + "/" + System.DateTime.Now.ToString(dateFormat) + ".json";

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
        AddUptime(0.0f, false);
        SaveUserMetrics();

        bool saved = false;

        while (true)
        {

            if (previouslyLoggedTime.Day != System.DateTime.Now.Day)
            {
                SaveDailySummary();
                dailySummary = new SerializeableUserMetrics();
            }

            // Save frequently
            if (System.DateTime.Now.Minute % minutesBetweenAutoSaves == 0 && System.DateTime.Now.Second == 0)
            {
                if (!saved)
                {
                    SaveUserMetrics();
                    saved = true;
                }
    
            }
            else
            {
                saved = false;
            }

            previouslyLoggedTime = System.DateTime.Now;

            yield return null;

        }
    }


    [System.Serializable]
    private class SerializeableUserMetrics
    {

        public int timesActivated = 1;
        public float averageSessionLength;

        public string totalUptime;
        public string userActivityTime;
        public string idleTime;

        public float uptimeInSeconds;
        public float userActivityTimeInSeconds;
        public float idleTimeInSeconds;

        private string[] userActivityTimeList;

        public void AddTimeActivated()
        {
            timesActivated++;

            string[] newTimeList = new string[userActivityTimeList.Length + 1];
            for (int i = 0; i < userActivityTimeList.Length; i++)
            {
                newTimeList[i] = userActivityTimeList[i];
            }

            newTimeList[newTimeList.Length - 1] = System.DateTime.Now.ToString();

        }

        public void AddUptime(float time, bool isActiveTime)
        {

            uptimeInSeconds += time;
            totalUptime = SecondsToReadableTime(uptimeInSeconds);

            if (isActiveTime)
            {
                userActivityTimeInSeconds += time;
                userActivityTime = SecondsToReadableTime(userActivityTimeInSeconds);

            }
            else
            {
                idleTimeInSeconds += time;
                idleTime = SecondsToReadableTime(idleTimeInSeconds);
            }

            if (timesActivated < 1)
            {
                timesActivated = 1;
            }

            averageSessionLength = userActivityTimeInSeconds / timesActivated;

        }

        public string SecondsToReadableTime(float seconds)
        {

            int hours = 0;
            int minutes = 0;

            if (seconds > 60.0)
            {
                minutes = Mathf.FloorToInt(seconds / 60.0f);
                seconds %= 60.0f;
               
            }

            if (minutes > 60)
            {

                hours = minutes / 60;
                minutes %= 60;

            }

            string readableTime = string.Format("{0}h:{1}m:{2}s", hours, minutes, seconds);

            return readableTime;
        }
    }
}
