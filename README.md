# Rewards System API

API REST para un sistema de recompensas por compras, construida en **C# / ASP.NET Core 8**.

## Regla de negocio

- Por cada **$1.000** pesos en una compra, el cliente gana **1 punto**.
- Al redimir, cada punto equivale a **$100** pesos.
- Los puntos se acumulan entre múltiples compras.
- No se puede redimir más puntos de los que el cliente tiene disponibles.

## Arquitectura (separación de responsabilidades)

```
Controllers/   -> Capa HTTP: recibe la petición y delega en Services
Services/      -> Reglas de negocio (cálculo de puntos, validaciones, redención)
Repositories/  -> Acceso a datos (implementación en memoria, fácil de reemplazar por una BD)
Models/        -> Entidades del dominio (Customer, Purchase, Redemption, PointsAccount)
Dtos/          -> Objetos de entrada/salida de la API
Exceptions/    -> Excepciones de negocio propias
Middleware/    -> Manejo global de errores (evita caídas inesperadas)
```

## Endpoints

| Método | Ruta                        | Descripción                                   |
|--------|-----------------------------|------------------------------------------------|
| POST   | `/api/purchases`            | Registra una compra y calcula los puntos       |
| GET    | `/api/points/{customerId}`  | Consulta el saldo total de puntos              |
| POST   | `/api/redemptions`          | Redime puntos (falla si el saldo es insuficiente) |

### Ejemplo: registrar compra

```
POST /api/purchases
{
  "customerId": "C001",
  "amount": 25000
}
```

Respuesta (201):

```
{
  "purchaseId": "...",
  "customerId": "C001",
  "amount": 25000,
  "pointsEarned": 25,
  "newPointsBalance": 25,
  "message": "Purchase registered successfully. 25 point(s) earned."
}
```

### Ejemplo: redimir puntos

```
POST /api/redemptions
{
  "customerId": "C001",
  "points": 10
}
```

Si el cliente no tiene suficientes puntos, la API responde `400 Bad Request`:

```
{
  "error": "InsufficientPointsException",
  "message": "Customer 'C001' does not have enough points. Requested: 999999, available: 25."
}
```

## Cómo ejecutar el proyecto

Requiere el [.NET 8 SDK](https://dotnet.microsoft.com/download).

```bash
cd RewardsSystem
dotnet restore
dotnet run
```

## Pruebas unitarias

El proyecto `RewardsSystem.Tests` contiene pruebas con xUnit para las reglas de negocio
(cálculo de puntos, acumulación, validaciones y error por saldo insuficiente).

```bash
cd RewardsSystem.Tests
dotnet test
```

(Opcional) Para agrupar ambos proyectos en una solución:

```bash
dotnet new sln -n RewardsSystem
dotnet sln add RewardsSystem/RewardsSystem.csproj RewardsSystem.Tests/RewardsSystem.Tests.csproj
```
El archivo `RewardsSystem.slnx` ya está incluido en el repositorio, así que puedes abrir la solución directamente con `dotnet build RewardsSystem.slnx` o desde Visual Studio.

La API queda disponible en `http://localhost:5080`, con Swagger en `http://localhost:5080/swagger`
para explorar y probar los endpoints desde el navegador.

## Pruebas con Postman

Importa el archivo `RewardsSystem.postman_collection.json` en Postman. Incluye casos válidos
e inválidos (monto negativo, monto no numérico, redención con saldo insuficiente).

## Subir el proyecto a un repositorio remoto

```bash
git init
git add .
git commit -m "feat: initial rewards system API structure"
git branch -M main
git remote add origin <URL_DE_TU_REPOSITORIO>
git push -u origin main
```

Sugerencia de commits descriptivos usando Conventional Commits:

- `feat: add purchase registration endpoint`
- `feat: add points redemption with balance validation`
- `feat: add global exception handling middleware`
- `test: add unit tests for RewardsService`
- `docs: add README with setup instructions`
