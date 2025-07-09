### Run Instructions
1. Clone the repo
2. Run 'dotnet restore'
3. Run 'dotnet build'
4. Run 'dotnet ef migrations add InitialSettings --project DataAccessLayer --startup-project APIs'
5. Run `dotnet ef database update --project DataAccessLayer --startup-project APIs`
6. Run The project and browse Swagger
7. Use /api/Auth/login to get JWT tokin from response body:
	Make a Post request with:
	username = "admin"
	password = "password"
8. After receiving the Token, Press on the Authorize Button on the upper right of Swagger to pass the Token and get Authorized (or use Postman to send requests using the Token).
9. Use 'dotnet test' to run unit tests


# 🧩 Product & Student Management API

A secure, well-structured .NET 8 Web API implementing CRUD operations for `Product` and `Student` entities, with clean architecture practices, JWT authentication, logging, and unit testing.

## 🔧 Technologies Used

- .NET 8 Web API
- Entity Framework Core (Code First)
- SQL Server
- JWT Bearer Authentication
- xUnit & Moq for unit testing
- Serilog for structured logging
- Swagger / OpenAPI
- Clean architecture with separation of concerns

## ✅ Features

- ✅ CRUD operations for `Product` and `Student`
- ✅ Repository-like service layer abstraction
- ✅ Global exception handling with `ProblemDetails` standard
- ✅ JWT Authentication to protect endpoints
- ✅ Fluent model validation using Data Annotations
- ✅ Unit Tests with mocked dependencies
- ✅ Structured logging via Serilog
- ✅ Swagger for API testing

📋 Sample Endpoints
Method	       Endpoint	                    Description	                Auth Required
GET          	/api/products	                Get all products	            ✅ Yes
GET	          /api/products/{id}	          Get product by ID	            ✅ Yes
POST	        /api/products	                Create new product	          ✅ Yes
PUT          	/api/products/{id}	          Update product	              ✅ Yes
DELETE	      /api/products/{id}	          Delete product	              ✅ Yes

Similar endpoints exist for /api/students.


👨‍💻 Author
Alaa Atef

Email: alaaatef97@gmail.com

