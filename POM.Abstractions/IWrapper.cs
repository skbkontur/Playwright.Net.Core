namespace SkbKontur.POM.Abstractions;

public interface IWrapper<out T>
{
    T WrappedItem { get; }
}