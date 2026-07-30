# Endless Runner - Technical Assessment

**Studio Target:** Nukebox Studios  
**Engine Version:** Unity 6000.3.12f1 (Unity 6)  
**Target Platform:** Mobile (Android / iOS friendly)  

---

## 1. Executive Architecture Overview

This project implements the core mechanics of a production-quality Endless Runner mobile game designed for scalability, zero runtime Garbage Collection (GC) allocations, and extreme modularity.

### Architectural Highlights
* **Service Locator Architecture:** System managers (`SaveManager`, `AudioManager`, `PoolManager`) are registered cleanly via `ServiceLocator` and accessed through C# interfaces (`ISaveService`, `IAudioService`), avoiding monolithic Singletons or God classes.
* **Type-Safe Generic EventBus:** Inter-system messaging utilizes a static generic `EventBus<T>` where events are immutable C# `struct` payloads. This guarantees **zero runtime GC allocations** during gameplay event dispatches.
* **Data-Driven Configuration:** All gameplay parameters, player speed, gravity multipliers, audio configurations, and spawn parameters are driven by `ScriptableObject` assets.
* **Generic Object Pooling:** Dynamic objects (track segments, obstacles, coins) are pre-warmed and recycled using a generic `PoolManager` and `IPoolable` lifecycle interface, completely eliminating dynamic `Instantiate()` or `Destroy()` calls during active runs.

---

## 2. Project Folder Structure

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
│   ├── Architecture/       # ServiceLocator & Interface abstractions
│   ├── Audio/              # AudioManager implementation
│   ├── Core/               # Bootstrapper, GameManager, InputManager
│   ├── Events/             # Generic EventBus & Event Structs
│   ├── Gameplay/           # PlayerController, TrackSpawner, Collision
│   ├── Interfaces/         # Service & Poolable contracts
│   ├── Pooling/            # GameObjectPool & PoolManager
│   ├── Save/               # JSON Save System & Persistence Data Models
│   ├── Score/              # Distance & Coin score tracker
│   ├── ScriptableObjects/  # Player, Game, Audio, and Spawn Config assets
│   └── UI/                 # UIManager, HUDView, GameOverView
└── Tests/                  # EditMode and PlayMode Unit Tests
```

---

## 3. How to Run & Controls

### Build & Run
1. Open the project in Unity 6000.3.12f1.
2. Open `Assets/Scenes/Bootstrapper.unity`.
3. Press Play in the Unity Editor.

### Player Controls

**Mobile / Touch:**
- Swipe Up: Jump
- Swipe Left / Right: Switch Lanes

**Desktop Debug Controls:**
- W / Up Arrow: Jump
- A / D or Left / Right Arrows: Switch Lanes

---

## 4. Mobile Performance Optimizations

- **Garbage Collection Minimization:** Struct-based events, stack-allocated object pools, and string-free hash lookups (`GetInstanceID()`) ensure zero GC spikes during running loops.
- **Rendering & Physics:** Hard shadows, low draw-call canvas overlays, and physics trigger masks (`LayerMask`) minimize CPU/GPU bandwidth on mobile chipsets.
- **Frame Rate Locking:** Target framerate forced to 60 FPS with VSync disabled in `Bootstrapper.cs` for mobile battery efficiency.

---

## 5. AI Usage Disclosure

- **Tools Used:** Claude 3.5 Sonnet / ChatGPT / Gemini.
- **AI Assistance:** Assisted in generating initial code boilerplates, writing Unit Test scenarios, and designing Mermaid architecture diagrams.
- **Manual Implementation & Verification:** Architecture design, interface segregation, EventBus design, mobile performance profiling, scriptable object tuning, and inspector configurations were authored and reviewed manually.

---

## 6. Known Issues & Future Improvements

**Known Issues:** None. Project compiles cleanly with zero warnings or errors on Unity 6.

**Future Improvements:**
- Integrate VContainer or Zenject for explicit Dependency Injection.
- Integrate Addressables for remote downloading of level segments and audio banks.
- Add visual particle FX pooling upon coin collection.