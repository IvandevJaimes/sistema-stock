# Sistema de Stock
Aplicación de consola para gestión de inventario de electrodomésticos en múltiples sucursales con persistencia en MySQL.

## Tecnologías
- C# (.NET 10)
- MySQL (MySqlConnector)
- Spectre.Console (interfaz de terminal)
- Arquitectura por capas (models, db, services, controllers, ui)

## Estructura del Proyecto
```bash
sistemaStock/
├── src/
│   ├── models/              # Entidades y objetos de valor
│   │   ├── Producto.cs               (abstracta)
│   │   ├── Televisor.cs
│   │   ├── Heladera.cs
│   │   ├── Lavarropas.cs
│   │   ├── Sucursal.cs
│   │   ├── CarritoItem.cs
│   │   ├── VentaDetalle.cs
│   │   └── Result.cs                 (patrón Result<T>)
│   ├── db/                  # Capa de acceso a datos
│   │   ├── ConexionDB.cs             (conexión MySQL)
│   │   ├── ProductosQuerys.cs        (CRUD productos)
│   │   ├── SucursalQuerys.cs         (lectura sucursales)
│   │   └── VentasQuerys.cs           (ventas con transacción)
│   ├── services/            # Lógica de negocio
│   │   ├── ProductoService.cs
│   │   └── SucursalService.cs
│   ├── controllers/         # Validación y encauzamiento (async + Result<T>)
│   │   ├── ProductoController.cs
│   │   └── SucursalController.cs
│   ├── ui/                  # Interfaz con Spectre.Console
│   │   ├── MenuPrincipal.cs
│   │   ├── MenuSucursal.cs
│   │   ├── MenuProducto.cs
│   │   ├── MenuCrear.cs
│   │   ├── MenuEditar.cs             (campos editables + extras del tipo)
│   │   ├── MenuEliminar.cs
│   │   ├── MenuBuscar.cs             (por nombre o código)
│   │   ├── MenuVender.cs             (carrito con control de stock)
│   │   ├── MenuVentas.cs             (historial de ventas)
│   │   └── MenuComprobante.cs

├── esquema.sql              # Schema de la base de datos MySQL
├── Program.cs
├── sistemaStock.csproj
└── sistemaStock.sln
```

## Funcionalidades
- Gestionar productos (crear, editar, eliminar, listar con tabla)
- Edición parcial por campos (nombre, precio, cantidad, detalles del tipo)
- Buscar productos por nombre o código
- 2 sucursales con stock independiente (Centro, Norte)
- Carrito de compras con control de stock en tiempo real
- Venta con transacción (descuenta stock + registra detalle)
- Comprobante de venta generado en terminal
- Historial de ventas por sucursal

## Requisitos
- .NET 10 SDK
- MySQL 8+ (ejecutar `esquema.sql` para crear la base de datos)
- Variable de entorno `CONEXION_STRING` con la cadena de conexión

## Ejecutar
```bash
dotnet run --project sistemaStock.csproj
```