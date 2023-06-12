public enum ESettingType
{
    Bool,
    Int,
    Float,
    String
}

public class AddSettingForm
{
    

    internal ESettingType SettingType { get; set; } = ESettingType.Int;
    internal string SettingName { get; set; } = default;
    internal string SettingTag { get; set; } = default;
    internal int SettingIntValue { get; set; } = default;
    internal float SettingFloatValue { get; set; } = default;
    internal string SettingStringValue { get; set; } = default;
    internal bool SettingBoolValue { get; set; } = default;
}
