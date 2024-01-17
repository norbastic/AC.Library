namespace AC.Library.Models;

public enum PowerValues {
    Off = 0,
    On = 1
}

public enum ModeParameter {
    Auto = 0,
    Cool = 1,
    Dry = 2,
    Fan = 3,
    Heat = 4
}

public enum TempUnitParameter {
    Celsius = 0,
    Fahrenheit = 1
}

public enum FanSpeedParameter {
    Auto = 0,
    Low = 1,
    MediumLow = 2,
    Medium = 3,
    MediumHigh = 4,
    High = 5
}

public enum AirParameter {
    Off = 0,
    Inside = 1,
    Outside = 2,
    Mode3 = 3
}

public enum HealthParameter {
    Off = 0,
    On = 1,
}

public enum SleepParameter {
    Off = 0,
    On = 1,
}

public enum LightsParameter {
    Off = 0,
    On = 1,
}

public enum SwingHorizontalParameter {
    Default = 0,
    Full = 1,
    FixedLeft = 2,
    FixedMidLeft = 3,
    FixedMid = 4,
    FixedMidRight = 5,
    FixedRight = 6,
    FullAlt = 7,
}

public enum SwingVerticalParameter {
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

public enum QuietParameter {
    Off = 0,
    Mode1 = 1,
    Mode2 = 2,
    Mode3 = 3,
}

public enum TurboParameter {
    Off = 0,
    On = 1,
}

public enum PowerSaveParameter {
    Off = 0,
    On = 1,
}

public enum SafetyHeatingParameter {
    Off = 0,
    On = 1,
}