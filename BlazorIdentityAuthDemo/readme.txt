
In Blazor, as well as other ASP.NET Core projects, the default identity security library is called "ASP.NET Core Identity." It's a membership system that allows developers to add login functionality to their applications and supports account creation, password storage, profile management, and more.
User Registration: Allows new users to create accounts.
- Authentication: Verifying the identity of a user, typically via a username and password.
- Authorization: Determining what authenticated users are allowed to do. For instance, certain users might have access to specific sections of an application or certain administrative functionalities.
- Password Management: This includes features like resetting forgotten passwords, enforcing password strength requirements, and allowing users to change their passwords.
- Profile Management: Users can manage their personal details, preferences, and other profile-related information.
- Role Management: Assigning roles to users. Roles are often used to group a set of permissions or access rights. For example, an "Admin" role might have more privileges than a "User" role.
- Security Features: This can include features like two-factor authentication, account lockout after a certain number of failed login attempts, and more.

In essence, a membership system provides the tools and mechanisms required to manage users and their interactions with a web application securely. ASP.NET Core Identity, which we mentioned earlier, is an example of such a system for .NET-based web applications.


Customising ASP.Net Identity in Blazor server side
https://mvc.tech/blog/blazoridentityuser/