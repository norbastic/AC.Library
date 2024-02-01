using AC.Library.Interfaces;

namespace AC.Library.Models;

public enum PowerValues {
    Off = 0,
    On = 1
}

public class PowerParameterValue : IParameterValue
{
    private readonly PowerValues _value;
    public PowerParameterValue(PowerValues value) {
        _value = value;
    }
    public int Value => (int) _value;
}

public enum ModeValues {
    Auto = 0,
    Cool = 1,
    Dry = 2,
    Fan = 3,
    Heat = 4
}

public class ModeParameterValue : IParameterValue
{
    private readonly ModeValues _value;
    public ModeParameterValue(ModeValues value) {
        _value = value;
    }
    public int Value => (int) _value;
}

public enum TemperatureValues
{
    _16 = 16,
    _17 = 17,
    _18 = 18,
    _19 = 19,
    _20 = 20,
    _21 = 21,
    _22 = 22,
    _23 = 23,
    _24 = 24,
    _25 = 25,
    _26 = 26,
    _27 = 27,
    _28 = 28,
    _29 = 29,
    _30 = 30,
    _31 = 31,
    _32 = 32,
    _33 = 33,
    _34 = 34,
    _35 = 35
}

public class TempParameterValue : IParameterValue
{
    private readonly TemperatureValues _value;
    public TempParameterValue(TemperatureValues value) {
        _value = value;
    }
    public int Value => (int) _value;
}

public enum TempUnitValues {
    Celsius = 0,
    Fahrenheit = 1
}

public enum FanSpeedValues {
    Auto = 0,
    Low = 1,
    MediumLow = 2,
    Medium = 3,
    MediumHigh = 4,
    High = 5
}

public enum AirValues {
    Off = 0,
    Inside = 1,
    Outside = 2,
    Mode3 = 3
}

public enum HealthValues {
    Off = 0,
    On = 1,
}

public enum SleepValues {
    Off = 0,
    On = 1,
}

public enum LightsValues {
    Off = 0,
    On = 1,
}

public enum SwingHorizontalValues {
    Default = 0,
    Full = 1,
    FixedLeft = 2,
    FixedMidLeft = 3,
    FixedMid = 4,
    FixedMidRight = 5,
    FixedRight = 6,
    FullAlt = 7,
}

public enum SwingVerticalValues {
    Default = 0,
    Full = 1,
    FixedTop = 2,
    FixedMidTop = 3,
    FixedMid = 4,
    FixedMidBottom = 5,
    FixedBottom = 6,
    SwingBottom = 7,
    SwingMidBottom = 8,
    SwingMid = 9,
    SwingMidTop = 10,
    SwingTop = 11,
}

public enum QuietValues {
    Off = 0,
    Mode1 = 1,
    Mode2 = 2,
    Mode3 = 3,
}

public enum TurboValues {
    Off = 0,
    On = 1,
}

public enum PowerSaveValues {
    Off = 0,
    On = 1,
}

public enum SafetyHeatingValues {
    Off = 0,
    On = 1,
}