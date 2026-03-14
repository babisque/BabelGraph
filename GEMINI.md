# BabelGraph

## Project Purpose
`BabelGraph` is a high-performance, cross-platform desktop application designed for the architectural visualization, creation, and manipulation of diagrams. It bridges the gap between **Code-First** efficiency and **Visual-First** intuition through a hybrid synchronization engine. The application supports native `JSON` schemas while providing seamless interoperability with industry-standard formats like `Mermaid` and `Draw.io`.

Built as a standalone desktop entity, it operates without an external API, ensuring data privacy and local-first performance.

## Technology Stack
* **Core:** .NET 10+ / C# 13
* **UI Framework:** Avalonia UI (Cross-platform XAML-based)
* **Testing:** xUnit
* **Mocking:** Moq
* **Persistence:** Local File System (JSON)

## Architecture and Directory Structure
The project is strictly architected under **Domain-Driven Design (DDD)** principles to prevent anemic models and ensure business logic remains encapsulated within the Domain layer.

* `src/BabelGraph.Domain`: Pure business logic, Entities (Rich Models), Value Objects, and Domain Service interfaces.
* `src/BabelGraph.Application`: Orchestration, Use Cases, DTOs, and Mappers.
* `src/BabelGraph.Infrastructure`: File I/O, Serialization/Deserialization (JSON/Mermaid Parsers), and external integrations.
* `src/BabelGraph.Desktop`: Avalonia UI implementation, ViewModels (MVVM), and Composition Root.

The `tests/` directory mirrors this hierarchy. Every implementation project has a corresponding `*.Tests` project.

## Directory Structure

The project follows a strict separation of concerns. The `src/` folder contains the implementation, while `tests/` mirrors its structure for TDD consistency.

```text
BabelGraph/
├── src/
│   ├── BabelGraph.Domain/           # Core: Entities, Value Objects, Domain Services, Interfaces
│   ├── BabelGraph.Application/      # Orchestration: Use Cases, DTOs, Mappers, Commands/Queries
│   ├── BabelGraph.Infrastructure/   # External: File System, JSON/Mermaid Parsers, Serialization
│   └── BabelGraph.Desktop/          # UI: Avalonia Views, ViewModels, Assets, Composition Root
├── tests/
│   ├── BabelGraph.Domain.Tests/     # Unit tests for business logic and entities
│   ├── BabelGraph.Application.Tests/# Unit tests for use cases and flow orchestration
│   ├── BabelGraph.Infrastructure.Tests/ # Integration tests for parsers and I/O
│   └── BabelGraph.Shared.Tests/     # Test utilities, Builders, and Fakes
├── docs/                            # Additional documentation and design assets
├── BabelGraph.sln                   # Project Solution
└── GEMINI.md                        # Project Blueprint & Rules
```

## Engineering Principles

### 1. Hard TDD Enforcement (Red-Green-Refactor)
This project follows a **strict Test-First mandate**:
* **No Production Code First**: It is strictly forbidden to write a single line of production code (Classes, Methods, or Business Rules) before a corresponding, failing test exists.
* **Granular Testing**: Every new behavior must be preceded by a test case in the `tests/` directory that proves the necessity of that code.
* **Refactoring**: Code improvement is only allowed once the test suite is green, ensuring no regressions are introduced.

### 2. Dependency Management
* **Constructor Injection**: All services and business logic must receive their dependencies via interfaces in the constructor.
* **Mocking**: External dependencies (File System, Parsers, UI notifications) must be mocked using **Moq** during unit testing to ensure the Domain remains isolated and pure.

### 3. Clean Code & DDD
* **Rich Domain Models**: Business rules belong inside Entities and Domain Services. Anemic objects (simple getters/setters without logic) should be avoided.
* **Encapsulation**: Internal state of the diagram (coordinates, connections) must be protected against invalid mutations.

## Business Logic and Rules

### Domain Services and Use Cases
* **Bi-Directional Sync (Real-Time):** A specialized service that monitors changes in both the Text Buffer (Code) and the Canvas (Visual). It uses a **Single Source of Truth (SSoT)** pattern where the Domain Model mediates state between the two representations.
* **Multi-Format Parser (The "Babel" Layer):** A service that translates DSLs (Domain Specific Languages) like Mermaid into the internal `BabelGraph` JSON schema.
    * **Class Diagrams:** Mapping properties, methods, and access modifiers.
    * **ER Diagrams:** Mapping PKs, FKs, and cardinality relationships.
* **Heuristic Auto-Layout:** For code-imported diagrams lacking coordinates, the system implements a Sugiyama-style algorithm to calculate optimal node placement (X, Y) to minimize edge crossing.

### Application Resilience
* **Circuit-Breaker Syntax Validation:** If the Code-First editor detects a syntax error, the Domain Model enters a **"Frozen State."** The visual canvas remains locked to the last known valid state, and a diagnostic error is surfaced to the user, preventing model corruption.
* **Change Debouncing:** To optimize CPU usage on Arch Linux and other environments, the Synchronization Engine implements a 300ms debounce on text input before triggering the Parser.
* **Atomic Persistence:** Saving to the filesystem utilizes a "write-to-temp-then-swap" strategy to ensure that a crash during a save operation never results in a corrupted `.json` diagram file.

### Visual Constraints
* **Canvas Geometry:** Nodes are treated as rectangular bounds with dynamic anchor points for relationships. 
* **Export Pipeline:** High-resolution rendering for `SVG` (vector) and `PNG` (raster) must be performed off-thread to maintain UI responsiveness.