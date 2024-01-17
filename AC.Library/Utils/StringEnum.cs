namespace AC.Library.Utils;

public class StringEnum
{
    public string Value { get; set; }
 
    public StringEnum(string value) => Value = value;
 
    public static bool operator ==(StringEnum enumA, StringEnum enumB) => enumA.Value == enumB.Value;
    public static bool operator !=(StringEnum enumA, StringEnum enumB) => enumA.Value != enumB.Value;
 
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
 
        if (ReferenceEquals(obj, null))
            return false;
 
        if (obj is not StringEnum enumB)
            return false;
 
        return Value.Equals(enumB.Value);
    }
 
    public override int GetHashCode() => Value.GetHashCode();
}