using Farming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class FarmTileSaveData
{
    public FarmTile.Condition condition;
    public bool hasCrop;
    public int cropID;
    public int growthStage;
}
