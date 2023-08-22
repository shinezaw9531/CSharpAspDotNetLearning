
	
	---create db
	create table Employees(
	Id int primary key IDENTITY(1,1) not null,
	FirstName varchar(50) not null,
	LastName varchar(50) not null,
	DateOfBirth date not null,
	Email nvarchar(50) not null,
	Salary float not null
)
----------------------------------------
	-- stored procedure
	------------------------
	-- Read All Employees--------------------
create proc usp_Get_Employees
as
begin
	select Id, FirstName, LastName, DateOfBirth, Email, Salary from Employees with (nolock)
end

-- GetById--------------------------
create proc usp_Get_EmployeeById
(
	@Id int
)
as
begin
	select Id, FirstName, LastName, DateOfBirth, Email, Salary from Employees with (nolock)
	where Id = @Id
end

-- Insert-----------------
create proc usp_Insert_Employee
(
	@FirstName		varchar(50)
	,@LastName		varchar(50)
	,@DateOfBirth	date
	,@Email			varchar(50)
	,@Salary		float
)
as
begin
begin try
begin tran
	insert into Employees(FirstName,LastName,DateOfBirth,Email,Salary)
	values
	(
		@FirstName
		,@LastName
		,@DateOfBirth
		,@Email
		,@Salary
	)
commit tran
end try
begin catch
	rollback tran
end catch
end

-- Update---------------------------------
create proc usp_Update_Employee
(
	@Id				int
	,@FirstName		varchar(50)
	,@LastName		varchar(50)
	,@DateOfBirth	date
	,@Email			varchar(50)
	,@Salary		float
)
as
begin
declare @RowCount int = 0
	begin try
		set @RowCount = (select count(1) from Employees with (nolock) where Id = @Id)

		If(@RowCount > 0)
			begin
				begin tran
					update Employees
						set
							FirstName	= @FirstName,
							LastName	= @LastName,
							DateOfBirth = @DateOfBirth,
							Email		= @Email,
							Salary		= @Salary
						where Id = @Id
				commit tran
			end
		end try
		begin catch
			rollback tran
		end catch
end

-- Delete ------------------
create proc usp_Delete_Employee
(
	@Id int
)
as
begin
declare @RowCount int = 0
	begin try
		set @RowCount = (select count(1) from Employees with (nolock) where Id = @Id)
		if(@RowCount > 0)
			begin
				begin tran
					delete from Employees
						where Id = @Id
				commit tran
			end
	end try
	begin catch
		rollback tran
	end catch
end
------------------------

	- create Employe_DAL.cs
	- Employee.cs model and EmployeeController.cs
	- program.cs => builder.Services.AddScoped<Employee_DAL>();



-------------------







