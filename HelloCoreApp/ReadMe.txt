
1. Create HWController
	Edit Properties->launchSettings.json->//"launchUrl": "SayHello",

2. Create Models->ItemModel.cs
	Create ItemController.cs->HttpGet,HttpPost

3. ADO CRUD
	instll nuget.org-> System.Data.SqlClient

	Query
	create database CSharpAspDotNetLearning
	create table Item(
	ItemID int primary key identity(1,1),
	ItemCode nvarchar(10) NOT NULL,
	ItemName nvarchar(50) NOT NULL,
	Price int NOT NULL
);

	ItemController.cs -> add configuration
	add connection on appSettings.json

	----
	-SqlClient to connect using a SQL Server login:
		Server=ServerName;Database=MSSQLTipsDB;User Id=Username;Password=Password;
	-SqlClient to connect to localhost using Windows Authentication:
		Server=.;Database=MSSQLTipsDB;Trusted_Connection=True;
	-SqlClient to connect to named instance using a port number on localhost using Windows Authentication:
		Server=.\instancename,51688;Database=MSSQLTipsDB;Trusted_Connection=True;
	-SqlClient to connect to SQL Server Express on localhost using Windows Authentication:
		Server=.\SQLExpress;Database=MSSQLTipsDB;Trusted_Connection=True;
	--






