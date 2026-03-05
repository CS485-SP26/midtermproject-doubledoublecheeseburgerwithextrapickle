using Environment;
using UnityEngine;
using UnityEngine.Events;

public class CalendarController : MonoBehaviour
{
    public enum DayOfWeek { monday, tuesday, wednesday, thursday, friday, saturday, sunday }

    public DayOfWeek currentDay;

    public int currentWeek;

    public int dayOfSeason;


    [SerializeField] SeasonManager seasonManager;
    [SerializeField] DayController dayController;

    private void Start()
    {
        dayController.dayPassedEvent.AddListener(AdvanceDay);
        currentWeek = 1;
        dayOfSeason = 1;
    }

    public void AdvanceDay()
    {
        currentDay = (DayOfWeek)(((int)currentDay + 1) % 7);
        dayOfSeason++;
        if(currentDay == DayOfWeek.monday)
        {
            currentWeek = (currentWeek + 1) % 4;
        }
        if(dayOfSeason > seasonManager.GetCurrentSeason().seasonLengthInDays)
        {
            dayOfSeason = 1;
            seasonManager.AdvanceSeason();
        }
    }

    public void setDay(DayOfWeek dayOfWeek)
    {
        currentDay = dayOfWeek;
    }

    public DayOfWeek GetDayOfWeek(DayOfWeek dayOfWeek)
    {
        return currentDay;
    }

    public int getCurrentWeek()
    {
        return currentWeek;
    }

    public int getDayOfSeason()
    {
        return dayOfSeason;
    }

}
