using UnityEngine;
using UnityEngine.Events;

public class SeasonManager : MonoBehaviour
{

    [SerializeField] private SeasonData currentSeason;

    public UnityEvent OnSeasonChanged = new UnityEvent();


    private SeasonData ScratchData;

    public SeasonData RuntimeData
    {
        get
        {
            return ScratchData;
        }
        set
        {
            ScratchData = Instantiate(value);
        }
    }

    public enum Seasons
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    [SerializeField] private SeasonData Winter;
    [SerializeField] private SeasonData Spring;
    [SerializeField] private SeasonData Summer;
    [SerializeField] private SeasonData Fall;

    private void Start()
    {
        SetSeason(Seasons.Winter);
        AdvanceSeason();
    }

    public void SetSeason(Seasons season)
    {
        switch (season)
        {
            case Seasons.Winter: RuntimeData = Winter; break;
            case Seasons.Spring: RuntimeData = Spring; break;
            case Seasons.Summer: RuntimeData = Summer; break;
            case Seasons.Fall: RuntimeData = Fall; break;
        }
        currentSeason = RuntimeData;
        OnSeasonChanged?.Invoke();
    }

    public void AdvanceSeason()
    {
        Seasons next = getEnumSeason() + 1;

        if (next > Seasons.Winter)
            next = Seasons.Spring;

        SetSeason(next);

    }

    public SeasonData GetCurrentSeason()
    {
        return currentSeason;
    }

    public Seasons getEnumSeason()
    {
        return currentSeason.seasonsEnum;
    }


}
