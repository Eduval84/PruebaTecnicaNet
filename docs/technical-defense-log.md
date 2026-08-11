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

Checklist de navegación:

1. Abrir `README.md` para situar la arquitectura general.
2. Abrir `src/GtMotive.Estimate.Microservice.Domain` para mostrar dónde vive la regla.
3. Abrir `test/unit/GtMotive.Estimate.Microservice.UnitTests/Domain/VehicleManufacturingDateTests.cs`.
4. Abrir `src/GtMotive.Estimate.Microservice.Domain/ValueObjects/ManufacturingDate.cs`.
5. Mostrar el historial de commits con `git log --oneline -n 5`.
6. Cerrar enseñando el documento `docs/technical-defense-log.md` como resumen de la narrativa.

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

Checklist de navegación:

1. Abrir `test/unit/GtMotive.Estimate.Microservice.UnitTests/Domain/CustomerRentalRuleTests.cs`.
2. Abrir `src/GtMotive.Estimate.Microservice.Domain/Customer.cs`.
3. Señalar que el estado mínimo del agregado es suficiente para proteger la regla.
4. Mostrar el test verde y el commit asociado.

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

Checklist de navegación:

1. Abrir `test/unit/GtMotive.Estimate.Microservice.UnitTests/Domain/CustomerReturnRuleTests.cs`.
2. Abrir `src/GtMotive.Estimate.Microservice.Domain/Customer.cs`.
3. Mostrar el método `EndRental()` y explicar que solo libera el estado.
4. Enseñar que el test pasa sin añadir todavía más complejidad al modelo.

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

Checklist de navegación:

1. Abrir `global.json`.
2. Abrir `Directory.Build.props`.
3. Abrir `Directory.Build.targets`.
4. Explicar que estos cambios solo sostienen la ejecución local del TDD.

### Punto 5. Siguiente paso natural

Qué mostrar:
- La carpeta `ApplicationCore` y los contratos de la capa de aplicación.

Qué decir:
- "Con el dominio ya probado, el siguiente paso lógico es movernos a la capa de aplicación."
- "Ahí empezamos a introducir comandos, handlers, puertos y el patrón Presenter."
- "MediatR me ayuda a despachar el comando al handler correcto, pero no contiene la lógica de negocio; esa sigue en dominio y aplicación."
- "De esa forma mantenemos el dominio limpio y dejamos que la aplicación orqueste los casos de uso."

Mensaje clave:
- Primero dominio, luego aplicación, después infraestructura y finalmente la API.

Checklist de navegación:

1. Abrir `src/GtMotive.Estimate.Microservice.ApplicationCore`.
2. Identificar los contratos y los casos de uso.
3. Explicar que ahí se introducen comandos, handlers y puertos.
4. Dejar claro que el Presenter y la API vendrán después como adaptación de la capa de aplicación.

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
- MediatR recibe el mensaje, lo enruta al handler y mantiene el controlador desacoplado de la implementación concreta.
- La capa de aplicación traduce la intención del usuario en acciones del dominio y delega la persistencia o integración en puertos.

Recordatorio rápido sobre MediatR:

