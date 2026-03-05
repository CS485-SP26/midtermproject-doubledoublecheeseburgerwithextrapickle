using UnityEngine;
using UnityEngine.Events;

public class WeatherController : MonoBehaviour
{

    [SerializeField] private SeasonManager seasonManager;
    [SerializeField] private Light sunlight;
    private void Start()
    {
        seasonManager.OnSeasonChanged.AddListener(UpdateWeather);
        UpdateWeather();
    }

    private void OnDestroy()
    {
        seasonManager.OnSeasonChanged.RemoveListener(UpdateWeather);
    }

    public void UpdateWeather()
    {
        var data = seasonManager.GetCurrentSeason();
        sunlight.color = data.skyColor;
    }
}
