using Configuration.Ranges;

namespace Devices.Calculation;

public class RangesController : IRangesController
{
    private KeyValuePair<int,int> MinMaxRange { get; set; }
    private KeyValuePair<int,int> MinAlarm { get; set; }
    private KeyValuePair<int,int> MaxAlarm { get; set; }
    private KeyValuePair<int,int> MinWarning { get; set; }
    private KeyValuePair<int,int> MaxWarning { get; set; }
    
    public IRangesController Setup(KeyValuePair<int, int> minMaxRange)
    {
        this.MinMaxRange = minMaxRange;
        var absoluteValue = Math.Abs(minMaxRange.Key) + Math.Abs(minMaxRange.Value);
        CalculateAlarmRange(true, absoluteValue);
        CalculateAlarmRange(false, absoluteValue);
        CalculateWarningRange(true, absoluteValue);
        CalculateWarningRange(false, absoluteValue);
        return this;
    }

    private void CalculateWarningRange(bool minRange, float absoluteValue)
    {
        var warning = .25f * absoluteValue;
        if (minRange)
        {
            var wMin = (int)Math.Round((MinMaxRange.Key + warning), MidpointRounding.AwayFromZero);
            MinWarning = new KeyValuePair<int, int>(MinMaxRange.Key, wMin);
            return;
        }
        
        var wMax = (int)Math.Round((MinMaxRange.Value - warning), MidpointRounding.AwayFromZero);
        MaxWarning = new KeyValuePair<int, int>(wMax, MinMaxRange.Value);
    }

    private void CalculateAlarmRange(bool minRange, float absoluteValue)
    {
        var alarm = 0.1f * absoluteValue;
        if (minRange)
        {
            var aMin = (int)Math.Round((MinMaxRange.Key  + alarm), MidpointRounding.AwayFromZero);
            MinAlarm = new KeyValuePair<int, int>(MinMaxRange.Key, aMin);
            return;
        }
        
        var aMax = (int)Math.Round((MinMaxRange.Value - alarm), MidpointRounding.AwayFromZero);
        MaxAlarm = new KeyValuePair<int, int>(aMax, MinMaxRange.Value);
    }

    public State CalculateStatus(float value)
    {
        var result = State.Normal;
        var intValue = (int)Math.Round(value, MidpointRounding.AwayFromZero);
        
        if (IsInRange(MinWarning, intValue))
        {
            result = State.MinWarning;
        }
        if (IsInRange(MaxWarning ,intValue))
        {
            result = State.MaxWarning;
        }
        if (IsInRange(MinAlarm, intValue))
        {
            result = State.MinAlarm;
        }
        if (IsInRange(MaxAlarm, intValue))
        {
            result = State.MaxAlarm;
        }
            
        return result;
    }

    private bool IsInRange(KeyValuePair<int,int> range, int value)
    {
        return range.Key < value && range.Value > value;
    }
}