- Recibe un mensaje y lo entrega a su handler correspondiente.
- Permite desacoplar el controlador del caso de uso concreto.
- Hace que la API sea más delgada y fácil de probar.
- No sustituye al dominio ni mueve la lógica de negocio fuera de él.

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
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/Domain/VehicleManufacturingDateTests.cs`

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
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/Domain/CustomerRentalRuleTests.cs`

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
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/Domain/CustomerReturnRuleTests.cs`

Historial de commits:
- `7bdeb83` - `test(domain): add failing test for rental return behavior`
- `9b1310f` - `feat(domain): add end rental behavior to customer aggregate`

Cómo explicarlo:
- "Primero cerré la invariante de que solo puede haber un alquiler activo y después añadí la devolución como la operación mínima para liberar ese estado. No añadí más modelo del necesario porque todavía no hace falta saber qué vehículo concreto se devuelve para cumplir esta regla."

### 4. Primer caso de uso de la aplicación

Caso elegido:
- Crear vehículo.

Qué mostrar:
- `ApplicationCore/UseCases`.
- `CreateVehicleInput`.
- `CreateVehicleUseCase`.
- `ICreateVehicleOutputPort`.
- `IUseCase<TInput>` como contrato base de la plantilla.

Qué decir:
- "Ahora ya no estoy validando solo una regla de dominio aislada, sino el flujo que orquesta la aplicación."
- "Sigo usando las interfaces de la plantilla: el caso de uso implementa `IUseCase<TInput>` y el resultado sale por un puerto estándar."
- "MediatR aquí solo actúa como despachador del mensaje; la lógica sigue viviendo en el caso de uso y en el dominio."
- "El controlador no conoce la implementación concreta del flujo, solo el mensaje que envía y el presenter que devuelve la respuesta."

Mensaje clave:
- La capa de aplicación orquesta la intención del usuario y delega la regla al dominio.

Historial de commits:
- `ef8a7bd` - `feat(application): add create vehicle use case using template contracts`

Cómo explicarlo:
- "Aquí ya estoy usando la plantilla real de la capa de aplicación: el caso de uso implementa `IUseCase<TInput>`, el input implementa `IUseCaseInput` y el resultado sale por un puerto estándar."
- "MediatR no decide la lógica; solo entrega el mensaje al handler adecuado."
- "El dominio sigue validando las reglas y la aplicación orquesta el flujo."

Checklist de navegación:

1. Abrir `src/GtMotive.Estimate.Microservice.ApplicationCore/UseCases`.
2. Mostrar `IUseCase<TInput>`, `IUseCaseInput` e `IOutputPortStandard<TOutput>`.
3. Abrir `CreateVehicleInput` y `CreateVehicleUseCase`.
4. Abrir `ICreateVehicleOutputPort` y explicar su relación con el Presenter de la API.
5. Señalar `MediatR` en `ApiConfiguration` como mecanismo de despacho, no como lugar de negocio.

Notas para explicar la implementación:

- El input implementa `IUseCaseInput`.
- El caso de uso implementa `IUseCase<CreateVehicleInput>`.
- El output implementa `IUseCaseOutput`.
- El puerto de salida sigue la forma estándar de la plantilla.
- El dominio sigue siendo el que valida la fecha de fabricación antes de construir el vehículo.

### 5. Segundo caso de uso de la aplicación

Caso elegido:
- Listar vehículos disponibles.

### 6. Primer test funcional de integración (Application + Infrastructure)

Objetivo:
- Validar un flujo real de integración sin levantar el host HTTP, ejecutando el caso de uso con DI y persistencia en memoria.

Decisión técnica:
- Añadir implementaciones in-memory para `IVehicleRepository`, `ICustomerRepository` e `IUnitOfWork`.
- Conectar estas implementaciones en `InfrastructureConfiguration`.
- Crear un test funcional que ejecute `CreateVehicleUseCase` desde el composition root del proyecto funcional y valide persistencia.

Archivos relevantes:
- `src/GtMotive.Estimate.Microservice.Infrastructure/InMemory/InMemoryFleetStore.cs`
- `src/GtMotive.Estimate.Microservice.Infrastructure/InMemory/InMemoryVehicleRepository.cs`
- `src/GtMotive.Estimate.Microservice.Infrastructure/InMemory/InMemoryCustomerRepository.cs`
- `src/GtMotive.Estimate.Microservice.Infrastructure/InMemory/InMemoryUnitOfWork.cs`
- `src/GtMotive.Estimate.Microservice.Infrastructure/InfrastructureConfiguration.cs`
- `test/functional/GtMotive.Estimate.Microservice.FunctionalTests/Specs/CreateVehicleFunctionalTests.cs`

Resultado:
- Test funcional en verde validando que un vehículo creado por el caso de uso queda persistido y recuperable desde el repositorio en memoria.

Historial de commits:
- `77d60d1` - `feat(functional): add in-memory infrastructure and create vehicle integration test`

Cómo explicarlo:
- "Este test me permite demostrar integración de aplicación e infraestructura sin ruido del host web."
- "Mantengo el entorno local sin dependencias externas, pero ya pruebo wiring real de DI y puertos."
- "Es un paso intermedio entre unit tests puros y pruebas HTTP de extremo a extremo."

### 7. Test funcional de listado de vehículos disponibles

Objetivo:
- Validar de forma integrada que el caso de uso de listado solo devuelve vehículos disponibles.

Decisión técnica:
- Crear un escenario funcional con dos vehículos: uno disponible y otro alquilado.
- Alquilar el segundo mediante `RentVehicleUseCase` para que el estado de disponibilidad cambie en dominio.
- Ejecutar `ListAvailableVehiclesUseCase` y comprobar el resultado expuesto por `ListAvailableVehiclesPresenter`.

Archivos relevantes:
- `test/functional/GtMotive.Estimate.Microservice.FunctionalTests/Specs/ListAvailableVehiclesFunctionalTests.cs`
- `test/functional/GtMotive.Estimate.Microservice.FunctionalTests/Infrastructure/CompositionRootTestFixture.cs`

Resultado:
- Test funcional en verde confirmando que el vehículo alquilado no aparece en el listado de disponibles.

Historial de commits:
- `67da1b6` - `test(functional): verify list available excludes rented vehicles`

Cómo explicarlo:
- "Este test prueba el flujo completo de aplicación contra la infraestructura en memoria sin levantar HTTP."
- "La disponibilidad se valida como comportamiento observable de negocio, no como detalle interno del repositorio."

### 8. Test funcional del ciclo devolución y disponibilidad

Objetivo:
- Validar el ciclo completo Create -> Rent -> Return -> ListAvailable para comprobar que un vehículo vuelve a estar disponible tras la devolución.

Decisión técnica:
- Crear datos únicos por ejecución (customer/vehicle) para evitar acoplamiento entre tests del mismo fixture compartido.
- Verificar una precondición de negocio: tras alquilar, el vehículo no aparece en el listado de disponibles.
- Ejecutar devolución y validar postcondición: el mismo vehículo vuelve a aparecer en disponibles.

Archivos relevantes:
- `test/functional/GtMotive.Estimate.Microservice.FunctionalTests/Specs/ReturnVehicleAvailabilityFunctionalTests.cs`

Resultado:
- Test funcional en verde confirmando que la devolución restituye la disponibilidad observable del vehículo.

Cómo explicarlo:
- "Aquí demuestro comportamiento de negocio de extremo a extremo dentro de aplicación+infraestructura sin host web."
- "No valido estados internos de objetos, valido resultado funcional observable en el listado disponible."

Qué mostrar:
- `IVehicleRepository` con `ListAvailable()`.
- `ListAvailableVehiclesInput`.
- `ListAvailableVehiclesUseCase`.
- `IListAvailableVehiclesOutputPort`.
- `ListAvailableVehiclesOutput` y `ListAvailableVehiclesOutputItem`.

Qué decir:
- "Repetimos el mismo patrón de la plantilla para mantener consistencia entre casos de uso."
- "El caso de uso se limita a orquestar: pide datos al repositorio y los proyecta al output."
- "La API no necesita conocer la implementación del repositorio, solo el contrato del puerto."
- "Este diseño nos prepara para implementar después infraestructura en memoria sin tocar la lógica de aplicación."

Mensaje clave:
- Coherencia de patrón: mismo contrato, diferente intención de negocio.

Historial de commits:
- `bcac50d` - `test(application): add failing test for listing available vehicles`
- `3e5cc7d` - `feat(application): add list available vehicles use case using template contracts`

Checklist de navegación:

1. Abrir `src/GtMotive.Estimate.Microservice.ApplicationCore/UseCases/IVehicleRepository.cs`.
2. Mostrar `ListAvailableVehiclesInput` y `ListAvailableVehiclesUseCase`.
3. Mostrar `IListAvailableVehiclesOutputPort` y el output (`ListAvailableVehiclesOutput`).
4. Abrir `test/unit/GtMotive.Estimate.Microservice.UnitTests/ApplicationCore/ListAvailableVehiclesUseCaseTests.cs`.
5. Enseñar el test verde y remarcar que no dependemos todavía de infraestructura concreta.

### 6. Tercer caso de uso de la aplicación

Caso elegido:
- Alquilar vehículo.

Qué mostrar:
- `RentVehicleInput`.
- `RentVehicleUseCase`.
- `IRentVehicleOutputPort`.
- `RentVehicleOutput`.
- `RentVehicleUseCaseTests`.

Qué decir:
- "Mantengo exactamente los mismos contratos de plantilla para que cada caso de uso sea predecible y fácil de defender."
- "El caso de uso valida existencia de cliente y vehículo a través de puertos, luego delega el cambio de estado al dominio."
- "La operación de alquiler no se resuelve en el controlador ni en MediatR; se resuelve en aplicación más dominio."
- "Persisto ambos agregados y confirmo transacción con unit of work antes de emitir el output." 

Mensaje clave:
- Mismo patrón arquitectónico, distinta acción de negocio, misma disciplina TDD.

Archivos relevantes:
- `src/GtMotive.Estimate.Microservice.ApplicationCore/UseCases/ICustomerRepository.cs`
- `src/GtMotive.Estimate.Microservice.ApplicationCore/UseCases/RentVehicleInput.cs`
- `src/GtMotive.Estimate.Microservice.ApplicationCore/UseCases/RentVehicleOutput.cs`
- `src/GtMotive.Estimate.Microservice.ApplicationCore/UseCases/IRentVehicleOutputPort.cs`
- `src/GtMotive.Estimate.Microservice.ApplicationCore/UseCases/RentVehicleUseCase.cs`
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/ApplicationCore/RentVehicleUseCaseTests.cs`

