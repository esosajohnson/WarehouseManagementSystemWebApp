# Warehouse Management System (WMS)

A full-stack Warehouse Management System (WMS) built using ASP.NET Core MVC, Entity Framework Core, and SQL Server.

This project simulates real-world warehouse operations including inbound stock receiving, inventory tracking, and outbound shipment processing. The goal of the project is to move beyond basic CRUD functionality and implement realistic warehouse business workflows.

---

## Features

### Inventory Management
- Product management
- Category management
- Supplier management
- Warehouse location tracking
- Stock level monitoring

### Inbound Operations
- Purchase Orders
- Purchase Order Items
- Goods Receipts
- Goods Receipt Items
- Automatic stock updates when goods are received
- Inventory transaction logging

### Outbound Operations
- Shipment management
- Shipment item tracking
- Stock deduction on dispatch
- Inventory movement history

### Dashboard & UI
- Warehouse dashboard overview
- KPI cards
- Responsive UI using Bootstrap
- Enterprise-style sidebar navigation

### User Management
- Authentication & authorisation
- User management functionality

---

## Tech Stack

### Backend
- ASP.NET Core MVC
- Entity Framework Core
- C#

### Frontend
- Razor Views
- Bootstrap 5
- Bootstrap Icons
- HTML/CSS/JavaScript

### Database
- SQL Server
- SQL Server Management Studio (SSMS)

### Development Tools
- Visual Studio 2026
- Git
- GitHub

---

## Database Design

The system uses a relational SQL database with interconnected entities such as:

- Products
- Categories
- Suppliers
- Employees
- Locations
- StockLevels
- PurchaseOrders
- GoodsReceipts
- Shipments
- InventoryTransactions

Example warehouse flow:

Purchase Order
↓
Goods Receipt
↓
Stock Level Updated
↓
Inventory Transaction Logged
↓
Shipment
↓
Stock Reduced

---

## Screenshots

(Add screenshots here later)

---

## Future Improvements

- Low stock alerts
- Reporting & analytics
- Role-based permissions
- Search and filtering
- Dashboard charts
- Audit trail logging

---

## What I Learned

Through building this project I improved my understanding of:

- ASP.NET Core MVC architecture
- Entity Framework Core relationships
- SQL database design
- Warehouse business workflows
- CRUD operations with business logic
- Debugging complex application issues
- Building enterprise-style software systems

---

## Author

Esosa Johnson  
BEng Computer Hardware & Software Engineering (First Class Honours)
