# Sistema de Stock
Aplicación de consola para gestión de inventario en múltiples sucursales.
## Tecnologías
- C# (.NET 10)
- Spectre.Console (interfaz de terminal)
- Arquitectura por capas (models, services, controllers, UI)
## Estructura del Proyecto
sistemaStock/
├── src/
│   ├── models/          # Entidades y objetos de valor
│   │   ├── Producto.cs           (abstracta)
│   │   ├── Herramienta.cs
│   │   ├── MaterialInsumo.cs
│   │   ├── AccesorioEquipamiento.cs
│   │   ├── Sucursal.cs
│   │   ├── CarritoItem.cs
│   │   └── Result.cs             (patrón Result<T>)
│   ├── data/
│   │   └── Data.cs               (datos en memoria)
│   ├── services/
│   │   ├── ProductoService.cs
│   │   └── SucursalService.cs
│   ├── controllers/
│   │   ├── ProductoController.cs
│   │   └── SucursalController.cs
│   ├── ui/
│   │   ├── MenuPrincipal.cs
│   │   ├── MenuSucursal.cs
│   │   ├── MenuProducto.cs
│   │   ├── MenuCrear.cs
│   │   ├── MenuEditar.cs
│   │   ├── MenuEliminar.cs
│   │   ├── MenuBuscar.cs
│   │   ├── MenuVender.cs         (con carrito)
│   │   ├── MenuComprobante.cs
│   │   └── Alerta.cs
│   └── utils/
│       └── GenerarID.cs
├── Program.cs
├── sistemaStock.csproj
└── sistemaStock.sln
## Funcionalidades
- Gestionar productos (crear, editar, eliminar, listar)
- Buscar productos por nombre
- Carrito de compras con múltiples productos
- Comprobante de venta generado en terminal
- 2 sucursales con stock independiente
## Ejecutar
```bash
dotnet run --project sistemaStock.csproj
Los datos se mantienen en memoria durante la sesión.