JSON Web Tokens are an open, industry standard RFC 7519 method for representing claims securely between two parties.
JWT.IO allows you to decode, verify and generate JWT.
https://jwt.io/


Required Packages
- Microsoft.IdentityModel.Tokens
- Microsoft.AspNetCore.Authentication.JwtBearer


Configuration
Add your JWT settings to the appsettings.json


Models
- UserLoginRequest
- UserProfile

Services + DI
- AuthenticationService
	- ValidateUserCredentials

Troubleshooting
- Update-Package System.IdentityModel.Tokens.Jwt

