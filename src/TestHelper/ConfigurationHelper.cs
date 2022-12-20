namespace TestHelper;

public class ConfigurationHelper
{
    public String ExampleSensorConfigContent()
    {
        return
            @"{
  ""Sensors"": [
    {
      ""ID"": 1,
      ""Type"": ""speed"",
      ""MinValue"": -10,
      ""MaxValue"": 100,
      ""EncoderType"": ""fixed"",
      ""Frequency"": 2
    },
    {
      ""ID"": 2,
      ""Type"": ""position"",
      ""MinValue"": -10000,
      ""MaxValue"": 10000,
      ""EncoderType"": ""fixed"",
      ""Frequency"": 1
    },
    {
      ""ID"": 3,
      ""Type"": ""depth"",
      ""MinValue"": 0,
      ""MaxValue"": 255,
      ""EncoderType"": ""fixed"",
      ""Frequency"": 10
    }
  ]
}";
    }

    public String ExampleReceiverConfig()
    {
        return @"
{
""Receivers"": [
  {
    ""Id"": 1,
    ""IsActive"" : true,
    ""ListenedSensorId"" : 1
  },
  {
    ""Id"": 2,
    ""IsActive"" : true,
    ""ListenedSensorId"" : 2
  },
  {
    ""Id"": 3,
    ""IsActive"" : true,
    ""ListenedSensorId"" : 3
  },
  {
    ""Id"": 4,
    ""IsActive"": false,
    ""ListenedSensorId"": 3
    }]
}";
    }
}