Historial de commits:
- `9ab5f0f` - `feat(application): add rent vehicle use case with template ports`
- `5501412` - `test(application): add failing tests for rent vehicle edge cases`
- `ccca10d` - `feat(application): handle rent vehicle edge cases`

Cómo explicarlo:
- "Primero escribí el test del flujo feliz de alquiler y después implementé solo los contratos y la orquestación mínima para pasarlo, manteniendo el dominio como único lugar de reglas."

Cobertura de edge cases (TDD):

- Customer no existe: se corta el flujo con `NotFoundHandle` y no se consulta vehículo.
- Vehicle no existe: se responde `NotFoundHandle` específico de alquiler y no hay persistencia.
- Alquiler inválido (vehículo no disponible): se responde `NotFoundHandle` y no se altera estado ni se persiste.

Qué cambió en implementación:

- Se movió la consulta de vehículo después de validar customer.
- Se ajustó el mensaje de not found para el caso de alquiler.
- Se añadió guard clause para evitar `Rent()` cuando `vehicle.IsAvailable` es `false`.

### 7. Cuarto caso de uso de la aplicación

Caso elegido:
- Devolver vehículo.

Qué mostrar:
- `ReturnVehicleInput`.
- `ReturnVehicleUseCase`.
- `IReturnVehicleOutputPort`.
- `ReturnVehicleOutput`.
- `ReturnVehicleUseCaseTests`.

