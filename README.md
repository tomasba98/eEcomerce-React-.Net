# 🛒 eCommerce API BackEnd <br/>
This is a backend project for an eCommerce system, developed with ASP.NET Core. The application allows managing products, comments, and purchase orders, integrating authentication and authorization for registered users.

## 🧾 Description
The backend is built in C# with ASP.NET Core, using Entity Framework Core as the ORM and PostgreSQL as the relational database. The API exposes endpoints for managing products, comments, orders, and users, with a distinction between anonymous and registered users.

## 🚀 Features
### Anonymous users:
- Can view products and comments.

### Registered users:
- Publish new products.
- Comment on products.
- Generate purchase orders.

## 🔐 Security and Authentication
- Password encryption using SHA-256.

- Authentication and authorization via JWT tokens with expiration.

- Protection of sensitive endpoints based on user roles.

## 🛠️ Architecture and Best Practices
Dependency injection for greater decoupling and testability.

- Implementation of the Generic Repository design pattern via GenericService and GenericDao.

- Clear separation of layers (Controller, Service, DAO).

- Standardized error handling and responses.

## 📦 Technologies Used
C# / ASP.NET Core

- Entity Framework Core

- PostgreSQL

- Swagger for API documentation and testing

## UML Diagrams
Class diagram of the API architecture. <br/>
![ENTITIES!](UML/Entities.png)
![SERVICES!](UML/Services.png)
![CONTROLLERS!](UML/Controllers.png)

## DER of the data base.
![DER!](UML/DER.png)

