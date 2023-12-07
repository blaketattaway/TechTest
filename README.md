# TechTest

Execute sql server script SQLInitialScript.sql located on script folder on root

Change server's name on appsettings.json's connection strings to match to our server.

When all is done, execute project.

Protected routes with authorization are:

tenant/products/getAll

tenant/products/getById?productId={}

tenant/products/create

tenant/products/update

tenant/products/delete?productId={}

Unprotected routes are:

api/organizations/register

api/users/register

api/users/login
