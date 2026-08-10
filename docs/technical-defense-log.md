# Registro de defensa técnica

Documento vivo para la entrevista de revisión del microservicio de renting. Este archivo se mantiene pequeño, práctico y se actualiza después de cada feature cerrada o commit relevante.

## Objetivo

El objetivo de esta implementación es mostrar:

- Cómo se organiza la solución usando Arquitectura Hexagonal, Clean Architecture y DDD.
- Cómo TDD guía cada regla de negocio desde rojo hasta verde.
- Por qué el dominio se mantiene pequeño y expresivo en lugar de sobrecargado.
- Cómo la Web API, la capa de aplicación y la infraestructura permanecen desacopladas.
- Cómo se usa el patrón Presenter en la API para formatear respuestas.
- Cómo el entorno local sigue siendo ejecutable sin dependencias externas.

## Guion De Entrevista

Cuando comparta pantalla, el recorrido recomendado es:

1. Empezar en la raíz del repositorio y explicar la estructura de la solución.
2. Bajar de fuera hacia dentro: API -> Application -> Domain -> Infrastructure.
3. Mostrar la primera regla de negocio como test rojo y después la implementación mínima en dominio.
4. Explicar cómo cada commit corresponde a un paso pequeño y verificable.
5. Cerrar remarcando que la arquitectura sirve a las reglas de negocio, no al revés.

## Notas De Arquitectura

### Dominio

- Mantener las reglas de negocio dentro del modelo de dominio.
- Usar value objects para validaciones e invariantes.
- Mantener los agregados pequeños y explícitos.
- Evitar introducir factories, services o repositories si la regla no los necesita de verdad.

### Aplicación

- Los casos de uso deben orquestar, no contener la lógica de negocio.
- Commands y queries son la frontera natural de la capa de aplicación.
- MediatR se usa como mecanismo de despacho, no como lugar donde viven las reglas.

### Infraestructura

- Usar SQLite o EF Core InMemory para mantener la solución ejecutable en local.
- La infraestructura adapta la persistencia y los servicios externos a los puertos de aplicación.
- No deben filtrarse decisiones de negocio a esta capa.

### Web API

- Los controladores deben ser delgados.
- El controlador delega el formateo de respuesta en el Presenter.
- Los ViewModels pertenecen a la frontera de API y no deben contaminar el dominio.

## Features Completadas

### 1. Validación de fecha de fabricación

Regla de negocio:
- Un vehículo no puede crearse si su fecha de fabricación es anterior a 5 años.

Decisión de dominio:
- Crear el value object `ManufacturingDate`.
- Validar la fecha en la frontera del dominio.
- Lanzar `DomainException` cuando se rompe el invariante.

Archivos relevantes:
- `src/GtMotive.Estimate.Microservice.Domain/ValueObjects/ManufacturingDate.cs`
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/VehicleManufacturingDateTests.cs`

Historial de commits:
- `11ae03e` - `test(domain): add failing test for manufacturing date older than five years`
- `0967fd3` - `feat(domain): implement manufacturing date value object validation`

Cómo explicarlo:
- "Llevé la validación de antigüedad al dominio porque es un invariante puro del negocio. Primero escribí el test y luego implementé el value object más pequeño posible para hacerlo pasar."

### 2. Un alquiler activo por cliente

Regla de negocio:
- Un usuario no puede tener más de un alquiler activo al mismo tiempo.

Decisión de dominio:
- Añadir un agregado mínimo `Customer`.
- Guardar si el cliente ya tiene un alquiler activo.
- Rechazar un segundo alquiler lanzando `DomainException`.

Archivos relevantes:
- `src/GtMotive.Estimate.Microservice.Domain/Customer.cs`
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/CustomerRentalRuleTests.cs`

Historial de commits:
- `614955b` - `test(domain): add failing test for single active rental per customer`
- `8363f01` - `feat(domain): enforce single active rental per customer`

Cómo explicarlo:
- "Mantuvimos el agregado intencionalmente pequeño. Solo sabe si el cliente ya tiene un alquiler activo, porque esa es la invariante que necesitamos proteger ahora mismo."

### 3. Limpieza del entorno

Objetivo:
- Mantener las ejecuciones TDD locales estables y reproducibles.

Cambios realizados:
- Se fijó el SDK a una versión instalada localmente.
- Se redujo el ruido de auditoría de paquetes para que el ciclo rojo/verde pueda continuar.
- Se mantuvo la solución ejecutable en esta máquina sin dependencias de base de datos externas.

Archivos relevantes:
- `global.json`
- `Directory.Build.props`
- `Directory.Build.targets`

Historial de commits:
- `5336a16` - `chore(build): pin .NET SDK to 9.0.202`

Cómo explicarlo:
- "Solo corregí el entorno para eliminar fricción del ciclo TDD. El objetivo es mantener el foco en el comportamiento de negocio, no en problemas de tooling."

## Estado Actual De Reglas

- Fecha máxima de fabricación del vehículo: implementada y en verde.
- Un alquiler activo por cliente: implementada y en verde.
- Casos de uso de crear/listar/alquilar/devolver vehículo: siguiente paso.

## Registro De Seguimiento

Usar esta sección para ir añadiendo cada commit/feature a medida que avancemos.

Formato:

- Commit: `hash` - `type(scope): mensaje`
- Regla o feature:
- Por qué se implementó así:
- Archivos tocados:
- Cómo explicarlo en la entrevista:

## Notas De Demo

Frases útiles para la entrevista:

- "Empiezo por el test porque quiero que la regla de dominio quede explícita antes de escribir la implementación."
- "El dominio es el centro de gravedad; las demás capas solo se adaptan a él."
- "Evito el sobre-ingeniería introduciendo patrones solo cuando la regla de negocio los necesita."
- "El Presenter mantiene el formateo de respuesta fuera del controlador, lo que preserva la separación de responsabilidades."
- "La elección de base de datos local es intencional para que el evaluador pueda ejecutar el proyecto sin preparación extra."
