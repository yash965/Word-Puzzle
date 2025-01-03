using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "DataModel", menuName = "Data")]
public class DataModel : ScriptableObject
{
    public string question;
    public string[] words;
    public bool[] correctQ;
    public int QuesToClearLevel;

    public WeatherType selectedWeatherType;     // Enum
    public WeatherType completeWeatherType;
}
