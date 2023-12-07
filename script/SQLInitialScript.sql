USE MASTER
GO

IF NOT EXISTS (
	SELECT name  
      FROM master.sys.server_principals
     WHERE name = 'techtest_login')
 BEGIN
	 CREATE LOGIN techtest_login WITH PASSWORD = 'techtest_password', DEFAULT_DATABASE = master;
 END

IF NOT EXISTS (
 SELECT [name]
   FROM [sys].[database_principals]
  WHERE [type] = N'S' AND [name] = N'techtest_user')
 BEGIN
	CREATE USER techtest_user FOR LOGIN techtest_login
 END

ALTER ROLE db_owner ADD MEMBER techtest_user;

GRANT CREATE ANY DATABASE TO techtest_login WITH GRANT OPTION;

GRANT CREATE LOGIN TO techtest_login;

IF NOT EXISTS(
	SELECT name
	  FROM master.sys.asymmetric_keys
	WHERE name = 'LoginPassKey')
 BEGIN
	CREATE ASYMMETRIC KEY [LoginPassKey]
		AUTHORIZATION [dbo]
		WITH ALGORITHM = RSA_2048
		ENCRYPTION BY PASSWORD = N'AlddagtaeYksvGnm0deaSmdemsFT7_&#$!~<kilzz@Bvrala';
 END

IF DB_ID('tenants') IS NOT NULL
	DROP DATABASE tenants

CREATE DATABASE tenants

USE tenants

IF NOT EXISTS (
	SELECT name  
      FROM master.sys.server_principals
     WHERE name = 'tenants_login')
BEGIN
	 CREATE LOGIN tenants_login WITH PASSWORD = 'tenants_password', DEFAULT_DATABASE = tenants;
END

IF NOT EXISTS (
 SELECT [name]
   FROM [sys].[database_principals]
  WHERE [type] = N'S' AND [name] = N'tenants_user')
 BEGIN
	CREATE USER tenants_user FOR LOGIN tenants_login
 END

CREATE TABLE tenants(
	TenantId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(100) NOT NULL,
	ConnectionString VARCHAR(500) NOT NULL
)

CREATE INDEX ix_tenants_name
ON tenants (Name)

GO
CREATE PROCEDURE [dbo].[Usp_Tenants_INS]
	@pc_Name VARCHAR(100),
	@pc_ConnectionString VARCHAR(500)
AS
BEGIN
	INSERT 
	  INTO tenants
	VALUES (@pc_Name, 
			@pc_ConnectionString)
END

GO
CREATE PROCEDURE [dbo].[Usp_Tenants_SEL]
	@pc_Name VARCHAR(100)
AS
BEGIN
	SELECT TenantId,
		   Name,
		   ConnectionString
	  FROM tenants
	 WHERE Name = @pc_Name
END

GO

GRANT EXECUTE ON OBJECT::[dbo].[Usp_Tenants_SEL]
   TO tenants_user

GRANT EXECUTE ON OBJECT::[dbo].[Usp_Tenants_INS]
   TO tenants_user

USE master

IF DB_ID('login') IS NOT NULL
 BEGIN
	DROP DATABASE login
 END

CREATE DATABASE login

USE login

IF NOT EXISTS (
	SELECT name  
      FROM master.sys.server_principals
     WHERE name = 'login_login')
 BEGIN
	 CREATE LOGIN login_login WITH PASSWORD = 'login_password', DEFAULT_DATABASE = login;
 END

IF NOT EXISTS (
 SELECT [name]
   FROM [sys].[database_principals]
  WHERE [type] = N'S' AND [name] = N'login_user')
 BEGIN
	CREATE USER login_user FOR LOGIN login_login
 END

CREATE TABLE users(
	UserId INT IDENTITY PRIMARY KEY,
	Email VARCHAR(100) NOT NULL,
	Password VARBINARY(256) NOT NULL,
	OrganizationId INT
)

CREATE TABLE organizations(
	OrganizationId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(300),
	SlugTenant VARCHAR(100)
)

CREATE INDEX ix_users_email
ON users (Email)

GO
CREATE PROCEDURE [dbo].[Usp_CheckPassword]
	@pc_Email INT,
	@pc_Password VARCHAR(MAX)
AS
BEGIN
	DECLARE 
		@vc_Salt VARCHAR(50) = 'F*Ws^2MJAySbz*e^RMbZ',
		@vb_EncryptedPassword VARBINARY(256)

	SET @pc_Password = CONCAT(@pc_Password, @vc_Salt)

	SELECT @vb_EncryptedPassword = Password
	  FROM [dbo].[Users]
	 WHERE Email = @pc_Email

	SELECT 
		CASE WHEN @pc_Password = CONVERT(VARCHAR(MAX), DECRYPTBYASYMKEY(ASYMKEY_ID('FluxPassKey'), @vb_EncryptedPassword, N'lu&1OGEwtyL9NBWyTI#hW0XIIly4omcrxr3Fa4z1LnG$NZx#6S')) THEN 1
		ELSE 0
	END

END

GO
CREATE PROCEDURE [dbo].[Usp_Users_INS]
	@pc_Email INT,
	@pc_Password VARCHAR(MAX),
	@pi_OrganizationId INT
AS
BEGIN
	DECLARE 
		@vc_Salt VARCHAR(50) = 'F*Ws^2MJAySbz*e^RMbZ',
		@vb_EncryptedPassword VARBINARY(256)

	INSERT 
	  INTO [dbo].[Users]
	VALUES (@pc_Email,
	        ENCRYPTBYASYMKEY(ASYMKEY_ID('FluxPassKey'), CONCAT(@pc_Password, @vc_Salt)),
			@pi_OrganizationId)
END

GO
CREATE PROCEDURE [dbo].[Usp_UserByEmail]
	@pc_Email VARCHAR(100)
AS
BEGIN
	SELECT UserId,
		   Email
	  FROM users
	 WHERE Email = @pc_Email
END

GO
CREATE PROCEDURE [dbo].[Usp_Organizations_SEL]
	@pi_OrganizationId INT = NULL
AS
BEGIN
	SELECT OrganizationId,
		   Name,
		   SlugTenant
	  FROM organizations
	 WHERE OrganizationId = COALESCE(@pi_OrganizationId, OrganizationId)
END

GO
CREATE PROCEDURE [dbo].[Usp_Organizations_INS]
	@pc_Name VARCHAR(100),
	@pc_SlugTenant VARCHAR(100)
AS
BEGIN
	INSERT	
	  INTO organizations
	VALUES (@pc_Name,
			@pc_SlugTenant)
END

GRANT EXECUTE ON OBJECT::[dbo].[Usp_CheckPassword]
   TO login_user

GRANT EXECUTE ON OBJECT::[dbo].[Usp_Users_INS]
   TO login_user

GRANT EXECUTE ON OBJECT::[dbo].[Usp_UserByEmail]
   TO login_user

GRANT EXECUTE ON OBJECT::[dbo].[Usp_Organizations_SEL]
   TO login_user

GRANT EXECUTE ON OBJECT::[dbo].[Usp_Organizations_INS]
   TO login_user
