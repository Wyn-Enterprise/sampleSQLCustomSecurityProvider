** How to use:
1. Create an SQLServer database named 'sample_csp_db' on your local server.
2. Execute the SQL script 'script.sql' to initialize user data.
3. Build this project and copy the artifacts to folder "Server/SecurityProviders" or "Portal/SecurityProviders"(this depends on where your identity server located, server or portal).
4. Restart the Wyn service.
5. Log in to the admin portal and enable the security provider "SqlDBSecurityProvider", and fill in the correct SQLServer database connection string.
6. Then you can log in to Wyn with the users "alice/alice" and "bob/bob" which stored in the sample database "sample_db".

** Note:
1. The user password stored in database is the just the base64 encoded text of the real user password.
2. There is three supported custom user context stored in the database, they are "height", "weight" and "birthday", so you need to add the related custom properties in the admin portal to make them available.
3. By default, the user "alice" belongs to roles "developer" and "tester", and the user "bob" belongs to role "tester", you should create these roles in the admin portal.
4. By default, the user "alice" belongs to the organization "org1" and the user "bob" belongs to the organization "org2", you also need to create these organizations in the admin portal.
