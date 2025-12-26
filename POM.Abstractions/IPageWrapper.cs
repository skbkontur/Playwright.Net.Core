namespace Kontur.POM.Abstractions;

public interface IPageWrapper<out TPage> : IWrapper<TPage>
{
    string Url { get; }
}