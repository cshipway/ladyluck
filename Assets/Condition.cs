[System.Serializable]
public class Condition
{
    public string name;

    public enum ConditionBasis
    {
        Round
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
        }

        toRet += ": ";

        return toRet;
    }

    public bool Evaluate(int round)
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

            default:
                return false;
        }
    }
}
