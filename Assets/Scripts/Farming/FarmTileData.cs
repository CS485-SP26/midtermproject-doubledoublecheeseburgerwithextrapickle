using System;
using Farming;

[Serializable]
public class FarmTileData
{
    public FarmTile.Condition condition;
    public bool hasPlant;
    public bool isGrown;
    public bool isWithered;
    public int daysSinceLastInteraction;
    public int growthStage;
}