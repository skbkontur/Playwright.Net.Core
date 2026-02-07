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
- Использует паттерн Factory для создания компонентов
- Поддерживает различные стратегии аутентификации
- Обеспечивает thread-safety и lazy loading для тяжёлых объектов
- Интегрируется с различными тестовыми фреймворками (NUnit, xUnit, MSTest)

---

### Основные компоненты

## 1. Фабрики (Factories)

### IPlaywrightFactory / PlaywrightFactory\<TConfiguration\>
Создание и управление экземпляром Playwright.
- Кэширует экземпляр Playwright
- Применяет конфигурацию через generic параметр `TConfiguration`
- Использует lazy loading для инициализации

### IBrowserFactory / BrowserFactoryBase
Абстрактная фабрика для создания браузерных контекстов.
- Поддерживает различные браузеры (Chrome, Firefox)
- Интегрируется со стратегиями аутентификации
- Управляет жизненным циклом контекстов браузера

### ChromeFactory, FirefoxFactory
Конкретные реализации для запуска браузеров.
- Используют `IBrowserConfigurator` для настроек запуска
- Создают новые контексты для каждого теста

### PersistentChromeFactory, PersistentFirefoxFactory
Создание персистентных контекстов браузеров.
- Используют отдельные директории пользователя
- Сохраняют состояние между запусками тестов
- Полезны для работы с расширениями браузера

### PageObjectsFactory
Основная фабрика для создания Page Objects и элементов управления.
- Реализует интерфейсы: `IPageFactory`, `IControlFactory`, `IPageObjectsFactory`
- Создаёт page objects и элементы управления с автоматическим внедрением зависимостей
- Поддерживает создание коллекций элементов через `ElementsCollection<TItem>`

---

## 2. Стратегии аутентификации (Auth)

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
- Значительно ускоряет выполнение тестов

### WithoutAuthStrategy / WithoutAuthAuthenticator
Пустые реализации для сценариев без аутентификации.
- Используются по умолчанию
- Не выполняют никаких действий по аутентификации

---

## 3. Конфигурации (Configurations)

### IPlaywrightConfiguration / DefaultPlaywrightConfiguration
Настройка глобальных параметров Playwright.
- Устанавливает атрибут для поиска элементов (по умолчанию `data-tid`)
- Настраивает таймауты и другие глобальные параметры
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

### ITracingConfigurator / DefaultTracingConfigurator / FixtureTracingConfigurator
Конфигурация трассировки Playwright.
- Метод `GetTracingStartOptions()` - опции запуска трассировки
- Метод `GetTracingStopOptions()` - опции остановки и сохранения трассировки
- Сохраняет трассировку в ZIP файлы с названиями тестов
- `FixtureTracingConfigurator` - сохраняет трассировку для всего класса тестов

### ITestInfoGetter
Предоставление метаданных о текущем тесте.
- ID теста
- Имя теста
- Класс теста
- Рабочая директория для артефактов
- Используется для именования файлов трассировки

---

## 4. Компоненты браузеров (Browsers)

### IBrowserGetter / DefaultBrowserProvider
Создание и управление контекстами браузеров.
- Создают новый контекст браузера для каждого теста
- Интегрируют трассировку автоматически
- Применяют `IContextOptionsUpdater` перед созданием контекста

### IContextTracing / ContextTracing
Управление трассировкой на уровне контекста браузера.
- Стартуют запись трассировки при создании контекста
- Останавливают и сохраняют трассировку после теста
- Интегрируются с `ITracingConfigurator`

### ILocalStorage / LocalStorage
Доступ к localStorage браузера.
- Методы: `Get`, `Set`, `Remove`, `Clear`
- Полная реализация API localStorage
- Работает через JavaScript-инъекции в контекст страницы

---

## 5. Компоненты страниц (Pages)

### IPageGetter / PageProvider
Получение активной страницы браузера.
- Возвращает текущую активную страницу
- Создаёт новую страницу при необходимости
- Интегрируется с контекстом браузера

### ILoadable
Интерфейс для объектов, требующих асинхронной загрузки.
- Метод `WaitLoadAsync()` - ожидание загрузки объекта
- Используется в page objects для ожидания загрузки страницы

---

## 6. Коллекции (Collections)

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

## 7. Система зависимостей (Dependencies)

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

## 8. Инфраструктура и расширения

### ServiceCollectionExtensions
Расширения для `IServiceCollection` для регистрации компонентов в DI.
- `AddPlaywrightTestCore<TTestInfoGetter>()` - регистрация базовых компонентов
- `UsePom()` - регистрация инфраструктуры Page Object Model
- `UseTestInfoProvider<T>()` - регистрация провайдера информации о тестах
- `UseBrowser<TFactory, TConfigurator, TUpdater>()` - настройка браузера
- `UseAuthenticator<TAuthenticator, TStrategy>()` - настройка аутентификации
- `ReplaceTracing<TTracing, TConfigurator>()` - замена реализации трассировки

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

