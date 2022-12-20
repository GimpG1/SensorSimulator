using Configuration.Ranges;

namespace Devices.Calculation;

public interface IRangesController
{
    IRangesController Setup(KeyValuePair<int, int> minMaxRange);
    State CalculateStatus(float value);
}