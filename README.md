# SkbKontur.Playwright.TestCore и SkbKontur.Playwright.POM.Abstractions

## Описание

Библиотеки для организации E2E тестирования на базе Microsoft Playwright с поддержкой паттерна Page Object Model (POM), Dependency Injection и гибкой конфигурации браузеров.

---

## SkbKontur.Playwright.POM.Abstractions

### Назначение

Базовые абстракции для реализации паттерна Page Object Model с использованием паттерна обёртки (Wrapper). Позволяет интегрировать слой POM вашей инфраструктуры с SkbKontur.Playwright.TestCore.

### Компоненты

#### 1. IWrapper\<T\>
Базовый интерфейс обёртки.
- Предоставляет доступ к обёрнутому объекту через свойство `WrappedItem`

#### 2. ILocatorWrapper\<TLocator\>
Интерфейс для PageElements - обёрток локаторов Playwright.
- Наследуется от `IWrapper<TLocator>`, где `TLocator` - тип локатора Playwright
- Используется для создания типизированных элементов управления

#### 3. IPageWrapper\<TPage\>
Интерфейс для PageObjects - обёрток страниц Playwright.
- Наследуется от `IWrapper<TPage>`, где `TPage` - тип страницы Playwright
- Предоставляет свойство `Url` для получения URL страницы

#### 4. IPageFactory\<in TWrappedItem\>
Интерфейс для создания страниц (PageObjects).
- Методы:
  - `Create<TPage>(TWrappedItem page)` - создание из готового объекта
  - `Create<TPage>(Func<TWrappedItem> getPage)` - создание с lazy-получением

#### 5. IControlFactory\<in TWrappedItem\>
Интерфейс для создания контролов (PageElements).
- Методы:
  - `Create<TControl>(TWrappedItem locator)` - создание из локатора
  - `Create<TControl>(Func<TWrappedItem> getLocator)` - создание с lazy-получением локатора
  - `Create<TControl>(ILocatorWrapper<TWrappedItem> locatorWrapper, string dataTestId)` - создание с data-test-id через обёртку
  - `Create<TControl>(TWrappedItem locator, string dataTestId)` - создание с data-test-id

---

## SkbKontur.Playwright.TestCore

### Назначение

Инфраструктура для запуска E2E тестов на базе Playwright с поддержкой Dependency Injection и паттерна Page Object Model.

### Архитектура

- Построена на принципах Dependency Injection
- Трёхуровневая модель: Playwright → Browser → BrowserContext → Page
- Поддерживает различные стратегии аутентификации
- Обеспечивает thread-safety и lazy loading для тяжёлых объектов
- Расширяемая система трассировки и действий при закрытии страниц

---

### Основные компоненты

## 1. Провайдеры (Providers)

### IPlaywrightGetter / PlaywrightProvider\<TConfiguration\>
Создание и управление экземпляром Playwright.
- Кэширует экземпляр Playwright
- Применяет конфигурацию через generic параметр `TConfiguration`
- Использует lazy loading для инициализации
- Реализует `IDisposable` и `IAsyncDisposable`

### IBrowserGetter / SingletonBrowserProvider / TransientBrowserProvider
Получение и управление экземпляром браузера.
- `SingletonBrowserProvider` — один экземпляр браузера на весь процесс (статическое поле)
- `TransientBrowserProvider` — экземпляр браузера привязан к конкретному провайдеру
- Потокобезопасная инициализация через `SemaphoreSlim`

### IBrowserContextGetter / DefaultBrowserContextProvider
Управление жизненным циклом контекста браузера.
- Создаёт контекст через `IBrowserContextFactory`
- Автоматически запускает и останавливает трассировку
- Управляет очисткой ресурсов при `Dispose`

---

## 2. Фабрики (Factories)

### IBrowserFactory / ChromeFactory, FirefoxFactory, WebkitFactory
Фабрики для создания экземпляров браузеров.
- Возвращают `IBrowser` (не контекст)
- Используют `IBrowserConfigurator` для настроек запуска
- Поддерживают Chrome (Chromium), Firefox и WebKit

