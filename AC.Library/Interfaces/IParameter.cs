namespace AC.Library.Interfaces;

public interface IParameter
{
    string Value { get; set; }
    bool Equals(object? obj);
    int GetHashCode();
}