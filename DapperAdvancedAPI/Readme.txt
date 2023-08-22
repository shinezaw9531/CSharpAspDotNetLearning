1. Bulk insert in dapper | Dapper advanced #1
-	create table Book
	-	query
		create table Book(
			Id uniqueidentifier not null default newid() primary key,
			Title nvarchar(100) not null CHECK (LEN(Title) >=4),
			Author nvarchar(100) not null,
			Year int null,
		)

		create type typBook as table(
			Title nvarchar(100),
			Author nvarchar(100),
			[Year] int
		)

		create proc sp_AddBooks @typBook dbo.typBook readonly
		as
		begin
			insert into Book(Title,Author,[Year])
			select * from @typBook
		end

		declare @book typBook

		insert into @book (title,author,[year]) values ('book1','author 1',2009)
		insert into @book (title,author,[year]) values ('book2','author 2',2008)
		insert into @book (title,author,[year]) values ('book3','author 3',2007)
		insert into @book (title,author,[year]) values ('book4','author 4',2006)

		exec sp_AddBooks @book

		select * from Book

- create project DapperAdvancedAPI
- create DapperAdvanced.Data model Book.cs,repositories IBookRepository.cs
- create BooksController.cs

builder.Services.AddTransient<IBookRepository, BookRepository>();


2.Output parameter with Dapper | Dapper Advanced #2
----
	create stored procedure
	---
	create proc uspGetBookDetail
	@Id uniqueidentifier, 
	@Title nvarchar(100) output, 
	@Author nvarchar(100) output
	as
	begin
		select @Title=Title,@Author=Author from Book where Id=@Id
	end


	select * from Book

3.Handling multiple results | Dapper advanced #3
----
	create table genre(
		Id int identity(1,1) primary key,
		Name nvarchar(100) null
	);

	insert into genre (Name) values
	('Scifi'),('Programming'),('SelfHelp');

	select * from genre
	select * from Book

	create model Genre.cs

4. Transactions in dapper | Dapper advanced #4
-----------
	create table [Order](
		Id int identity(1,1) primary key,
		OrderDate DateTime
	);

	create table OrderDetail(
		Id int identity(1,1) primary key,
		OrderId int not null,
		ProductId uniqueidentifier not null,
		Price int not null,
		Quantity int not null
	);
	crete model Order.cs and OrderDetails.cs
	create OrderRepository.cs and IOrderRepository.cs and OrderController.cs
	builder.Services.AddTransient<IOrderRepository, OrderRepository>();


5.Multi mapping in dapper | Dapper advanced #5
------
	Need to add query to Order "CustomerName,PhoneNumber"

	ALTER TABLE [Order]
	ADD CustomerName nvarchar(50),PhoneNumber nvarchar(11);

	Order.cs model to add 
		public string? CustomreName { get; set; }
		public string? PhoneNumber { get; set; }
		public ICollection<OrderDetail>? OrderDetails { get; set; }
	
	OrderDeatil.cs model to add
	 public Order? Order { get; set; }
     public Book? Book { get; set; }

	 select o.Id, o.CustomerName, o.PhoneNumber,
    b.Id,b.Title,b.Author,b.[Year],
    od.Id,od.OrderId,od.ProductId,od.Price,od.Quantity
    from OrderDetail od join [Order] o
    on od.OrderId = o.id
    join book b on od.ProductId=b.Id
	-------
	create GetBooksWithGenres() in book

	public List<OrderDetail>? OrderDetails { get; set; }
        public List<GenreVm>? Genres { get; set; }


		 public class GenreVm
    {
        public int GenreId { get; set; }
        public string? GenreName { get; set; }
    }

	create table Book_Genre(
		Id int identity(1,1) primary key,
		BookId uniqueidentifier not null,
		GenreId int
	)


		select b.Id, b.Title, b.Author, b.Year, g.Id As GenreId, g.Name As GenreName
		From Book b
		Inner Join Book_Genre bg on b.Id = bg.BookId
		Inner Join Genre g on bg.GenreId = g.Id


	