### IBrowserContextFactory / DefaultBrowserContextFactory
Фабрика для создания контекстов браузера.
- Получает `IBrowser` через `IBrowserGetter`
- Применяет стратегию аутентификации
- Обновляет параметры контекста через `IContextOptionsUpdater`

### PersistentChromeContextFactory, PersistentFirefoxContextFactory, PersistentWebkitContextFactory
Создание персистентных контекстов браузеров.
- Используют отдельные директории пользователя
- Реализуют `IBrowserContextFactory`
- Полезны для работы с расширениями браузера

### PageObjectsFactory
Основная фабрика для создания Page Objects и элементов управления.
- Реализует интерфейсы: `IPageFactory`, `IControlFactory`, `IPageObjectsFactory`
- Создаёт page objects и элементы управления с автоматическим внедрением зависимостей
- Поддерживает создание коллекций элементов через `ElementsCollection<TItem>`

---

## 3. Стратегии аутентификации (Auth)

### IAuthStrategy
Определяет интерфейс стратегии аутентификации.
- Предоставляет методы для получения аутентификатора

### IAuthenticator
Определяет интерфейс аутентификатора.
- Метод `CreateStorageStateAsync()` - создание состояния аутентификации (cookies, localStorage)
- Используется для кэширования состояния между тестами

### AuthWithCacheStrategy
Стратегия с кэшированием состояния аутентификации.
- Кэширует состояние аутентификации между тестами
- Использует двойную проверку блокировки для thread-safety
- Реализует `IDisposable` и `IAsyncDisposable`

### WithoutAuthStrategy / WithoutAuthAuthenticator
Пустые реализации для сценариев без аутентификации.
- Используются по умолчанию
- Не выполняют никаких действий по аутентификации

---

## 4. Конфигурации (Configurations)

### IPlaywrightConfiguration / DefaultPlaywrightConfiguration / DataTidConfiguration
Настройка глобальных параметров Playwright.
- `DefaultPlaywrightConfiguration` — пустая конфигурация без параметров
- `DataTidConfiguration` — устанавливает атрибут `data-tid` для поиска элементов
- Применяется при инициализации Playwright

### IBrowserConfigurator / HeadlessConfigurator
Настройка параметров запуска браузеров.
- Метод `GetLaunchOptions()` - опции запуска браузера
- Метод `GetLaunchPersistentContextOptions()` - опции персистентного контекста
- `HeadlessConfigurator` автоматически определяет headless режим в CI среде

### IContextOptionsUpdater / ViewportSizeUpdater
Обновление настроек контекста браузера.
- Метод `ExecuteAsync(BrowserNewContextOptions options)` - модификация опций контекста
- `ViewportSizeUpdater` - устанавливает размер viewport браузера
- Применяется перед созданием контекста

### ITracingConfigurator / DefaultTracingConfigurator
Конфигурация трассировки Playwright.
- Метод `GetTracingStartOptions()` - опции запуска трассировки
- Метод `GetTracingStopOptions()` - опции остановки и сохранения трассировки
- `DefaultTracingConfigurator` содержит свойства `TestName`, `TestClassName`, `WorkDirectory` с значениями по умолчанию
- Сохраняет трассировку в ZIP файлы с названиями тестов

---

## 5. Трассировка (Tracing)

### IContextTracing / FullTracing / FailureTestsTracing
Управление трассировкой на уровне контекста браузера.
- `FullTracing` — записывает и сохраняет трассировку всегда
- `FailureTestsTracing` — сохраняет трассировку только при падении теста (использует `IFailureTestResult`)
- Стартуют запись трассировки при создании контекста
- Останавливают и сохраняют трассировку после теста

### IFailureTestResult
Интерфейс для определения результата выполнения теста.
- Используется `FailureTestsTracing` для условной записи трассировок
- Реализация зависит от тестового фреймворка (NUnit, xUnit, MSTest)

---

## 6. Компоненты браузеров (Browsers)

### ILocalStorage / LocalStorage
Доступ к localStorage браузера.
- Методы: `Get`, `Set`, `Remove`, `Clear`
- Полная реализация API localStorage
- Работает через JavaScript-инъекции в контекст страницы