Qué decir:
- "Mantengo el mismo esqueleto del resto de casos de uso para no romper consistencia arquitectónica."
- "La aplicación orquesta la devolución, pero las transiciones válidas del estado siguen en dominio."
- "El flujo busca cliente y vehículo, ejecuta `EndRental()` y `Return()`, persiste cambios y responde por output port."
- "Si no existe cliente o vehículo, corto el flujo por `NotFoundHandle` sin tocar estado."

Mensaje clave:
- Misma plantilla de aplicación, nueva intención de negocio, misma disciplina TDD.

Archivos relevantes:
- `src/GtMotive.Estimate.Microservice.ApplicationCore/UseCases/ReturnVehicleInput.cs`
- `src/GtMotive.Estimate.Microservice.ApplicationCore/UseCases/ReturnVehicleOutput.cs`
- `src/GtMotive.Estimate.Microservice.ApplicationCore/UseCases/IReturnVehicleOutputPort.cs`
- `src/GtMotive.Estimate.Microservice.ApplicationCore/UseCases/ReturnVehicleUseCase.cs`
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/ApplicationCore/ReturnVehicleUseCaseTests.cs`

Historial de commits:
- `c889efd` - `feat(application): add return vehicle use case using template ports`
- `56fdf00` - `test(application): add failing tests for return vehicle edge cases`
- `374c2b4` - `feat(application): handle return vehicle edge cases`

Cómo explicarlo:
- "Primero dejé el test en rojo referenciando contratos que no existían todavía. Después implementé solo lo mínimo: input, output, puerto y caso de uso. Finalmente validé el verde con test filtrado."

Cobertura de edge cases (TDD):

- Customer no existe: se corta el flujo con `NotFoundHandle` y no se consulta vehículo.
- Vehicle no existe: se responde `NotFoundHandle` específico de devolución y no hay persistencia.
- Devolución inválida (vehículo no alquilado): se responde `NotFoundHandle` y no se altera estado ni se persiste.

Qué cambió en implementación:

- Se movió la consulta de vehículo después de validar customer.
- Se ajustó el mensaje de not found para el caso de devolución.
- Se añadió guard clause para evitar `Return()` cuando `vehicle.IsAvailable`.

### 8. Limpieza del entorno

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

### 9. Mapeo API consistente para alquiler y devolución

Objetivo:
- Llevar los edge cases de `RentVehicle` y `ReturnVehicle` a la frontera HTTP con respuestas consistentes.

Qué se implementó:
- Presenters dedicados para rent y return con mapeo uniforme:
	- `StandardHandle` -> `200 OK` con output del caso de uso.
	- `NotFoundHandle` -> `404 Not Found` con `ProblemDetails`.
- Controladores separados por responsabilidad (`RentVehicleController` y `ReturnVehicleController`) para evitar mezcla de responsabilidades.
- Registro en DI de casos de uso y presenters para resolver puertos de salida en tiempo de ejecución.
- Mantenimiento del filtro global `BusinessExceptionFilter` para mapear `DomainException` a `400 Bad Request`.

Archivos relevantes:
- `src/GtMotive.Estimate.Microservice.Api/UseCases/RentVehiclePresenter.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/ReturnVehiclePresenter.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/RentVehicleController.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/ReturnVehicleController.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/RentVehicleRequest.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/ReturnVehicleRequest.cs`
- `src/GtMotive.Estimate.Microservice.Api/DependencyInjection/UserInterfaceExtensions.cs`
- `src/GtMotive.Estimate.Microservice.ApplicationCore/ApplicationConfiguration.cs`
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/Api/UseCases/RentalPresentersTests.cs`