#### 2. Настройка информации о тестах (ITestInfoGetter)

По умолчанию библиотека использует `EmptyTestInfoProvider`. Чтобы трассировки и отчеты содержали корректные имена тестов, необходимо подключить реализацию, специфичную для вашего фреймворка (NUnit, xUnit или MSTest), либо использовать дефолтный провайдер.

```csharp
// Пример подключения своего провайдера метаданных тестов
services.UseTestInfoProvider<DefaultTestInfoProvider>();
```

#### 3. Настройка сценариев аутентификации

Если ваши тесты требуют авторизации, замените стандартный «пустой» аутентификатор на ваш рабочий:

```csharp
services.UseAuthenticator<MyProjectAuthenticator, AuthWithCacheStrategy>();
```

#### 4. Переопределение настроек браузера

Для запуска тестов в конкретном режиме (например, в Headful для локальной отладки) используйте метод `UseBrowser`:

```csharp
services.UseBrowser<ChromeFactory, HeadfulConfigurator, ViewportSizeUpdater>();
```

#### 5. Кастомизация трассировок

Вы можете переопределить логику сохранения трассировок (скриншотов, видео, логов Playwright):

```csharp
services.ReplaceTracing<CustomContextTracing, DefaultTracingConfigurator>();
```

#### Пример полной конфигурации

Использование Fluent API позволяет настроить весь стек одной цепочкой:

```csharp
var serviceProvider = new ServiceCollection()
    .AddPlaywrightTestCore<DefaultPlaywrightConfiguration>() // Настройка базовых параметров
    .UseTestInfoProvider<NUnitTestInfoProvider>()            // Интеграция с NUnit
    .UseBrowser<ChromeFactory, HeadlessConfigurator, ViewportSizeUpdater>() // Chrome headless
    .UseAuthenticator<IdentityServerAuthenticator, AuthWithCacheStrategy>() // Авторизация с кэшем
    .UsePom()                                                // Page Object Model
    .BuildServiceProvider();
```

---

## Пример тестового проекта

### ServiceCollectionExtensions.UsePlaywright()

Расширение для регистрации всех компонентов TestCore в DI контейнере с поддержкой NUnit:

```csharp
public static IServiceCollection UsePlaywright(this IServiceCollection services)
{
    return services
        .AddPlaywrightTestCore<DefaultPlaywrightConfiguration>()
        .UseTestInfoProvider<NUnitTestInfoProvider>()
        .UseBrowser<ChromeFactory, HeadlessConfigurator, ViewportSizeUpdater>()
        .UsePom();
}
```

### TestInfoGetter

Адаптер для получения метаданных тестов из NUnit TestContext:

```csharp
public class NUnitTestInfoProvider : ITestInfoGetter
{
    public string GetTestId() => TestContext.CurrentContext.Test.ID;
    public string GetTestName() => TestContext.CurrentContext.Test.Name;
    public string GetTestClassName() => TestContext.CurrentContext.Test.ClassName;
    public string GetWorkDirectory() => TestContext.CurrentContext.WorkDirectory;
}
```

### Базовый класс для тестов

Стандартный базовый класс демонстрирующий использование инфраструктуры:

```csharp
public class BaseTest
{
    protected IServiceProvider ServiceProvider { get; private set; }
    protected IPageFactory PageFactory { get; private set; }
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        ServiceProvider = new ServiceCollection()
            .UsePlaywright()
            .BuildServiceProvider();
            
        PageFactory = ServiceProvider.GetRequiredService<IPageFactory>();
    }
    
    [Test]
    public async Task ExampleTest()
    {
        var page = PageFactory.Create<MainPage>();
        await page.NavigateAsync();
        await page.SearchBox.FillAsync("Playwright");
        // ...
    }
}
```

---

## Ключевые особенности архитектуры

1. **Dependency Injection в основе** - все компоненты разрешаются через DI контейнер
2. **Lazy loading** - тяжёлые объекты (Playwright, браузеры) создаются по требованию
3. **Thread safety** - стратегии аутентификации защищены от конкурентного доступа
4. **Расширяемость** - интерфейсы позволяют легко заменять реализации
5. **Гибкая конфигурация** - Fluent API для настройки всех компонентов
6. **Интеграция с Playwright** - полная поддержка всех возможностей Playwright
7. **POM паттерн** - поддержка создания типизированных page objects и page elements
8. **Tracing и отладка** - встроенная поддержка трассировки для debugging
9. **CI/CD готовность** - автоматическое определение headless режима в CI окружении
10. **Фреймворк-агностичность** - поддержка разных фреймворков тестирования (NUnit, xUnit, MSTest)

---

## Заключение

Эта инфраструктура предоставляет полноценное решение для E2E тестирования веб-приложений с Microsoft Playwright, следуя современным принципам архитектуры и обеспечивая высокую поддерживаемость и расширяемость кода тестов.
