**How to use:**

1. Create an database named 'sample_csp_db' on your local SQLServer.
2. Execute the SQL script 'script.sql' to initialize the user data.
3. Build this project and copy the artifacts to folder "Server/SecurityProviders" or "Portal/SecurityProviders" *(this depends on where your identity server location in your Wyn.conf settings)*
4. Restart the Wyn service.
5. Log in to the admin portal and enable the security provider "SqlDBSecurityProvider", and fill in SQLServer database connection string for the database created in the step above.
6. Then you can log in to Wyn with the users "alice/alice" and "bob/bob" which are the users initialized in the sample database "sample_csp_db" you created above.

**Note:**

1. The user password stored in database is just the base64 encoded text of the real user password. The decryption is part of the SQLHelper functions in the code.
2. There is three supported custom user context initialized in the database, they are "height", "weight", and "birthday". Thus if you want to see the usage of the User Contexts, you will need to add the related custom properties in the admin portal to make them available.
3. By default, the user "alice" belongs to roles "developer" and "tester", and the user "bob" belongs to role "tester". To see the mapping and usage of authorization settings for the different users and organizations, you should create these roles in the admin portal.
4. By default, the user "alice" belongs to the organization "org1" and the user "bob" belongs to the organization "org2". To see the behavior of Organization Context and use the Organization managment features of the custom security proider, you also need to create these organizations in the admin portal.

For more information about how to setup Organizations, Users, and Roles, refer to the documentation here: https://wyn.grapecity.com/docs/administration-guide/Security-Management/ 
