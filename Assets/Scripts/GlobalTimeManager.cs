using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GlobalTimeManager : GenericSingleton<GlobalTimeManager>
{
    public TMP_Text sceneTimeText;
    public TMP_Text sessionTimeText;
    public TMP_Text todayTimeText;
    public TMP_Text lifetimeTimeText;

    private float sceneTime;
    private float sessionTime;
    private float todayTime;
    private float lifetimeTime;

    private float updateTimer = 0f;
    private string todayKey;
    private const string LifetimeKey = "LifetimeTime";
    private const string TodayDateKey = "TodayDate";

    void OnEnable()
    {
        CustomSceneLoader.OnSceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        CustomSceneLoader.OnSceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(string sceneName)
    {
        ResetSceneTime();
    }
    void Start()
    {
        todayKey = $"TodayTime_{DateTime.Now:yyyyMMdd}";
        LoadPersistentTimes();
        ResetSceneTime();

    }

    void Update()
    {
        float delta = Time.deltaTime;
        sceneTime += delta;
        sessionTime += delta;
        todayTime += delta;
        lifetimeTime += delta;

        updateTimer += delta;
        if (updateTimer >= 1f)
        {
            updateTimer = 0f;
            UpdateTimeTexts();
            SavePersistentTimes();
        }
    }

    private void OnApplicationQuit()
    {
        SavePersistentTimes();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SavePersistentTimes();
    }

    private void LoadPersistentTimes()
    {
        // Load today's time
        string lastSavedDate = PlayerPrefs.GetString(TodayDateKey, "");
        string todayString = DateTime.Now.ToString("yyyyMMdd");
        if (lastSavedDate == todayString)
        {
            todayTime = PlayerPrefs.GetFloat(todayKey, 0f);
        }
        else
        {
            todayTime = 0f;
            PlayerPrefs.SetString(TodayDateKey, todayString);
        }

        // Load lifetime time
        lifetimeTime = PlayerPrefs.GetFloat(LifetimeKey, 0f);
    }

    private void SavePersistentTimes()
    {
        PlayerPrefs.SetFloat(todayKey, todayTime);
        PlayerPrefs.SetFloat(LifetimeKey, lifetimeTime);
        PlayerPrefs.SetString(TodayDateKey, DateTime.Now.ToString("yyyyMMdd"));
        PlayerPrefs.Save();
    }

    private void UpdateTimeTexts()
    {
        if (sceneTimeText) sceneTimeText.text = $"Scene Time : {FormatTime(sceneTime)})";
        if (sessionTimeText) sessionTimeText.text = $"Session Time : {FormatTime(sessionTime)}";
        if (todayTimeText) todayTimeText.text = $"Day Time : {FormatTime(todayTime)}";
        if (lifetimeTimeText) lifetimeTimeText.text = $"Lifetime Time : {FormatTime(lifetimeTime)}";
    }

    private string FormatTime(float seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);
        if (t.Days > 0)
            return $"{t.Days} days {t.Hours} hr {t.Minutes} min {t.Seconds} sec";
        return $"{t.Hours} hr {t.Minutes} min {t.Seconds} sec";
    }

    // Add this method to allow external reset of scene time
    public void ResetSceneTime()
    {
        sceneTime = 0f;
    }
}