Historial de commits:
- `4a4014d` - `test(api): add failing tests for rental presenter mappings`
- `4e3f239` - `feat(api): add rental presenters and controllers with consistent mappings`

Cómo explicarlo:
- "En aplicación ya teníamos decisiones de negocio para no encontrados y casos inválidos; aquí trasladé ese resultado al contrato HTTP de forma estable y predecible: 200 para éxito, 404 para not found por puerto, y 400 para reglas de dominio por filtro global."

### 10. Refactor a primary constructors

Objetivo:
- Modernizar los constructores explícitos de los casos de uso y adaptadores de infraestructura migrando a la sintaxis de primary constructor de C# 12.

Alcance:
- 8 clases de `ApplicationCore/UseCases`: `CreateVehicleUseCase`, `ListAvailableVehiclesUseCase`, `RentVehicleUseCase`, `ReturnVehicleUseCase` y sus inputs/outputs.
- 2 clases de `Infrastructure`: `LoggerAdapter` y `MongoService`.

Qué se hizo:
- Se movieron los parámetros del constructor a la firma de la clase.
- Se conservaron exactamente las mismas asignaciones de null-check y la misma lógica funcional.
- Se separaron refactor y corrección StyleCop en dos commits distintos para mayor claridad en la revisión.

Problemas resueltos:
- SA1505: blank line after opening brace en primary constructors.
- SA1507: multiple blank lines en cuerpos de clase.

Archivos relevantes:
- `src/GtMotive.Estimate.Microservice.ApplicationCore/UseCases/*UseCase.cs` (4 archivos)
- `src/GtMotive.Estimate.Microservice.Infrastructure/Logging/LoggerAdapter.cs`
- `src/GtMotive.Estimate.Microservice.Infrastructure/MongoDb/MongoService.cs`

Historial de commits:
- `d9ac6a9` - `refactor(core): migrate use cases and adapters to primary constructors`
- `adf2f30` - `style(core): fix StyleCop spacing after primary constructor refactor`

Cómo explicarlo:
- "Separé el refactor de la corrección de estilo para que cada commit tenga un único propósito y sea fácil de revertir si fuera necesario. No cambié ninguna lógica funcional, solo la sintaxis del constructor."

### 11. Edge case en CreateVehicle y capa API para todos los casos de uso

Objetivo:
- Completar la cobertura TDD de `CreateVehicle` con el edge case de fecha inválida y extender la capa API a los cuatro casos de uso.

**Edge case de CreateVehicle**

Regla de negocio:
- El dominio ya rechaza fechas de fabricación anteriores a 5 años lanzando `DomainException`.
- El test verifica que el use case deja propagarse esa excepción sin capturarla, y que los repositorios/presenters nunca se invocan gracias a `MockBehavior.Strict`.

Historial de commits:
- `aeb07c1` - `test(use-case): add edge case for CreateVehicle with invalid manufacturing date`

**Presenters y tests para CreateVehicle y ListAvailableVehicles**

Qué se implementó:
- `CreateVehiclePresenter`: `StandardHandle` → `200 OK` con `CreateVehicleOutput`.
- `ListAvailableVehiclesPresenter`: `StandardHandle` → `200 OK` con `ListAvailableVehiclesOutput`.
- `CreateVehicleController`: `POST /api/vehicles/create` con `[JsonRequired]` en `ManufacturingDate` para cumplir la regla S6964 de SonarQube.
- `ListAvailableVehiclesController`: `GET /api/vehicles/available`.
- Registro de ambos presenters en `UserInterfaceExtensions.AddPresenters()`.
- 2 tests nuevos en `RentalPresentersTests`: mapeo de `StandardHandle` para `CreateVehiclePresenter` y `ListAvailableVehiclesPresenter`.

