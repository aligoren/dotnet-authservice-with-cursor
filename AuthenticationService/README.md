# 🔐 Multi-Provider Authentication API

<div align="center">

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)

</div>

## 📋 Table of Contents

- [🚀 Features](#-features)
- [⚙️ Installation](#️-installation)
- [🔧 Configuration](#-configuration)
- [📝 API Usage](#-api-usage)
- [🔒 Security](#-security)
- [📦 Database](#-database)
- [🧪 Testing](#-testing)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)

## 🚀 Features

- 🔑 **Multiple Authentication Providers**
  - JWT (JSON Web Token)
  - LDAP (Lightweight Directory Access Protocol)
  - OAuth2
  - Microsoft Entra ID (Azure AD)

- 💾 **Database**
  - SQLite support
  - Entity Framework Core
  - Identity Framework integration

- 🛡️ **Security**
  - Token-based authentication
  - Role-based authorization
  - Secure password policies

- 📚 **API Documentation**
  - Swagger/OpenAPI integration
  - Detailed endpoint descriptions

## ⚙️ Installation

1. Clone the repository:
```bash
git clone https://github.com/username/AuthenticationService.git
cd AuthenticationService
```

2. Install dependencies:
```bash
dotnet restore
```

3. Create the database:
```bash
dotnet ef database update
```

4. Run the application:
```bash
dotnet run
```

## 🔧 Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=auth.db"
  },
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  },
  "LdapInstances": {
    "mainoffice": {
      "Server": "ldap.example.com",
      "Port": 389,
      "BaseDn": "dc=example,dc=com"
    }
  },
  "OAuth2": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "RedirectUri": "https://localhost:7001/callback"
  },
  "EntraIdInstances": {
    "main": {
      "TenantId": "your-tenant-id",
      "ClientId": "your-client-id",
      "ClientSecret": "your-client-secret",
      "Instance": "https://login.microsoftonline.com/",
      "CallbackPath": "/signin-oidc"
    }
  }
}
```

## 📝 API Usage

### 🔐 Authentication

#### Register
```http
POST /api/auth/register
```
```json
{
  "username": "test@example.com",
  "email": "test@example.com",
  "password": "Test123!",
  "firstName": "Test",
  "lastName": "User"
}
```

#### Login
```http
POST /api/auth/login
```
```json
{
  "username": "test@example.com",
  "password": "Test123!",
  "provider": "jwt"
}
```

#### Validate Token
```http
POST /api/auth/validate
```

#### Test Endpoint
```http
GET /api/auth/test
```
> ⚠️ This endpoint requires a Bearer token.

### 🔑 Supported Authentication Methods

#### JWT
- Standard token-based authentication
- Configurable token expiration
- Refresh token support

#### LDAP
- Active Directory integration
- Multiple LDAP server support
- Secure connection (LDAPS)

#### OAuth2
- Social media login
- Token management
- Authorization flow

#### Microsoft Entra ID
- Azure AD integration
- Multi-tenant support
- Modern authentication

## 🔒 Security

- ✅ HTTPS requirement
- ✅ Secure password policies
- ✅ Rate limiting
- ✅ CORS configuration
- ✅ XSS and CSRF protection

## 📦 Database

SQLite database schema:

```
📁 Tables
├── AspNetUsers
│   ├── Id
│   ├── UserName
│   ├── Email
│   ├── FirstName
│   └── LastName
├── AspNetRoles
├── AspNetUserRoles
├── AspNetUserClaims
├── AspNetRoleClaims
├── AspNetUserLogins
└── AspNetUserTokens
```

## 🤝 Testing

The project includes comprehensive unit tests for all components:

### Authentication Services
- **JwtAuthenticationService**
  - Login with valid credentials
  - Login with invalid credentials
  - Token validation
  - Token expiration

- **AuthenticationServiceFactory**
  - Service creation for different providers
  - Instance validation
  - Configuration validation

### Endpoints
- **AuthEndpoints**
  - Login endpoint
  - Register endpoint
  - Token validation endpoint
  - Error handling

- **TestEndpoints**
  - Protected endpoint access
  - Authorization validation

### Test Coverage
- Services: JWT authentication, service factory
- Endpoints: Authentication and test endpoints
- Success and failure scenarios
- Error conditions

To run the tests:
```bash
dotnet test
```

## 🤝 Contributing

1. Fork it
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'feat: Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Create a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

<div align="center">
Made with ❤️ by Your Team
</div>

Access the API documentation at:
```
https://localhost:7001/swagger
```

## Security Features

- Strong password policy
  - Minimum 8 characters
  - At least 1 uppercase letter
  - At least 1 lowercase letter
  - At least 1 number
  - At least 1 special character
- Token-based authorization
- HTTPS requirement
- Token expiration 