---

## 7. Компоненты страниц (Pages)

### IPageGetter / PageProvider
Получение активной страницы браузера.
- Возвращает текущую активную страницу
- Создаёт новую страницу при необходимости
- Выполняет `IBeforeDisposePageActions` перед закрытием страницы

### IBeforeDisposePageActions / NoActions
Расширение поведения при закрытии страницы.
- Позволяет выполнить действия перед закрытием (например, сохранить скриншот)
- `NoActions` — реализация по умолчанию (ничего не делает)
- Регистрируется через DI, поддерживает множественные регистрации

### ILoadable
Интерфейс для объектов, требующих асинхронной загрузки.
- Метод `WaitLoadAsync()` - ожидание загрузки объекта
- Используется в page objects для ожидания загрузки страницы

---

## 8. Коллекции (Collections)

### ElementsCollection\<TItem\>
Представление коллекции элементов на странице.
- Параметры конструктора: `ILocator locator`, `IControlFactory controlFactory`
- Поддерживает итерацию через `IEnumerable<TItem>`
- Методы:
  - `GetEnumerator()` - получение енумератора для итерации
  - `Expect()` - получение `ILocatorAssertions` для проверок Playwright

### ElementsCollectionExtensions
Расширения для работы с коллекциями элементов.
- Методы для проверки текста элементов с учётом порядка
- Упрощённые методы фильтрации и доступа

---

## 9. Система зависимостей (Dependencies)

### IDependenciesFactory / DependencyFactory
Создание зависимостей для конструкторов page objects.
- Метод `CreateDependency(Type controlType)` - создание массива зависимостей
- Использует `IServiceProvider` для разрешения зависимостей
- Работает с рефлексией для анализа конструкторов

### IDependenciesFilter / DefaultDependenciesFilter
Фильтрация параметров конструкторов.
- Определяет, какие параметры должны разрешаться через DI
- Исключает базовые типы Playwright (`IPage`, `ILocator`) из DI
- Позволяет настраивать правила разрешения зависимостей

---

## 10. Инфраструктура и расширения

### ServiceCollectionExtensions
Расширения для `IServiceCollection` для регистрации компонентов в DI.
- `AddPlaywrightTestCore()` — регистрация с конфигурацией по умолчанию (`DefaultPlaywrightConfiguration`, `SingletonBrowserProvider`)
- `AddPlaywrightTestCore<TConfig, TBrowserProvider>()` — регистрация с указанной конфигурацией и провайдером браузера
- `UsePom()` — регистрация инфраструктуры Page Object Model
- `UseBrowser<TFactory, TConfigurator, TUpdater>()` — настройка фабрики браузера
- `UseAuthenticator<TAuthenticator, TStrategy>()` — настройка аутентификации
- `ReplaceTracing<TTracing, TConfigurator, TFailureProvider>()` — замена реализации трассировки

### BrowsersInstaller
Установка браузеров Playwright.

---

## Инструкция по настройке Playwright TestCore

### Регистрация в DI контейнере

Данная библиотека использует Dependency Injection (DI) для управления жизненным циклом браузера, страниц и аутентификации.

#### 1. Базовая регистрация

В методе инициализации вашего тестового проекта (например, в `GlobalSetup` или базовом классе тестов) создайте `ServiceCollection` и вызовите метод `AddPlaywrightTestCore`.

```csharp
var services = new ServiceCollection();

services.AddPlaywrightTestCore() // Регистрация базовой инфраструктуры
        .UsePom();               // Регистрация инфраструктуры Page Object Model
```

По умолчанию используется `DefaultPlaywrightConfiguration` (пустая конфигурация) и `SingletonBrowserProvider` (один браузер на процесс).

#### 2. Настройка конфигурации Playwright

Для установки атрибута `data-tid` для поиска элементов:

```csharp
services.AddPlaywrightTestCore<DataTidConfiguration, SingletonBrowserProvider>();
```

#### 3. Выбор провайдера браузера