Archivos relevantes:
- `src/GtMotive.Estimate.Microservice.Api/UseCases/CreateVehiclePresenter.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/CreateVehicleController.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/CreateVehicleRequest.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/ListAvailableVehiclesPresenter.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/ListAvailableVehiclesController.cs`
- `src/GtMotive.Estimate.Microservice.Api/DependencyInjection/UserInterfaceExtensions.cs`
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/Api/UseCases/RentalPresentersTests.cs`

Historial de commits:
- `7945f68` - `test(api): add presenter mapping tests for CreateVehicle and ListAvailableVehicles`
- `8d18242` - `feat(api): add presenters and controllers for CreateVehicle and ListAvailableVehicles`

Resultado final:
- 32/32 tests en verde.
- 0 errores de compilación (solo 2 warnings esperados de proyectos de test vacíos).
- Todos los casos de uso con su presenter, controller y DI registrados.

Cómo explicarlo:
- "Apliqué exactamente el mismo patrón Presenter que ya habíamos probado en rent/return. Para `CreateVehicle` marqué `ManufacturingDate` con `[JsonRequired]` para satisfacer la regla de SonarQube que requiere que value types usados como input de controladores sean explícitamente requeridos."

### 12. Prueba de infraestructura HTTP respetando template (auth/authz + MediatR)

Objetivo:
- Validar la capa de infraestructura/host ejecutando un controller real con payload JSON y comprobando binding + ejecución de caso de uso mockeado.

Decisión técnica:
- Mantener `UseAuthentication()` y `UseAuthorization()` en el pipeline de `Startup` de tests.
- Mantener registro de MediatR en la configuración de servicios para respetar la plantilla arquitectónica.
- Sustituir dependencia antigua de test host por una versión compatible con .NET 9 para estabilizar la ejecución HTTP en pruebas.

Archivos relevantes:
- `test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/Infrastructure/Startup.cs`
- `test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/GtMotive.Estimate.Microservice.InfrastructureTests.csproj`
- `test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/Specs/CreateVehicleRequestValidationInfrastructureTests.cs`
- `Directory.Build.targets`

Resultado:
- `InfrastructureTests` en verde con ejecución real de controller y pipeline de autenticación/autorización activo.

Cómo explicarlo:
- "No quité seguridad del pipeline para hacer pasar tests; ajusté el test host a runtime actual y mantuve la plantilla intacta."
- "El test verifica comportamiento HTTP y model binding en la frontera, sin mover reglas de negocio fuera de aplicación/dominio."

### 13. Validación 400 en CreateVehicle por payload inválido

Objetivo:
- Verificar en infraestructura que un request inválido (sin `manufacturingDate`) devuelve `400 Bad Request` y no ejecuta el caso de uso.

Decisión técnica:
- Reutilizar el mismo arranque de TestServer con `UseAuthentication`, `UseAuthorization` y registro de MediatR.
- Inyectar un spy de `IUseCase<CreateVehicleInput>` para comprobar no invocación cuando falla model validation.

Archivo relevante:
- `test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/Specs/CreateVehicleRequestValidationInfrastructureTests.cs`

Resultado:
- Test de infraestructura en verde con dos escenarios:
	- Payload válido: binding correcto e invocación del use case.
	- Payload inválido: `400 BadRequest` y `ExecutionCount = 0`.

Cómo explicarlo:
- "La validación de contrato HTTP se prueba en la frontera sin romper la arquitectura interna."
- "Cuando el contrato falla, el caso de uso no se ejecuta, preservando la responsabilidad de cada capa."

### 14. Endpoint de listado disponible con payload en infraestructura

Objetivo:
- Verificar que `GET /api/vehicles/available` responde `200 OK` y devuelve payload cuando el caso de uso publica vehículos disponibles.

Decisión técnica:
- Mantener el pipeline real de infraestructura de tests (auth/authz + controllers de la API).
- Sustituir solo `IUseCase<ListAvailableVehiclesInput>` por un spy que publica salida vía `ListAvailableVehiclesPresenter`.
- Validar tanto ejecución del caso de uso como contenido JSON de respuesta.

Archivo relevante:
- `test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/Specs/ListAvailableVehiclesInfrastructureTests.cs`

Resultado:
- Test en verde validando estado HTTP `200` y presencia de `vehicleId` en el payload serializado.

Cómo explicarlo:
- "En esta prueba de infraestructura validamos el contrato HTTP del endpoint de listado sin acoplar el test a almacenamiento real."
- "La respuesta se construye por el Presenter de la plantilla, manteniendo separación de responsabilidades."

### 15. Contrato HTTP 404 para Rent y Return en infraestructura

Objetivo:
- Verificar que los endpoints de alquiler y devolución devuelven `404 NotFound` cuando el caso de uso informa recurso inexistente.

Decisión técnica:
- Mantener el pipeline de tests con autenticación/autorización activo.
- Reemplazar `IUseCase<RentVehicleInput>` e `IUseCase<ReturnVehicleInput>` por spies que publican `NotFoundHandle` a través de sus presenters.
- Validar código HTTP, ejecución del caso de uso y presencia del identificador esperado en el body de respuesta.

Archivo relevante:
- `test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/Specs/RentalNotFoundInfrastructureTests.cs`

Resultado:
- Dos tests nuevos en verde:
	- `POST /api/rentals/rent` devuelve `404` cuando no existe customer.
	- `POST /api/rentals/return` devuelve `404` cuando no existe vehicle.

Cómo explicarlo:
- "La frontera HTTP respeta el contrato de error de la aplicación: not found se traduce a 404 de forma consistente."
- "El Presenter mantiene centralizado el mapeo de respuesta y el controller permanece delgado."

### 16. Contrato HTTP 200 para Rent y Return en infraestructura

Objetivo:
- Verificar que los endpoints de alquiler y devolución responden `200 OK` con payload correcto cuando el caso de uso publica éxito.

Decisión técnica:
- Mantener pipeline real del TestServer (auth/authz + controllers + presenters).
- Inyectar spies de `IUseCase<RentVehicleInput>` e `IUseCase<ReturnVehicleInput>` que llaman `StandardHandle` en sus presenters.
- Validar estado HTTP, ejecución del caso de uso y presencia de `customerId`/`vehicleId` en body.

Archivo relevante:
- `test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/Specs/RentalSuccessInfrastructureTests.cs`

Resultado:
- Dos tests nuevos en verde:
	- `POST /api/rentals/rent` -> `200 OK` con payload de éxito.
	- `POST /api/rentals/return` -> `200 OK` con payload de éxito.

Cómo explicarlo:
- "Con estas pruebas cerramos el contrato HTTP tanto para error (404) como para éxito (200) en rent y return."
- "El controller delega al caso de uso y el Presenter define la forma de respuesta, siguiendo la plantilla hexagonal."

### 17. Contrato HTTP 400 para Rent y Return por payload inválido

Objetivo:
- Completar la matriz de comportamiento HTTP en alquiler y devolución validando `400 BadRequest` cuando faltan identificadores obligatorios en el body.

Decisión técnica:
- Escribir primero tests de infraestructura que demuestran el fallo: sin validación explícita, el controller terminaba devolviendo `500` por el `ActionResult` por defecto del presenter.
- Corregir en la frontera HTTP añadiendo `[Required]` sobre `CustomerId` y `VehicleId` en los request models.
- Mantener intacto dominio, casos de uso y presenters porque la responsabilidad de este fallo era de contrato de entrada.

Archivos relevantes:
- `test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/Specs/RentalBadRequestInfrastructureTests.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/RentVehicleRequest.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/ReturnVehicleRequest.cs`

Resultado:
- Dos tests nuevos en verde:
	- `POST /api/rentals/rent` devuelve `400` y no ejecuta el use case cuando falta `customerId`.
	- `POST /api/rentals/return` devuelve `400` y no ejecuta el use case cuando falta `vehicleId`.

Cómo explicarlo:
- "La validación de contrato pertenece a la frontera HTTP; por eso la resolví en los request models y no en aplicación ni dominio."
- "El test prueba además que, cuando ModelState falla, el caso de uso no se ejecuta."

### 18. Cierre de validación global

Objetivo:
- Confirmar que toda la solución queda estable y consistente después de completar las capas de dominio, aplicación, infraestructura y API.

Resultado:
- Ejecución completa de `dotnet test src/microservice.sln --nologo` en verde.
- Total validado: `32/32` tests correctos.
- Cobertura funcional repartida entre unit tests, functional tests e infrastructure tests.

Cómo explicarlo:
- "Cierro con una validación completa de la solución para demostrar que no he optimizado solo casos aislados, sino el sistema completo."
- "El resultado final enseña una evolución TDD trazable: dominio primero, aplicación después, integración/infrastructura al final."

### 19. Ejecución local sin dependencias externas

Objetivo:
- Asegurar que cualquier persona pueda levantar la API en local sin instalar MongoDB, Key Vault ni servicios corporativos adicionales.

Decisión técnica:
- Añadir `launchSettings.json` al Host para que `dotnet run` arranque en `Development` por defecto.
- Mantener en desarrollo la infraestructura en memoria ya registrada por la solución.
- Eliminar del `Dockerfile` la dependencia implícita de un feed corporativo y añadir `docker-compose.yml` para un arranque de contenedor en un solo comando.
- Documentar ambos caminos de ejecución en `README.md`.

Archivos relevantes:
- `src/GtMotive.Estimate.Microservice.Host/Properties/launchSettings.json`
- `src/GtMotive.Estimate.Microservice.Host/Dockerfile`
- `docker-compose.yml`
- `README.md`

Resultado:
- `dotnet run --project src/GtMotive.Estimate.Microservice.Host` levanta la API en `http://localhost:5080` en entorno `Development`.
- La solución sigue sin requerir base de datos externa para ejecución local.

