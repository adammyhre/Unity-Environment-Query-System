# Unity C# Environment Query System (EQS)
![EQS](https://github.com/user-attachments/assets/dfb6ac88-74f2-47a3-8703-2e31385b3081)

## Description

This Environment Query System (EQS) provides a data-driven way for AI to find optimal positions in the game world. Inspired by Unreal Engine's EQS, it uses **generators** to produce candidate points, **tests** to score or filter them, and **context** (querier and target) so the same query can power cover-seeking, attack positioning, patrol points, and more—without hard-coding behavior.

All query configuration lives in ScriptableObject assets: assign a query to a runner, optionally set a target, and read the best position (or full result) each frame or on demand.

## Code Structure

**Core types**
- `QueryPoint.cs` – Struct for a single candidate (Position, Normal, Score, Valid).
- `EQSContext.cs` – Runtime context (querier/target positions and transforms); `From(querier, target)` and `Refresh()`.
- `EQSRunResult.cs` – Result of a query (Success, BestPosition, BestScore, BestIndex, AllCandidates for gizmos).

**Contracts**
- `IEQSGenerator.cs` – Interface: `Generate(context)` returns a list of `QueryPoint`.
- `IEQSTest.cs` – Interface: `Evaluate(point, context)` returns 0 (reject) or (0,1] (score multiplier).

**Query definition**
- `EnvironmentQueryAsset.cs` – ScriptableObject: one generator, list of tests, and optional agent strategy (Continuous / OnArrival, wait duration, run once).
- `EQSExecutor.cs` – Static runner: generates candidates, applies tests, returns best-scoring point and full list.

**Runtime**
- `EnvironmentQueryRunner.cs` – MonoBehaviour: holds query asset and target, runs query each frame or on demand, exposes `BestPosition`, `LastResult`, and draws gizmos when selected.

**Generators** (`EQS/Generators/`)
- `EQSGeneratorBase.cs` – Base ScriptableObject for generators.
- `GridGenerator.cs` – Grid of points (flat index: `i % divisions`, `i / divisions`), optional noise.
- `CircleGenerator.cs` – Fibonacci disc of points.
- `DonutGenerator.cs` – Donut-shaped distribution.
- `NavMeshSampleGenerator.cs` – Random NavMesh samples around the querier.

**Tests** (`EQS/Tests/`)
- `EQSTestBase.cs` – Base ScriptableObject for tests.
- `DistanceToTargetTest.cs` – Score by distance to target (closer or farther).
- `DistanceFromQuerierTest.cs` – Score by distance from querier (e.g. ideal patrol distance).
- `LineOfSightTest.cs` – Require or reject line of sight to target (e.g. attack vs cover).
- `SlopeTest.cs` – Reject or score by surface slope.
- `OverlapTest.cs` – Reject points overlapping selected layers.

**Example**
- `EQS/Examples/EQSExampleAgent.cs` – Example agent: requires `EnvironmentQueryRunner`, uses `NavMeshAgent` to move to `BestPosition`; behavior (Continuous vs OnArrival, wait, run once) is read from the query asset.

## Example Usage

Create a query asset (Create > EQS > Query), assign a generator and tests, then add a runner and optional example agent:

```csharp
// On a GameObject: add EnvironmentQueryRunner, assign your EnvironmentQueryAsset and optional Target.
// The runner runs the query and exposes the result.

var runner = GetComponent<EnvironmentQueryRunner>();
runner.RunQuery();  // or it runs every frame if RunEveryFrame is true

if (runner.HasValidResult) {
    Vector3 best = runner.BestPosition;
    // e.g. sample onto NavMesh then:
    agent.SetDestination(best);
}

// Or run a specific query with optional target override:
var result = runner.RunQuery(someOtherQueryAsset, targetOverride);
if (result.Success)
    agent.SetDestination(result.BestPosition);
```

Using the example agent (no code needed for basic movement):

1. Add `EnvironmentQueryRunner` and `EQSExampleAgent` (and a `NavMeshAgent`) to your AI.
2. Assign the EQS query asset and, for attack/cover queries, the target transform.
3. Strategy (Continuous vs OnArrival, wait duration, run once) is read from the query asset.

## YouTube

[**Watch the tutorial video here**](https://youtu.be/RW5iAJnBXhs)

You can also check out my [YouTube channel](https://www.youtube.com/@git-amend?sub_confirmation=1) for more Unity content.

## Installation and Setup

This is a Unity project (C#). Place the EQS scripts in your project (e.g. under `Assets/_Project/Scripts/EQS/`). The example agent uses `UnityUtils` (e.g. `GetOrAdd<T>()`); ensure that or equivalent is available, or replace with `GetComponent`/`AddComponent` as needed.

**Minimum setup**
- Add `EnvironmentQueryRunner` to a GameObject, assign an `EnvironmentQueryAsset` (with a generator and optionally tests), and read `BestPosition` / `LastResult` from your own movement or AI code.
- For NavMesh movement, use `NavMesh.SamplePosition` on the best position before calling `NavMeshAgent.SetDestination`.

**Optional**
- Use `EQSExampleAgent` for a ready-made “move to best point” behavior (Continuous or OnArrival from the query asset).
- Use the Editor menu **EQS > Create Example Query Assets** (if present) to generate example attack, cover, and patrol query assets.

## Inspired by

This project takes inspiration from:

- **Unreal Engine** – [Environment Query System](https://docs.unrealengine.com/5.0/en-US/environment-query-system-in-unreal-engine/) (generators, tests, contexts).
- [**UniEQS**](https://github.com/sotanmochi/UniEQS) – EQS-style system for Unity.
- [**EnvironmentQuerySystem-Unity**](https://github.com/segreaves/EnvironmentQuerySystem-Unity) – Queriers, formations, and evaluations in Unity.