- `SingletonBrowserProvider` — один экземпляр браузера на весь процесс (по умолчанию, быстрее)
- `TransientBrowserProvider` — экземпляр браузера на каждый провайдер

```csharp
services.AddPlaywrightTestCore<DataTidConfiguration, TransientBrowserProvider>();
```

#### 4. Настройка сценариев аутентификации

Если ваши тесты требуют авторизации, замените стандартный «пустой» аутентификатор на ваш рабочий:

```csharp
services.UseAuthenticator<MyProjectAuthenticator, AuthWithCacheStrategy>();
```

#### 5. Переопределение настроек браузера

Для запуска тестов в конкретном режиме (например, в Headful для локальной отладки) используйте метод `UseBrowser`:

```csharp
services.UseBrowser<ChromeFactory, HeadfulConfigurator, ViewportSizeUpdater>();
```

Поддерживаемые фабрики: `ChromeFactory`, `FirefoxFactory`, `WebkitFactory`.

#### 6. Кастомизация трассировок

Вы можете переопределить логику сохранения трассировок:

```csharp
// Запись трассировки только при падении теста
services.ReplaceTracing<FailureTestsTracing, MyTracingConfigurator, MyFailureTestResult>();
```

#### 7. Действия перед закрытием страницы

Для сохранения скриншотов или сбора данных перед закрытием:

```csharp
// Замените NoActions на свою реализацию
services.AddScoped<IBeforeDisposePageActions, ScreenshotOnFailure>();
```

#### Пример полной конфигурации

```csharp
var serviceProvider = new ServiceCollection()
    .AddPlaywrightTestCore<DataTidConfiguration, SingletonBrowserProvider>()
    .UseBrowser<ChromeFactory, HeadlessConfigurator, ViewportSizeUpdater>()
    .UseAuthenticator<IdentityServerAuthenticator, AuthWithCacheStrategy>()
    .ReplaceTracing<FailureTestsTracing, MyTracingConfigurator, NUnitFailureTestResult>()
    .UsePom()
    .BuildServiceProvider();
```

---

## Пример тестового проекта

### Базовая регистрация

```csharp
public static IServiceCollection UsePlaywright(this IServiceCollection services)
{
    return services
        .AddPlaywrightTestCore<DataTidConfiguration, SingletonBrowserProvider>()
        .UseBrowser<ChromeFactory, HeadlessConfigurator, ViewportSizeUpdater>()
        .UsePom();
}
```

### Базовый класс для тестов

```csharp
[Parallelizable(ParallelScope.All)]
public class RunPwShould
{
    private static readonly IServiceProvider ServiceProvider = new ServiceCollection()
        .AddPlaywrightTestCore()
        .UsePom()
        .BuildServiceProvider();

    [Test]
    public async Task BeSuccess_FromAsyncScope()
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var navigation = services.GetRequiredService<Navigation>();
        var page = await navigation.GoToUrlAsync("https://kontur.ru");
        var header = page.Locator("h1", new() { HasText = "Экосистема" });
        await Assertions.Expect(header).ToContainTextAsync("для бизнеса");
    }
}
```

---

## Ключевые особенности архитектуры

1. **Dependency Injection в основе** — все компоненты разрешаются через DI контейнер
2. **Трёхуровневая модель** — Playwright → Browser (IBrowserGetter) → BrowserContext (IBrowserContextGetter) → Page (IPageGetter)
3. **Lazy loading** — тяжёлые объекты (Playwright, браузеры) создаются по требованию
4. **Thread safety** — Singleton/Transient провайдеры защищены SemaphoreSlim
5. **Расширяемость** — интерфейсы позволяют легко заменять реализации на любом уровне
6. **Гибкая трассировка** — запись всегда (`FullTracing`) или только при падении (`FailureTestsTracing`)
7. **Хуки закрытия страниц** — `IBeforeDisposePageActions` для скриншотов, логов и пр.
8. **POM паттерн** — поддержка создания типизированных page objects и page elements
9. **CI/CD готовность** — автоматическое определение headless режима в CI окружении
10. **Мультибраузерность** — поддержка Chrome, Firefox и WebKit
