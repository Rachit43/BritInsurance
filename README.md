# BritInsurance API

### Authentication
- **JWT Authentication**: Implement JWT authentication for secure access to the API.
- Use endpoint /login to authenticate users and issue JWT tokens.
- Test username is 'test' and password is 'password'
- To fetch access token from refresh token, use the endpoint /login/refresh.

### Deployment
- Service can be deployed on IIS/Web service or any other web server

### Environment Setup
- Use .Net 8 sdk for development
- Use SQL Server 2019 for database