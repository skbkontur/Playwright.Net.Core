namespace SkbKontur.Playwright.POM.Abstractions;

/// <summary>
/// Базовый интерфейс паттерна обёртки (Wrapper pattern).
/// Предоставляет доступ к обёрнутому объекту.
/// </summary>
/// <typeparam name="T">Тип обёрнутого объекта</typeparam>
public interface IWrapper<out T>
{
    /// <summary>
    /// Получить обёрнутый объект
    /// </summary>
    T WrappedItem { get; }
}