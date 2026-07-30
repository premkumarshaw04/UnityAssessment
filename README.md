# Endless Runner - Technical Assessment

**Studio Target:** Nukebox Studios  
**Engine Version:** Unity 6 (6000.3.12f1)  
**Target Platform:** Android & iOS (Mobile)

---

# 1. Executive Architecture Overview

This project implements the core mechanics of a modular Endless Runner game using Unity 6. The architecture emphasizes maintainability, scalability, event-driven communication, and object reuse through pooling.

## Architectural Highlights

- **Service Locator Architecture:** Core services (`SaveManager`, `AudioManager`, `PoolManager`) are registered through a centralized `ServiceLocator` and accessed via interfaces, promoting loose coupling.
- **Type-Safe Generic EventBus:** Gameplay systems communicate using a generic `EventBus<T>` with immutable event structs, reducing dependencies between systems.
- **Data-Driven Configuration:** Gameplay values such as player movement, spawning, audio, and scoring are configured using `ScriptableObject` assets.
- **Object Pooling:** Track segments, obstacles, and collectibles are recycled through a generic pooling system to reduce runtime object creation and destruction.

---

# 2. Project Folder Structure

```text
Assets/
├── Art/
│   ├── Animations/
│   ├── Audio/
│   ├── Fonts/
│   ├── Materials/
│   ├── Prefabs/
│   │   ├── Gameplay/
│   │   ├── Level/
│   │   ├── Player/
│   │   └── UI/
│   └── Textures/
├── Core/
│   └── Settings/
├── Scenes/
│   ├── Bootstrapper.unity
│   └── Gameplay.unity
├── Scripts/
│   ├── Architecture/
│   ├── Audio/
│   ├── Core/
│   ├── Events/
│   ├── Gameplay/
│   ├── Interfaces/
│   ├── Pooling/
│   ├── Save/
│   ├── Score/
│   ├── ScriptableObjects/
│   └── UI/
└── Tests/
    └── EditMode/
```

---

# 3. How to Run

## Build & Run

1. Open the project in **Unity 6 (6000.3.12f1)**.
2. Open **Assets/Scenes/Bootstrapper.unity**.
3. Press **Play**.

## Controls

### Mobile

- Swipe Up — Jump

### Desktop

- W or Up Arrow — Jump

> **Note:** Lane switching input is planned but is not fully connected in the current implementation.

---

# 4. Performance Optimizations

- Event-driven architecture using lightweight event structs.
- Object pooling to reuse gameplay objects and reduce runtime allocations.
- ScriptableObject-based configuration to separate data from logic.
- Target frame rate locked to **60 FPS** with VSync disabled inside `Bootstrapper.cs` for mobile optimization.

---

# 5. AI Usage Disclosure

### Tools Used

- ChatGPT
- Gemini

### AI Assistance

AI tools were used to:

- Generate initial code structure and boilerplate.
- Suggest architecture and design patterns.
- Assist with documentation.
- Provide sample unit tests.
- Review implementation approaches.

### Manual Work

The following work was completed manually:

- Integrating all scripts into the Unity project.
- Organizing the project architecture.
- Resolving Unity 6 compatibility issues.
- Replacing the original pooling implementation with a Unity 6-compatible version.
- Verifying compilation and fixing project errors.
- Configuring project structure and preparing the repository for submission.

---

# 6. Unit Tests

EditMode unit tests are included for:

- SaveManager
- EventBus

These verify save/load functionality and event publishing/subscribing behavior.

---

# 7. Future Improvements

- Complete lane switching input implementation.
- Add visual effects and particle pooling.
- Integrate Addressables for content management.
- Replace Service Locator with a dependency injection framework such as Zenject or VContainer.
- Add PlayMode tests for gameplay systems.
- Improve UI polish and gameplay balancing.

---

# 8. Notes

The project compiles successfully under **Unity 6** and is structured with modular gameplay systems to support future expansion and maintenance.