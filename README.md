# Project Name: Shopping-Backend  
Shopping-Backend is a backend project built with ASP.NET Core that contains a RESTful API that manage users and products. The API includes JWT authentication, model validations with Data Annotations, and uses MySQL database. This project in development.

## Features
- Full CRUD operations for Products and Users
- Model validation using Data Annotations
- Secure authentication using JSON Web Tokens
- Clean architecture and folder structure
- MySQL integration

## Technologies Used
- ASP.NET Core 8
- Entity Framework Core
- MySQL
- JWT
- BCrypt.Net
- Swagger

## Project Structure
API_Shopping/  
├── Context/  
├───── AppDbContext.cs  
├── Controllers/  
├───── AuthController.cs  
├───── ProductsController.cs  
├───── UsersController.cs  
├───── DetailController.cs  
├── DTOs/  
├───── Detail/  
├───────── DetailCreateDTO.cs  
├───── Product/  
├───────── ProductCreateDTO.cs  
├───────── ProductResponseDTO.cs  
├───────── ProductUpdateDTO.cs  
├───── User/  
├───────── UserCreateDTO.cs  
├───────── UserDTO.cs  
├───────── UserUpdateDTO.cs  
├── Exceptions/  
├───── OutOfStockException.cs  
├───── ProductNotFoundException.cs  
├── Interfaces/  
├───── IDetailService.cs  
├───── IProductService.cs  
├───── IUserService.cs  
├── Middleware/  
├───── ExceptionMiddleware.cs  
├── Models/  
├───── AuthResponseDTO.cs  
├───── LoginDTO.cs  
├───── Detail.cs  
├───── Order.cs  
├───── User.cs  
├───── UserSessions.cs  
├───── Product.cs  
├── Services/  
├───── DetailService.cs  
├───── JwtService.cs  
├───── ProductService.cs  
├───── UserService.cs  
├── Program.cs  
└── appsettings.json  

## Endpoints

# Auth
1. Login endpoint: POST /api/Auth/login

# User
1. Add user endpoint: POST /api/Users
2. List user endpoint: GET: /api/Users
3. Get user endpoint: GET: /api/Users/id
4. Update user endpoint: PUT /api/Users/id 
5. Disable user endpoint: PUT api/Users/disable/id
6. Enable user endpoint: PUT api/Users/enable/id

# Products
1. Add product endpoint: POST /api/Products
2. List product endpoint: GET: /api/Products
3. Get product endpoint: GET: /api/Products/id
4. Delete product endpoint: DELETE api/Products/id
5. Update product endpoint: PUT /api/Products/id

## Installation and Run the project
1. Clone repository:
```bash
git clone https://github.com/dennis-gomez/Shopping-Backend.git
```
3. Configurate jwt and connection to the database in appsettings.json:
```json
"ConnectionStrings": {
  "DefaultConnection": "server=name_host;port=number_port;database=name_database;user=user_name;password=your_password;"
},
"JwtService": {
  "Key": "secret_key",
  "Duration": "60"
}
```
4. Build project:
```bash
dotnet build
```
3. Run the project:
```bash
dotnet run
```
