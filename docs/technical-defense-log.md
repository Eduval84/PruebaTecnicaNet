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

## Guion Por Puntos

### Punto 1. Validación de fecha de fabricación

Qué mostrar:
- El test `VehicleManufacturingDateTests`.
- El value object `ManufacturingDate`.

Qué decir:
- "Empiezo por esta regla porque es la más pequeña y me permite enseñar el enfoque TDD sin ruido."
- "La fecha de fabricación es un invariante puro del dominio, así que la validación vive aquí y no en una capa superior."
- "El test nace primero y después aplico la implementación mínima para hacerlo pasar."

Mensaje clave:
- El dominio protege la regla desde el centro del sistema.

### Punto 2. Un alquiler activo por cliente

Qué mostrar:
- El test `CustomerRentalRuleTests`.
- El agregado `Customer`.

Qué decir:
- "Esta regla demuestra que el agregado solo guarda el estado que necesita para proteger una invariante de negocio."
- "No introduzco más modelo del necesario; el cliente solo sabe si ya tiene un alquiler activo."
- "Con esto evito que el mismo usuario pueda alquilar más de un vehículo al mismo tiempo."

Mensaje clave:
- El agregado se mantiene pequeño y enfocado.

### Punto 3. Devolución del vehículo

Qué mostrar:
- El test `CustomerReturnRuleTests`.
- El método `EndRental()` en `Customer`.

Qué decir:
- "Una vez cerrada la restricción de un solo alquiler activo, necesito liberar ese estado cuando el vehículo se devuelve."
- "La implementación es mínima: solo cambio el estado de alquiler activo para permitir un nuevo alquiler."
- "Todavía no modelo el vehículo concreto devuelto porque la regla actual no lo necesita."

Mensaje clave:
- Solo incorporo el comportamiento estrictamente necesario para cumplir la regla.

### Punto 4. Preparación del entorno

Qué mostrar:
- `global.json`.
- `Directory.Build.props`.
- `Directory.Build.targets`.

Qué decir:
- "Ajusté el entorno para que la prueba técnica sea ejecutable en esta máquina sin fricción."
- "El objetivo no es tocar la lógica de negocio, sino asegurar que el ciclo TDD sea estable."
- "Prefiero resolver el tooling una sola vez y centrar la conversación en arquitectura y reglas de negocio."

Mensaje clave:
- El entorno acompaña al diseño, no lo condiciona.

### Punto 5. Siguiente paso natural

Qué mostrar:
- La carpeta `ApplicationCore` y los contratos de la capa de aplicación.

Qué decir:
- "Con el dominio ya probado, el siguiente paso lógico es movernos a la capa de aplicación."
- "Ahí empezamos a introducir comandos, handlers, puertos y el patrón Presenter."
- "De esa forma mantenemos el dominio limpio y dejamos que la aplicación orqueste los casos de uso."

Mensaje clave:
- Primero dominio, luego aplicación, después infraestructura y finalmente la API.

## Orden Lógico De Abordaje

El punto 1 debe ser la validación de la fecha de fabricación porque es la regla más pequeña, más aislada y más fácil de defender desde dominio:

- Es una invariante pura del negocio.
- No depende todavía de base de datos, controladores ni casos de uso.
- Permite mostrar el ciclo TDD completo en su forma más simple: test rojo, implementación mínima y test verde.
- Sirve para explicar la filosofía de la solución antes de entrar en capas más complejas.

Cómo contarlo en la entrevista:

1. "Empiezo por esta regla porque me permite enseñar la esencia de la arquitectura sin ruido."
2. "Primero valido la invariante en dominio, luego la hago pasar con la mínima implementación."
3. "A partir de ahí ya puedo ir creciendo hacia reglas más complejas sin romper el enfoque."

Guion práctico en pantalla:

1. Abrir la solución y señalar que el dominio está aislado de la API y de la infraestructura.
2. Abrir el test `VehicleManufacturingDateTests` y explicar que la regla nace desde el comportamiento esperado.
3. Señalar la aserción `DomainException` como la forma en la que el dominio protege el invariante.
4. Abrir `ManufacturingDate` y explicar que es un value object pequeño, centrado solo en validar una fecha.
5. Mostrar que la regla no depende de base de datos, controlador ni caso de uso, lo que reduce complejidad.
6. Cerrar enseñando el commit de test rojo y el commit de implementación verde como evidencia del ciclo TDD.

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

Punto de defensa:
- Este es el mejor primer ejemplo porque es simple, verificable y demuestra que la arquitectura protege la regla de negocio desde el centro del sistema.

Guion corto para decirlo en voz alta:

1. "Empiezo por esta regla porque no depende de ninguna otra capa."
2. "El test expresa el negocio en lenguaje claro: si la fecha tiene más de cinco años, falla."
3. "La implementación en dominio es mínima y no arrastra infraestructuras innecesarias."
4. "Eso me permite enseñar TDD y arquitectura al mismo tiempo sin perder al evaluador en detalles secundarios."

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

### 3. Devolución del vehículo

Regla de negocio:
- Cuando el cliente devuelve el vehículo, debe poder iniciar un nuevo alquiler.

Decisión de dominio:
- Añadir `EndRental()` al agregado `Customer`.
- Mantener el comportamiento mínimo: cerrar el estado de alquiler activo.
- No modelar todavía el vehículo específico devuelto, porque la necesidad actual solo exige liberar la restricción.

Archivos relevantes:
- `src/GtMotive.Estimate.Microservice.Domain/Customer.cs`
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/CustomerReturnRuleTests.cs`

Historial de commits:
- `7bdeb83` - `test(domain): add failing test for rental return behavior`
- `9b1310f` - `feat(domain): add end rental behavior to customer aggregate`

Cómo explicarlo:
- "Primero cerré la invariante de que solo puede haber un alquiler activo y después añadí la devolución como la operación mínima para liberar ese estado. No añadí más modelo del necesario porque todavía no hace falta saber qué vehículo concreto se devuelve para cumplir esta regla."

### 4. Limpieza del entorno

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
