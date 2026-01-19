using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Pages;

namespace SkbKontur.Playwright.TestCore.Browsers;

/// <summary>
/// Реализация работы с localStorage браузера.
/// Выполняет JavaScript код на странице для доступа к localStorage.
/// </summary>
/// <param name="pageGetter">Получатель активной страницы браузера</param>
public class LocalStorage(IPageGetter pageGetter) : ILocalStorage, IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Лениво инициализируемая страница браузера.
    /// </summary>
    private readonly Lazy<Task<IPage>> _page = new(pageGetter.GetAsync);

    /// <summary>
    /// Получить количество элементов в localStorage.
    /// </summary>
    /// <returns>Задача, возвращающая количество элементов</returns>
    public async Task<long> GetLengthAsync()
        => await (await _page.Value).EvaluateAsync<long>("localStorage.length;");

    /// <summary>
    /// Очистить все данные из localStorage.
    /// </summary>
    /// <returns>Задача завершения очистки</returns>
    public async Task ClearAsync()
        => await (await _page.Value).EvaluateAsync("localStorage.clear();");

    /// <summary>
    /// Получить значение элемента по ключу из localStorage.
    /// </summary>
    /// <param name="keyName">Имя ключа</param>
    /// <returns>Задача, возвращающая значение элемента или null</returns>
    public async Task<string> GetItemAsync(string keyName)
        => await (await _page.Value).EvaluateAsync<string>($"localStorage.getItem('{keyName}');");

    /// <summary>
    /// Получить имя ключа по индексу из localStorage.
    /// </summary>
    /// <param name="keyNumber">Индекс ключа (начиная с 0)</param>
    /// <returns>Задача, возвращающая имя ключа</returns>
    public async Task<string> GetKeyAsync(int keyNumber)
        => await (await _page.Value).EvaluateAsync<string>($"localStorage.key({keyNumber});");

    /// <summary>
    /// Удалить элемент из localStorage по ключу.
    /// </summary>
    /// <param name="keyName">Имя ключа для удаления</param>
    /// <returns>Задача завершения удаления</returns>
    public async Task RemoveItemAsync(string keyName)
        => await (await _page.Value).EvaluateAsync($"localStorage.removeItem('{keyName}');");

    /// <summary>
    /// Установить значение элемента в localStorage.
    /// </summary>
    /// <param name="keyName">Имя ключа</param>
    /// <param name="value">Значение для установки</param>
    /// <returns>Задача завершения установки значения</returns>
    public async Task SetItemAsync(string keyName, string value)
        => await (await _page.Value).EvaluateAsync($"localStorage.setItem(\"{keyName}\", '{value}');");

    /// <summary>
    /// Освободить ресурсы и очистить localStorage.
    /// </summary>
    /// <returns>Задача завершения освобождения ресурсов</returns>
    public ValueTask DisposeAsync()
        => new ValueTask(ClearAsync());

    /// <summary>
    /// Синхронно освободить ресурсы и очистить localStorage.
    /// </summary>
    public void Dispose()
        => DisposeAsync().GetAwaiter().GetResult();
}