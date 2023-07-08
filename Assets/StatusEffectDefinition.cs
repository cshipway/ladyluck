using System.Diagnostics;

[System.Serializable]
public class StatusEffectDefinition
{
    public string name;
    public int duration;

    public int strength;
    public int armor;

    public override string ToString()
    {
        string toRet = "";
        int count = 0;

        if(strength > 0)
        {
            toRet += AddStatToString(strength, "strength", count);
            count++;
        }
        if(armor > 0)
        {
            toRet += AddStatToString(armor, "armor", count);
        }
        toRet += $" for {(duration == -1 ? "the rest of the battle" : $"{duration} turn{(duration > 1 ? "s" : "")}")}";

        return toRet;
    }

    private string AddStatToString(int amount, string stat, int count)
    {
        string toRet = "";
        if (count > 0)
            toRet += ", ";
        toRet += $"{amount} {stat}";
        return toRet;
    }
}
