# Changelog

Все заметные изменения в проекте описываются в этом файле.

Формат основан на [Keep a Changelog](https://keepachangelog.com/ru/1.0.0/).

---

## [1.1.0] 15.04.2026

### ⚠ BREAKING CHANGES

#### Разделение `IBrowserFactory` на `IBrowserFactory` и `IBrowserContextFactory`

**Раньше:** `IBrowserFactory` создавал `IBrowserContext` напрямую через метод `CreateAsync()`.  
**Теперь:** `IBrowserFactory` создаёт `IBrowser`, а `IBrowserContextFactory` создаёт `IBrowserContext`.

- `IBrowserFactory.CreateAsync()` теперь возвращает `Task<IBrowser>` вместо `Task<IBrowserContext>`
- Добавлен новый интерфейс `IBrowserContextFactory` для создания контекстов
- `BrowserFactoryBase` удалён и заменён на `DefaultBrowserContextFactory`
- `ChromeFactory`, `FirefoxFactory` теперь реализуют `IBrowserFactory` (возвращают `IBrowser`, а не `IBrowserContext`)
- Добавлена `WebkitFactory` для создания браузера WebKit

**Миграция:**
```csharp
// Было: своя фабрика наследовала BrowserFactoryBase
public class MyFactory : BrowserFactoryBase { ... }

// Стало: реализовать IBrowserFactory (для браузера) или IBrowserContextFactory (для контекста)
public class MyBrowserFactory : IBrowserFactory
{
    public async Task<IBrowser> CreateAsync() { ... }
}
```

#### Добавлен слой провайдера браузера (`IBrowserGetter`)

**Раньше:** `IBrowserGetter` возвращал `IBrowserContext`.  
**Теперь:** `IBrowserGetter` возвращает `IBrowser`. Для получения контекста используется `IBrowserContextGetter`.

- `IBrowserGetter.GetAsync()` теперь возвращает `Task<IBrowser>` вместо `Task<IBrowserContext>`
- Добавлен `IBrowserContextGetter` с методом `GetAsync()`, возвращающим `Task<IBrowserContext>`
- `DefaultBrowserProvider` переименован в `DefaultBrowserContextProvider` и реализует `IBrowserContextGetter`
- Добавлены `SingletonBrowserProvider` и `TransientBrowserProvider` для управления жизненным циклом браузера

**Миграция:**
```csharp
// Было
services.GetRequiredService<IBrowserGetter>(); // возвращал IBrowserContext

// Стало
services.GetRequiredService<IBrowserContextGetter>(); // возвращает IBrowserContext
services.GetRequiredService<IBrowserGetter>();         // возвращает IBrowser
```

#### Переименования фабрик персистентных контекстов

- `PersistentChromeFactory` → `PersistentChromeContextFactory`
- `PersistentFirefoxFactory` → `PersistentFirefoxContextFactory`
- Добавлена `PersistentWebkitContextFactory`
- Все три теперь реализуют `IBrowserContextFactory` вместо `IBrowserFactory`

#### Удалён `ITestInfoGetter` и связанные классы

- Удалён интерфейс `ITestInfoGetter`
- Удалён `EmptyTestInfoProvider`
- Удалён `FixtureTracingConfigurator`
- Удалён метод `ServiceCollectionExtensions.UseTestInfoProvider<T>()`
- `DefaultTracingConfigurator` теперь содержит свойства `TestName`, `TestClassName`, `WorkDirectory` непосредственно (без зависимости от `ITestInfoGetter`)

**Миграция:**
```csharp
// Было
services.UseTestInfoProvider<NUnitTestInfoProvider>();

// Стало: наследуйте DefaultTracingConfigurator и переопределите свойства
// или создайте свою реализацию ITracingConfigurator
```

#### Изменена сигнатура `IPlaywrightFactory` → `IPlaywrightGetter`

- `IPlaywrightFactory` переименован в `IPlaywrightGetter`
- `PlaywrightFactory<T>` переименован в `PlaywrightProvider<T>`
- Пространство имён изменено с `SkbKontur.Playwright.TestCore.Factories` на `SkbKontur.Playwright.TestCore`
- `PlaywrightProvider<T>` теперь реализует `IDisposable` и `IAsyncDisposable`

#### Изменена сигнатура `AddPlaywrightTestCore`

- `AddPlaywrightTestCore<TConfig>()` заменён на `AddPlaywrightTestCore<TConfig, TBrowserProvider>()`
- По умолчанию используется `SingletonBrowserProvider`

**Миграция:**
```csharp
// Было
services.AddPlaywrightTestCore<MyConfig>();

// Стало
services.AddPlaywrightTestCore<MyConfig, SingletonBrowserProvider>();
// или для transient-поведения:
services.AddPlaywrightTestCore<MyConfig, TransientBrowserProvider>();
```

#### Изменена сигнатура `ReplaceTracing`

- `ReplaceTracing<TTracing, TConfig>()` заменён на `ReplaceTracing<TTracing, TConfig, TFailureProvider>()`
- Добавлен обязательный параметр `TFailureProvider : IFailureTestResult`

**Миграция:**
```csharp
// Было
services.ReplaceTracing<MyTracing, MyConfig>();

// Стало
services.ReplaceTracing<MyTracing, MyConfig, MyFailureProvider>();
```

#### `ContextTracing` переименован в `FullTracing`

- Класс `ContextTracing` удалён и заменён на `FullTracing` (идентичное поведение)
- Добавлен `FailureTestsTracing` — сохраняет трассировку только при падении теста

#### `DefaultPlaywrightConfiguration` больше не устанавливает `data-tid`

- `DefaultPlaywrightConfiguration.ApplyConfiguration()` теперь пустой метод (no-op)
- Для использования `data-tid` используйте `DataTidConfiguration`

**Миграция:**
```csharp
// Было: data-tid устанавливался автоматически
services.AddPlaywrightTestCore();

// Стало: явно указать DataTidConfiguration
services.AddPlaywrightTestCore<DataTidConfiguration, SingletonBrowserProvider>();
```

### Добавлено

- `IBrowserContextFactory` — интерфейс фабрики для создания контекстов браузера
- `IBrowserContextGetter` — интерфейс для получения контекста браузера
- `IBrowserGetter` — интерфейс для получения экземпляра браузера
- `IFailureTestResult` — интерфейс для определения результата теста (успех/неудача)
- `IBeforeDisposePageActions` — интерфейс для выполнения действий перед закрытием страницы
- `DefaultBrowserContextFactory` — фабрика контекстов с аутентификацией и обновлением параметров
- `DefaultBrowserContextProvider` — провайдер контекста с автоматической трассировкой
- `SingletonBrowserProvider` — singleton-провайдер браузера (статический экземпляр)
- `TransientBrowserProvider` — transient-провайдер браузера (экземпляр на провайдер)
- `FullTracing` — трассировка, сохраняющая запись всегда
- `FailureTestsTracing` — трассировка, сохраняющая запись только при падении теста
- `NoActions` — реализация `IBeforeDisposePageActions` без действий (по умолчанию)
- `WebkitFactory` — фабрика для создания браузера WebKit
- `PersistentWebkitContextFactory` — фабрика персистентных контекстов WebKit
- `DataTidConfiguration` — конфигурация Playwright с установкой атрибута `data-tid`
- `AuthWithCacheStrategy` теперь реализует `IDisposable` и `IAsyncDisposable`
- `PlaywrightProvider<T>` теперь реализует `IDisposable` и `IAsyncDisposable`

### Удалено

- `BrowserFactoryBase` — заменён на `DefaultBrowserContextFactory`
- `ContextTracing` — заменён на `FullTracing`
- `ITestInfoGetter` — метаданные теста встроены в `DefaultTracingConfigurator`
- `EmptyTestInfoProvider` — больше не нужен
- `FixtureTracingConfigurator` — удалён
- `ServiceCollectionExtensions.UseTestInfoProvider<T>()` — удалён
- `Tests/Infra/NUnitTestInfoProvider` — удалён
- `Tests/Infra/ServiceCollectionExtensions` — удалён

### Изменено

- `Microsoft.Playwright` обновлён с 1.58.0 до 1.59.0
- `LocalStorage.DisposeAsync()` теперь перехватывает исключения при очистке закрытого контекста
- `PageProvider` теперь принимает `IBrowserContextGetter` вместо `IBrowserGetter`
- `PageProvider` теперь выполняет `IBeforeDisposePageActions` перед закрытием страницы
- `DefaultPlaywrightConfiguration` теперь не содержит логики (no-op), `data-tid` вынесен в `DataTidConfiguration`
- `DefaultTracingConfigurator` больше не зависит от `ITestInfoGetter`, содержит свойства напрямую
