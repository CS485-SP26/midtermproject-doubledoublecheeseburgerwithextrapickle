using TMPro;
using UnityEngine;
using Environment;

public class SeasonUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text seasonText;
    [SerializeField] private SeasonManager seasonManager;
    [SerializeField] private CalendarController calendarController;
    [SerializeField] private DayController dayController;

    private void Start()
    {
        UpdateSeasonUI();

        if (seasonManager != null)
            seasonManager.OnSeasonChanged.AddListener(UpdateSeasonUI);

        if (dayController != null)
            dayController.dayPassedEvent.AddListener(UpdateSeasonUI);
    }

    private void OnDestroy()
    {
        if (seasonManager != null)
            seasonManager.OnSeasonChanged.RemoveListener(UpdateSeasonUI);

        if (dayController != null)
            dayController.dayPassedEvent.RemoveListener(UpdateSeasonUI);
    }

    public void UpdateSeasonUI()
    {
        if (seasonText == null || seasonManager == null || calendarController == null)
            return;

        string seasonName = seasonManager.GetCurrentSeason().seasonName;
        string dayName = calendarController.GetDayOfWeek().ToString();
        int dayNumber = calendarController.getDayOfSeason();

        seasonText.SetText(seasonName + " - " + dayName + " Day: " + dayNumber);
    }
}
