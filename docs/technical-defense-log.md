# Technical Defense Log

Living document for the interview walkthrough of the renting microservice. This file is intentionally small, practical, and updated after each completed feature or relevant commit.

## Purpose

The goal of this implementation is to show:

- How the solution is organized using Hexagonal Architecture, Clean Architecture, and DDD.
- How TDD drives each business rule from red to green.
- Why the domain stays small and expressive instead of over-engineered.
- How the Web API, application layer, and infrastructure layer remain decoupled.
- How the Presenter pattern is used in the API layer to format responses.
- How the local environment stays runnable without external dependencies.

## Interview Storyline

When sharing the screen, the recommended path is:

1. Start at the repository root and explain the solution structure.
2. Move from the outside in: API -> Application -> Domain -> Infrastructure.
3. Show the first business rule as a red test, then the minimal domain implementation.
4. Explain how each commit corresponds to a small, verifiable step.
5. Close by pointing out that the architecture serves the business rules, not the other way around.

## Architecture Notes

### Domain

- Keep the business rules inside the domain model.
- Use value objects for validation and invariants.
- Keep aggregates small and explicit.
- Avoid introducing factories, services, or repositories unless a rule genuinely needs them.

### Application

- Use cases should orchestrate, not contain business logic.
- Commands and queries are the natural boundary for the application layer.
- MediatR is used as the dispatch mechanism, not as the place where rules live.

### Infrastructure

- Use SQLite or EF Core InMemory to keep the solution runnable locally.
- Infrastructure adapts persistence and external services to the application ports.
- No business decisions should leak into this layer.

### Web API

- Controllers should stay thin.
- The controller delegates response formatting to the Presenter.
- ViewModels belong to the API boundary and should not leak domain concerns.

## Completed Features

### 1. Manufacturing date validation

Business rule:
- A vehicle cannot be created if its manufacturing date is older than 5 years.

Domain decision:
- Create a `ManufacturingDate` value object.
- Validate the date in the domain boundary.
- Throw `DomainException` when the invariant is broken.

Relevant files:
- `src/GtMotive.Estimate.Microservice.Domain/ValueObjects/ManufacturingDate.cs`
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/VehicleManufacturingDateTests.cs`

Commit history:
- `11ae03e` - `test(domain): add failing test for manufacturing date older than five years`
- `0967fd3` - `feat(domain): implement manufacturing date value object validation`

How to explain it:
- "I pushed the age validation into the domain because this is a pure business invariant. The test came first, then I implemented the smallest possible value object to make it pass."

### 2. Single active rental per customer

Business rule:
- A user cannot have more than one active rental at the same time.

Domain decision:
- Add a minimal `Customer` aggregate.
- Track whether the customer already has an active rental.
- Reject a second rental by throwing `DomainException`.

Relevant files:
- `src/GtMotive.Estimate.Microservice.Domain/Customer.cs`
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/CustomerRentalRuleTests.cs`

Commit history:
- `614955b` - `test(domain): add failing test for single active rental per customer`
- `8363f01` - `feat(domain): enforce single active rental per customer`

How to explain it:
- "I kept the aggregate intentionally small. The only thing it knows is whether the customer already has an active rental, because that is the business invariant we need to protect right now."

### 3. Environment cleanup

Purpose:
- Keep local TDD runs stable and reproducible.

Changes made:
- Updated the SDK pin to a version installed locally.
- Reduced noise from package auditing so the red/green cycle can continue.
- Kept the solution runnable in the current machine without external database dependencies.

Relevant files:
- `global.json`
- `Directory.Build.props`
- `Directory.Build.targets`

Commit history:
- `5336a16` - `chore(build): pin .NET SDK to 9.0.202`

How to explain it:
- "I fixed the environment only to remove friction from the TDD loop. The goal is to keep the focus on business behavior, not build tooling problems."

## Current Rule Status

- Vehicle manufacturing date max age: implemented and green.
- One active rental per customer: implemented and green.
- Vehicle creation/listing/rental/return use cases: next.

## Follow-up Log

Use this section to append each new commit/feature as we continue.

Format:

- Commit: `hash` - `type(scope): message`
- Rule or feature:
- Why it was implemented that way:
- Files touched:
- How to explain it in the interview:

## Demo Notes

Useful phrases for the interview:

- "I start with the test because I want the domain rule to be explicit before I write the implementation."
- "The domain is the center of gravity; the other layers just adapt to it."
- "I avoid over-engineering by only introducing patterns when the business rule needs them."
- "The Presenter keeps response formatting out of the controller, which preserves separation of concerns."
- "The local database choice is intentional so the evaluator can run the project without extra setup."