Cómo explicarlo:
- "No me limité a que el código compile; dejé preparado el arranque real para un evaluador sin dependencias externas."
- "La arquitectura sigue siendo la misma, pero la experiencia de ejecución local queda cerrada y reproducible."

### 20. Endpoints autenticados por defecto

Objetivo:
- Hacer que la API quede protegida por defecto, evitando acceso anónimo accidental a los endpoints del microservicio.

Decisión técnica:
- Configurar una `FallbackPolicy` en autorización para requerir usuario autenticado en toda la API.
- Mantener los tests de infraestructura con un esquema de autenticación de prueba, pero exigir un header explícito para simular credenciales válidas.
- Añadir tests de infraestructura que verifiquen que peticiones anónimas reciben `401 Unauthorized`.

Archivos relevantes:
- `src/GtMotive.Estimate.Microservice.Api/Authorization/AuthorizationOptionsExtensions.cs`
- `test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/Infrastructure/Startup.cs`
- `test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/Specs/AuthenticatedEndpointsInfrastructureTests.cs`

Resultado:
- Los endpoints quedan autenticados por defecto.
- `InfrastructureTests` verifica tanto acceso autorizado como rechazo anónimo.

Cómo explicarlo:
- "En vez de confiar en que cada controller recuerde poner `[Authorize]`, protegí la API por defecto desde la configuración central."
- "Los tests de infraestructura distinguen entre llamadas anónimas y llamadas autenticadas de forma explícita."

