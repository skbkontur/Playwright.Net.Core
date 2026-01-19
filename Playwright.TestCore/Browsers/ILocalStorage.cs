using System.Threading.Tasks;

namespace SkbKontur.Playwright.TestCore.Browsers;

/// <summary>
/// Интерфейс для работы с localStorage браузера.
/// Предоставляет полный API для управления локальным хранилищем.
/// </summary>
public interface ILocalStorage
{
    /// <summary>
    /// Получить количество элементов в localStorage.
    /// </summary>
    /// <returns>Задача, возвращающая количество элементов</returns>
    Task<long> GetLengthAsync();

    /// <summary>
    /// Очистить все данные из localStorage.
    /// </summary>
    /// <returns>Задача завершения очистки</returns>
    Task ClearAsync();

    /// <summary>
    /// Получить значение элемента по ключу.
    /// </summary>
    /// <param name="keyName">Имя ключа</param>
    /// <returns>Задача, возвращающая значение элемента или null</returns>
    Task<string> GetItemAsync(string keyName);

    /// <summary>
    /// Получить имя ключа по индексу.
    /// </summary>
    /// <param name="keyNumber">Индекс ключа (начиная с 0)</param>
    /// <returns>Задача, возвращающая имя ключа</returns>
    Task<string> GetKeyAsync(int keyNumber);

    /// <summary>
    /// Удалить элемент из localStorage по ключу.
    /// </summary>
    /// <param name="keyName">Имя ключа для удаления</param>
    /// <returns>Задача завершения удаления</returns>
    Task RemoveItemAsync(string keyName);

    /// <summary>
    /// Установить значение элемента в localStorage.
    /// </summary>
    /// <param name="keyName">Имя ключа</param>
    /// <param name="value">Значение для установки</param>
    /// <returns>Задача завершения установки значения</returns>
    Task SetItemAsync(string keyName, string value);
}