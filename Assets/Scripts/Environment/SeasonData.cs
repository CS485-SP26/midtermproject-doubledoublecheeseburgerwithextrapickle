using UnityEngine;
using static SeasonManager;

[CreateAssetMenu(fileName = "Season", menuName = "Scriptable Objects/Season")]
public class SeasonData : ScriptableObject
{
    [Range(-50, 100)]
    [Tooltip("Temperature in Fahrenheit for the season.")]
    public float temperature;

    [Tooltip("Name of the season")]
    public string seasonName;

    [Range(0, 24)]
    [Tooltip("Length of the day in hours for the season.")]
    public float dayLength;

    [Tooltip("Sky color for the season.")]
    public Color skyColor;

    public int seasonLengthInDays;

    [Range(0, 1)]
    [Tooltip("Rate at which plants wither during this season. 0 means no withering, 1 means instant withering.")]
    public float witherRate;

    public Seasons seasonsEnum;



}
