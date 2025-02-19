using UnityEngine;

public static class RecordManager
{
    private const string EasyKey = "BestTime_Easy";
    private const string MediumKey = "BestTime_Medium";
    private const string HardKey = "BestTime_Hard";

    public static void SaveRecord(int difficulty, float time)
    {
        string key = GetKey(difficulty);
        float currentRecord = PlayerPrefs.GetFloat(key, float.MaxValue);

        if (time < currentRecord)
        {
            PlayerPrefs.SetFloat(key, time);
            PlayerPrefs.Save();
        }
    }

    public static float GetRecord(int difficulty)
    {
        string key = GetKey(difficulty);
        return PlayerPrefs.GetFloat(key, float.MaxValue); // ���������� float.MaxValue, ���� ������ �� ����������
    }

    private static string GetKey(int difficulty)
    {
        switch (difficulty)
        {
            case 5: return EasyKey; // ������ ���������
            case 10: return MediumKey; // ������� ���������
            case 15: return HardKey; // ������� ���������
            default: return EasyKey;
        }
    }
}