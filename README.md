# SkbKontur.Playwright.TestCore и SkbKontur.Playwright.POM.Abstractions

### SkbKontur.Playwright.POM.Abstractions

**Цель:** Базовые абстракции для реализации паттерна Page Object Model (POM) с использованием паттерна обёртки (Wrapper pattern).

**Компоненты:**

1. **IWrapper<T>** - Базовый интерфейс обёрточного паттерна
   - Предоставляет доступ к обёрнутому объекту через свойство `WrappedItem`

2. **ILocatorWrapper<TLocator>** - Интерфейс для обёрток Playwright локаторов
   - Наследуется от `IWrapper<TLocator>` где `TLocator` - тип локатора Playwright

3. **IPageWrapper<TPage>** - Интерфейс для обёрток страниц Playwright
   - Наследуется от `IWrapper<TPage>` где `TPage` - тип страницы Playwright
   - Дополнительно предоставляет свойство `Url` для получения URL страницы

### SkbKontur.Playwright.TestCore

**Цель:** Инфраструктура для запуска E2E тестов на базе Playwright с поддержкой Dependency Injection и паттерна Page Object Model.

**Архитектура:**
- Построена на принципах Dependency Injection
- Использует паттерн Factory для создания компонентов
- Поддерживает различные стратегии аутентификации

**Основные компоненты:**

#### 1. Фабрики (Factories)

**IPlaywrightFactory / PlaywrightFactory<TConfiguration>**
- Создаёт и кэширует экземпляр Playwright
- Применяет конфигурацию через generic параметр
- Использует lazy loading для инициализации

**IBrowserFactory / BrowserFactoryBase**
- Абстрактная фабрика для создания браузерных контекстов
- Поддерживает различные браузеры (Chrome, Firefox)
- Интегрируется со стратегиями аутентификации

**ChromeFactory, FirefoxFactory**
- Конкретные реализации для запуска Chrome/Firefox браузеров
- Используют IBrowserConfigurator для настроек запуска

**PersistentChromeFactory, PersistentFirefoxFactory**
- Создают персистентные контексты браузеров с отдельными директориями пользователей
- Используются для сохранения состояния между тестами

**PageObjectsFactory**
- Реализует IPageFactory, IControlFactory, IPageObjectsFactory
- Создаёт page objects и элементы управления с dependency injection
- Поддерживает создание коллекций элементов

#### 2. Стратегии аутентификации (Auth)

**IAuthStrategy / IAutentificator**
- Определяют интерфейсы для работы с аутентификацией
- Поддерживают кэширование состояния браузера

**AuthWithCacheStrategy**
- Кэширует состояние аутентификации между тестами
- Использует двойную проверку блокировки для thread safety

**WithoutAuthStrategy / WithoutAuthAutentificator**
- Пустые реализации для сценариев без аутентификации

#### 3. Конфигурации (Configurations)

**IPlaywrightConfiguration / DefaultPlaywrightConfiguration**
- Настраивают глобальные параметры Playwright
- Устанавливают атрибут для поиска элементов по data-tid

**IBrowserConfigurator / HeadlessConfigurator**
- Определяют параметры запуска браузеров
- Автоматически определяют headless режим в CI среде

**ITracingConfigurator / DefaultTracingConfigurator / FixtureTracingConfigurator**
- Конфигурируют трассировку Playwright
- Сохраняют трассировку в ZIP файлы с названиями тестов

**ITestInfoGetter**
- Предоставляет информацию о текущем тесте (ID, имя, класс, рабочая директория)

#### 4. Компоненты браузеров (Browsers)

**IBrowserGetter / DefaultBrowserGetter**
- Создают и управляют контекстами браузеров
- Интегрируют трассировку автоматически

**IContextTracing / ContextTracing**
- Управляют трассировкой на уровне контекста браузера
- Стартуют и останавливают запись трассировки

**ILocalStorage / LocalStorage**
- Предоставляют доступ к localStorage браузера
- Реализуют полный API localStorage (get, set, remove, clear)

#### 5. Компоненты страниц (Pages)

**IPageGetter / PageGetter**
- Получают активную страницу браузера
- Создают новую страницу при необходимости

**ILoadable**
- Интерфейс для объектов, требующих асинхронной загрузки

#### 6. Коллекции (Collections)

**ElementsCollection<TItem>**
- Представляет коллекцию элементов на странице
- Поддерживает фильтрацию, доступ по индексу
- Реализует IEnumerable для итерации

**ElementsCollectionExtensions**
- Расширения для работы с коллекциями
- Предоставляют методы для проверки текста элементов с учётом порядка

#### 7. Система зависимостей (Dependencies)

**IDependenciesFactory / DependencyFactory**
- Создают зависимости для конструкторов page objects
- Используют IServiceProvider для разрешения зависимостей

**IDependenciesFilter / DefaultDependenciesFilter**
- Фильтруют параметры конструкторов
- Исключают базовые типы Playwright (IPage, ILocator) из DI

#### 8. Navigation и инфраструктура

**NavigationFactory**
- Статическая фабрика для создания Navigation с предустановленными зависимостями

**BrowsersInstaller**
- Устанавливает браузеры Playwright перед запуском тестов

### Тестовый проект (Tests)

**ServiceCollectionExtensions.UsePlaywright()**
- Расширения для регистрации всех компонентов TestCore в DI контейнере
- Настраивает scoped и singleton сервисы

**DependencyFactory, TestInfoGetter**
- Адаптеры для интеграции с NUnit и Microsoft.Extensions.DependencyInjection

**RunPwShould**
- Базовый тестовый класс демонстрирующий использование инфраструктуры
- Показывает работу с Navigation, локаторами и assertions

### Ключевые особенности архитектуры:

1. **Dependency Injection в основе** - все компоненты разрешаются через DI
2. **Lazy loading** - тяжёлые объекты (Playwright, браузеры) создаются по требованию
3. **Thread safety** - стратегии аутентификации защищены от конкурентного доступа
4. **Расширяемость** - интерфейсы позволяют легко заменять реализации
5. **Интеграция с Playwright** - полная поддержка всех возможностей Playwright
6. **POM паттерн** - поддержка создания типизированных page objects
7. **Tracing и отладка** - встроенная поддержка трассировки для debugging
8. **CI/CD готовность** - автоматическое определение headless режима

Эта инфраструктура предоставляет полноценное решение для E2E тестирования веб-приложений с Playwright, следуя современным принципам архитектуры и обеспечивая высокую поддерживаемость и расширяемость кода тестов.