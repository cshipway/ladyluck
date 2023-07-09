[System.Serializable]
public class Condition
{
    public string name;

    public enum ConditionBasis
    {
        Round,
        Health
    }
    public enum ComparisonType
    {
        Is,
        IsGreaterThan,
        IsLessThan
    }

    public ConditionBasis conditionBasis;
    public ComparisonType comparisonType;
    public int value;

    public override string ToString()
    {
        string toRet = "If ";
        
        switch(conditionBasis)
        {
            case ConditionBasis.Round:
                switch(comparisonType)
                {
                    case ComparisonType.Is:
                        toRet += "it's ";
                        break;

                    case ComparisonType.IsGreaterThan:
                        toRet += "it's after ";
                        break;

                    case ComparisonType.IsLessThan:
                        toRet += "it's before ";
                        break;
                }
                toRet += $"round {value}";
                break;

            case ConditionBasis.Health:
                switch(comparisonType)
                {
                    case ComparisonType.Is:
                        toRet += "your health is";
                        break;

                    case ComparisonType.IsGreaterThan:
                        toRet += "your health is more than ";
                        break;

                    case ComparisonType.IsLessThan:
                        toRet += "your health is less than ";
                        break;
                }
                toRet += $"{value}";
                break;
        }

        toRet += ": ";

        return toRet;
    }

    public bool Evaluate(int round, int health)
    {
        switch (conditionBasis)
        {
            case ConditionBasis.Round:
                switch (comparisonType)
                {
                    case ComparisonType.Is:
                        return round == value;

                    case ComparisonType.IsGreaterThan:
                        return round > value;

                    case ComparisonType.IsLessThan:
                        return round < value;
                }
                return false;

            case ConditionBasis.Health:
                switch (comparisonType)
                {
                    case ComparisonType.Is:
                        return health == value;

                    case ComparisonType.IsGreaterThan:
                        return health > value;

                    case ComparisonType.IsLessThan:
                        return health < value;
                }
                return false;

            default:
                return false;
        }
    }
}
