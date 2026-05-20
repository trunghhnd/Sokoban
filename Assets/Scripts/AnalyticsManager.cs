using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public static class AnalyticsManager
{
    public static void LogLevelStart(int level)
    {
        FirebaseAnalytics.LogEvent("level_start",
            new Parameter("level", level));
    }

    public static void LogLevelComplete(int level)
    {
        FirebaseAnalytics.LogEvent("level_complete",
            new Parameter("level", level));
    }
}