### 21. Swagger expuesto para la demo

Objetivo:
- Mantener Swagger siempre accesible para simplificar la demostración funcional del microservicio durante la presentación.

Decisión técnica:
- Revertir la restricción de Swagger por entorno y dejar el UI disponible de forma permanente.
- Mantener la autenticación por defecto en los endpoints de negocio, separando la seguridad operativa de la experiencia de demo.

Resultado:
- El Host vuelve a exponer Swagger y se validó acceso `200 OK` en `http://localhost:5080/swagger/index.html`.

Cómo explicarlo:
- "Para la demo prioricé accesibilidad de la documentación interactiva sin tocar la protección por defecto de los endpoints de negocio."

### 22. MediatR explícito en CreateVehicle

Objetivo:
- Alinear el endpoint `CreateVehicle` con el patrón Controller -> MediatR Request -> Handler -> Use Case -> Presenter descrito por la template.

Decisión técnica:
- Convertir `CreateVehicleRequest` en un mensaje `IRequest<IWebApiPresenter>`.
- Crear `CreateVehicleRequestHandler` para traducir el request HTTP al `CreateVehicleInput` del caso de uso.
- Simplificar el controller para que solo envíe el request a `IMediator` y devuelva el presenter resultante.

Archivos relevantes:
- `src/GtMotive.Estimate.Microservice.Api/UseCases/CreateVehicleRequest.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/CreateVehicleRequestHandler.cs`
- `src/GtMotive.Estimate.Microservice.Api/UseCases/CreateVehicleController.cs`
- `test/unit/GtMotive.Estimate.Microservice.UnitTests/Api/UseCases/CreateVehicleRequestHandlerTests.cs`

Resultado:
- `CreateVehicle` ya sigue explícitamente el patrón MediatR de la template.
- Validado en unit tests e infrastructure tests sin romper el contrato HTTP existente.

Cómo explicarlo:
- "El controller deja de conocer el caso de uso y delega el envío del mensaje en MediatR."
- "El handler adapta el request web al input de aplicación y devuelve el presenter, manteniendo separadas las responsabilidades de cada capa."

## Estado Actual De Reglas

- Fecha máxima de fabricación del vehículo: implementada y en verde.
- Un alquiler activo por cliente: implementada y en verde.
- Devolución de alquiler en dominio (`EndRental` + `Vehicle.Return`): implementada y en verde.
- Caso de uso `CreateVehicle`: implementado y en verde (edge case fecha inválida cubierto).
- Caso de uso `ListAvailableVehicles`: implementado y en verde.
- Caso de uso `RentVehicle`: implementado y en verde (3 edge cases cubiertos).
- Caso de uso `ReturnVehicle`: implementado y en verde (3 edge cases cubiertos).
- Presenter y controller `CreateVehicle` (API): implementados y en verde.
- Presenter y controller `ListAvailableVehicles` (API): implementados y en verde.
- Presenter y controller `RentVehicle` (API): implementados y en verde.
- Presenter y controller `ReturnVehicle` (API): implementados y en verde.
- Suite completa: **32/32 tests en verde**.

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
