USE [master]
GO
/****** Object:  Database [ConDatos]    Script Date: 02/07/2019 2:33:40 ******/
CREATE DATABASE [ConDatos]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ConDatos', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\ConDatos.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ConDatos_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\ConDatos_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ConDatos] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ConDatos].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ConDatos] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ConDatos] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ConDatos] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ConDatos] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ConDatos] SET ARITHABORT OFF 
GO
ALTER DATABASE [ConDatos] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ConDatos] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ConDatos] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ConDatos] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ConDatos] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ConDatos] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ConDatos] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ConDatos] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ConDatos] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ConDatos] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ConDatos] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ConDatos] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ConDatos] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ConDatos] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ConDatos] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ConDatos] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ConDatos] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ConDatos] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ConDatos] SET  MULTI_USER 
GO
ALTER DATABASE [ConDatos] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ConDatos] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ConDatos] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ConDatos] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ConDatos] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ConDatos] SET QUERY_STORE = OFF
GO
USE [ConDatos]
GO
/****** Object:  Table [dbo].[Areas]    Script Date: 02/07/2019 2:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Areas](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[DataSource] [nvarchar](max) NULL,
 CONSTRAINT [PK_Areas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuthTokens]    Script Date: 02/07/2019 2:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthTokens](
	[Id] [uniqueidentifier] NOT NULL,
	[Token] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AuthTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Components]    Script Date: 02/07/2019 2:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Components](
	[Id] [uniqueidentifier] NOT NULL,
	[Position] [int] NOT NULL,
	[ConditionId] [uniqueidentifier] NULL,
	[Discriminator] [nvarchar](max) NOT NULL,
	[Boolean] [bit] NULL,
	[Date] [datetime2](7) NULL,
	[NumberValue] [int] NULL,
	[QueryTextValue] [nvarchar](max) NULL,
	[TextValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_Components] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IndicatorItems]    Script Date: 02/07/2019 2:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndicatorItems](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[ConditionId] [uniqueidentifier] NULL,
	[IndicatorId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_IndicatorItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Indicators]    Script Date: 02/07/2019 2:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Indicators](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[AreaId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Indicators] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAreas]    Script Date: 02/07/2019 2:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAreas](
	[UserId] [uniqueidentifier] NOT NULL,
	[AreaId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserAreas] PRIMARY KEY CLUSTERED 
(
	[AreaId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserIndicators]    Script Date: 02/07/2019 2:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserIndicators](
	[IndicatorId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Alias] [nvarchar](max) NULL,
	[Position] [int] NOT NULL,
	[IsVisible] [bit] NOT NULL,
 CONSTRAINT [PK_UserIndicators] PRIMARY KEY CLUSTERED 
(
	[IndicatorId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 02/07/2019 2:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Username] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Role] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Areas] ([Id], [Name], [DataSource]) VALUES (N'8d61166d-d2b3-44a3-c62c-08d6fe9b7615', N'Area Idiomas Preferidos', N'Server=.\SQLEXPRESS;Database=DatosPrueba;Trusted_Connection=True;MultipleActiveResultSets=True;')
INSERT [dbo].[Areas] ([Id], [Name], [DataSource]) VALUES (N'c10cd57d-4835-4f5e-7513-08d6fea0c38b', N'Area Venta de Reptiles', N'Server=.\SQLEXPRESS;Database=DatosPrueba;Trusted_Connection=True;MultipleActiveResultSets=True;')
INSERT [dbo].[Areas] ([Id], [Name], [DataSource]) VALUES (N'f214da36-90f5-42a4-7514-08d6fea0c38b', N'Area Control de precio del Item', N'Server=.\SQLEXPRESS;Database=DatosPrueba;Trusted_Connection=True;MultipleActiveResultSets=True;')
INSERT [dbo].[Areas] ([Id], [Name], [DataSource]) VALUES (N'5e342da5-b784-4db5-7515-08d6fea0c38b', N'Area Categorias Favoritas', N'Server=.\SQLEXPRESS;Database=DatosPrueba;Trusted_Connection=True;MultipleActiveResultSets=True;')
INSERT [dbo].[Areas] ([Id], [Name], [DataSource]) VALUES (N'0658ee82-2fed-48e2-7516-08d6fea0c38b', N'Area Promedio precios ItemStatus P', N'Server=.\SQLEXPRESS;Database=DatosPrueba;Trusted_Connection=True;MultipleActiveResultSets=True;')
INSERT [dbo].[AuthTokens] ([Id], [Token], [UserId]) VALUES (N'898c7dde-172b-4ce2-e902-08d6fe9a92ab', N'4faef043-d778-4626-b545-046ea506cd65', N'228f04b0-fd01-42a4-610c-08d6e1266369')
INSERT [dbo].[AuthTokens] ([Id], [Token], [UserId]) VALUES (N'054a84b1-1f93-4b37-72cc-08d6fea85848', N'd78aee64-0318-4770-a07b-661dd2b7382d', N'c03a3cc5-ae2b-4bdf-f640-08d6fe9ad798')
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'2b4a6879-5cc4-4cd4-5c4c-08d6fe9be8c7', 0, NULL, N'MinorCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'790e6f1f-c8d3-42df-5c4d-08d6fe9be8c7', 0, N'2b4a6879-5cc4-4cd4-5c4c-08d6fe9be8c7', N'ItemQuery', NULL, NULL, NULL, N'SELECT COUNT(*) As TotalInglesEspanol FROM Account WHERE LangPref = ''Spanish'' OR LangPref = ''English''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'664a5e56-9ceb-4df3-5c4e-08d6fe9be8c7', 1, N'2b4a6879-5cc4-4cd4-5c4c-08d6fe9be8c7', N'ItemQuery', NULL, NULL, NULL, N'SELECT COUNT(*) As TotalOtrosIdiomas FROM Account WHERE LangPref <> ''Spanish'' AND LangPref <> ''English''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'4815ca06-19c1-48b1-3dea-08d6fe9f83d5', 0, NULL, N'AndCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'6effb728-0440-4d9d-3deb-08d6fe9f83d5', 0, N'4815ca06-19c1-48b1-3dea-08d6fe9f83d5', N'MayorEqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'bd1bf794-4f2b-4170-3dec-08d6fe9f83d5', 0, N'6effb728-0440-4d9d-3deb-08d6fe9f83d5', N'ItemQuery', NULL, NULL, NULL, N'SELECT COUNT(*) As TotalInglesEspanol FROM Account WHERE LangPref = ''Spanish'' OR LangPref = ''English''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'e36a6175-f673-458e-3ded-08d6fe9f83d5', 1, N'6effb728-0440-4d9d-3deb-08d6fe9f83d5', N'ItemQuery', NULL, NULL, NULL, N'SELECT COUNT(*) As TotalOtrosIdiomas FROM Account WHERE LangPref <> ''Spanish'' AND LangPref <> ''English''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'1d380c05-1fc8-41d8-3dee-08d6fe9f83d5', 1, N'4815ca06-19c1-48b1-3dea-08d6fe9f83d5', N'MayorCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'6ea80d3b-ee16-4152-3def-08d6fe9f83d5', 0, N'1d380c05-1fc8-41d8-3dee-08d6fe9f83d5', N'ItemQuery', NULL, NULL, NULL, N'SELECT COUNT(*) As TotalEspanol FROM Account WHERE LangPref = ''Spanish''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'a9f7f352-86de-45b9-3df0-08d6fe9f83d5', 1, N'1d380c05-1fc8-41d8-3dee-08d6fe9f83d5', N'ItemQuery', NULL, NULL, NULL, N'SELECT COUNT(*) As TotalIngles FROM Account WHERE LangPref = ''English''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'd55f3544-f6e5-4af6-77e4-08d6fea07799', 0, NULL, N'AndCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'7ab600c5-24b4-4ba3-77e5-08d6fea07799', 0, N'd55f3544-f6e5-4af6-77e4-08d6fea07799', N'MayorEqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'd495ef5c-e4a9-4513-77e6-08d6fea07799', 0, N'7ab600c5-24b4-4ba3-77e5-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT COUNT(*) As TotalInglesEspanol FROM Account WHERE LangPref = ''Spanish'' OR LangPref = ''English''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'7217b4be-2c13-493b-77e7-08d6fea07799', 1, N'7ab600c5-24b4-4ba3-77e5-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT COUNT(*) As TotalOtrosIdiomas FROM Account WHERE LangPref <> ''Spanish'' AND LangPref <> ''English''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'26e19946-1dc3-411d-77e8-08d6fea07799', 1, N'd55f3544-f6e5-4af6-77e4-08d6fea07799', N'MinorEqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'a42dad07-cf15-49ef-77e9-08d6fea07799', 0, N'26e19946-1dc3-411d-77e8-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT COUNT(*) As TotalEspanol FROM Account WHERE LangPref = ''Spanish''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'1e8982f1-ad27-413e-77ea-08d6fea07799', 1, N'26e19946-1dc3-411d-77e8-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT COUNT(*) As TotalIngles FROM Account WHERE LangPref = ''English''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'bd325477-e9ee-4c9e-77eb-08d6fea07799', 0, NULL, N'MinorCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'af221f90-d1cb-40f9-77ec-08d6fea07799', 0, N'bd325477-e9ee-4c9e-77eb-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT SUM(OrdersLine.LineQuantity) AS TotalVendidos FROM (Product INNER JOIN Item ON Product.ProductId = Item.ProductId) INNER JOIN OrdersLine ON Item.ItemId = OrdersLine.ItemId WHERE Product.CatId = 5', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'264aefec-0571-4bca-77ed-08d6fea07799', 1, N'bd325477-e9ee-4c9e-77eb-08d6fea07799', N'ItemNumeric', NULL, NULL, 1500, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'3a7c9eb5-4aa5-409a-77f1-08d6fea07799', 0, NULL, N'AndCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'b226ba69-6d92-4a32-77f2-08d6fea07799', 0, N'3a7c9eb5-4aa5-409a-77f1-08d6fea07799', N'MayorEqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'80e487f2-2e33-4388-77f3-08d6fea07799', 0, N'b226ba69-6d92-4a32-77f2-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT SUM(OrdersLine.LineQuantity) AS TotalVendidos FROM (Product INNER JOIN Item ON Product.ProductId = Item.ProductId) INNER JOIN OrdersLine ON Item.ItemId = OrdersLine.ItemId WHERE Product.CatId = 5', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'ab130808-a952-45f0-77f4-08d6fea07799', 1, N'b226ba69-6d92-4a32-77f2-08d6fea07799', N'ItemNumeric', NULL, NULL, 1500, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'a39b3d3c-cae5-4abe-77f5-08d6fea07799', 1, N'3a7c9eb5-4aa5-409a-77f1-08d6fea07799', N'MinorCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'94c631bb-00e7-444f-77f6-08d6fea07799', 0, N'a39b3d3c-cae5-4abe-77f5-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT SUM(OrdersLine.LineQuantity) AS TotalVendidos FROM (Product INNER JOIN Item ON Product.ProductId = Item.ProductId) INNER JOIN OrdersLine ON Item.ItemId = OrdersLine.ItemId WHERE Product.CatId = 5', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'b375bf00-900a-4254-77f7-08d6fea07799', 1, N'a39b3d3c-cae5-4abe-77f5-08d6fea07799', N'ItemNumeric', NULL, NULL, 4000, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'f940f150-6381-463a-77f8-08d6fea07799', 0, NULL, N'MayorEqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'b05c0755-7a23-4677-77f9-08d6fea07799', 0, N'f940f150-6381-463a-77f8-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'(SELECT SUM(OrdersLine.LineQuantity) AS TotalVendidos FROM (Product INNER JOIN Item ON Product.ProductId = Item.ProductId) INNER JOIN OrdersLine ON Item.ItemId = OrdersLine.ItemId WHERE Product.CatId = 5) ', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'ce3539d6-d4fa-4856-77fa-08d6fea07799', 1, N'f940f150-6381-463a-77f8-08d6fea07799', N'ItemNumeric', NULL, NULL, 4000, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'b918d590-93a3-49f7-77fb-08d6fea07799', 0, NULL, N'MinorCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'19190cee-b434-4495-77fc-08d6fea07799', 0, N'b918d590-93a3-49f7-77fb-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT ItemListPrice FROM Item WHERE ItemId = ''EST-13''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'7ee23d8b-2696-4483-77fd-08d6fea07799', 1, N'b918d590-93a3-49f7-77fb-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT ItemUnitCost FROM Item WHERE ItemId = ''EST-13''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'c7a75440-d8a7-4260-77fe-08d6fea07799', 0, NULL, N'AndCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'48183b2c-05b6-419e-77ff-08d6fea07799', 0, N'c7a75440-d8a7-4260-77fe-08d6fea07799', N'MayorEqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'8fb51d30-86da-4cd6-7800-08d6fea07799', 0, N'48183b2c-05b6-419e-77ff-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT ItemListPrice FROM Item WHERE ItemId = ''EST-13''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'577715e7-68f2-4bfc-7801-08d6fea07799', 1, N'48183b2c-05b6-419e-77ff-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT ItemUnitCost FROM Item WHERE ItemId = ''EST-13''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'a054b7cb-872d-44dd-7802-08d6fea07799', 1, N'c7a75440-d8a7-4260-77fe-08d6fea07799', N'MinorEqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'4c57709a-9997-4aba-7803-08d6fea07799', 0, N'a054b7cb-872d-44dd-7802-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT ItemListPrice FROM Item WHERE ItemId = ''EST-13''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'5358d774-0948-4584-7804-08d6fea07799', 1, N'a054b7cb-872d-44dd-7802-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT (ItemUnitCost * 1.7) As SetentaPorciento FROM Item WHERE ItemId = ''EST-13''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'e83fd112-7eb9-4bb5-7805-08d6fea07799', 0, NULL, N'MayorCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'68fec8a1-51e2-4b6a-7806-08d6fea07799', 0, N'e83fd112-7eb9-4bb5-7805-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT ItemListPrice FROM Item WHERE ItemId = ''EST-13''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'0486b13d-0b95-420e-7807-08d6fea07799', 1, N'e83fd112-7eb9-4bb5-7805-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT (ItemUnitCost * 1.7) As SetentaPorciento FROM Item WHERE ItemId = ''EST-13''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'40703330-26ab-4ae5-7808-08d6fea07799', 0, NULL, N'OrCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'fbb87a37-301e-4762-7809-08d6fea07799', 0, N'40703330-26ab-4ae5-7808-08d6fea07799', N'MinorCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'c10a1a85-d11b-4c82-780a-08d6fea07799', 0, N'fbb87a37-301e-4762-7809-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT Count(*) FROM Account WHERE FavCategory = ''Dogs''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'9279661c-4d08-4725-780b-08d6fea07799', 1, N'fbb87a37-301e-4762-7809-08d6fea07799', N'ItemNumeric', NULL, NULL, 30, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'428d76e4-2f64-47fc-780c-08d6fea07799', 1, N'40703330-26ab-4ae5-7808-08d6fea07799', N'MinorCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'9339cf3b-809a-4eb2-780d-08d6fea07799', 0, N'428d76e4-2f64-47fc-780c-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT Count(*) FROM Account WHERE FavCategory = ''Cats''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'7fd3c28e-9602-40dd-780e-08d6fea07799', 1, N'428d76e4-2f64-47fc-780c-08d6fea07799', N'ItemNumeric', NULL, NULL, 30, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'b835aeb2-2ec3-441e-780f-08d6fea07799', 0, NULL, N'AndCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'225a9e3c-c46f-413b-7810-08d6fea07799', 0, N'b835aeb2-2ec3-441e-780f-08d6fea07799', N'MayorEqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'72e20e9b-e003-4637-7811-08d6fea07799', 0, N'225a9e3c-c46f-413b-7810-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT Count(*) FROM Account WHERE FavCategory = ''Dogs''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'568c1c28-1bf1-4614-7812-08d6fea07799', 1, N'225a9e3c-c46f-413b-7810-08d6fea07799', N'ItemNumeric', NULL, NULL, 30, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'516f7d82-d81e-4e7e-7813-08d6fea07799', 1, N'b835aeb2-2ec3-441e-780f-08d6fea07799', N'AndCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'67218914-42da-4cad-7814-08d6fea07799', 0, N'516f7d82-d81e-4e7e-7813-08d6fea07799', N'MayorEqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'6b0f4f8a-f29e-4834-7815-08d6fea07799', 0, N'67218914-42da-4cad-7814-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'SELECT Count(*) FROM Account WHERE FavCategory = ''Cats''', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'ca22a54b-3908-43a7-7816-08d6fea07799', 1, N'67218914-42da-4cad-7814-08d6fea07799', N'ItemNumeric', NULL, NULL, 30, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'40834e4d-2c87-4914-7817-08d6fea07799', 1, N'516f7d82-d81e-4e7e-7813-08d6fea07799', N'MinorCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'5692c9a3-1064-400c-7818-08d6fea07799', 0, N'40834e4d-2c87-4914-7817-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'(SELECT Count(*) FROM Account)', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'7c54622d-ee2b-49b0-7819-08d6fea07799', 1, N'40834e4d-2c87-4914-7817-08d6fea07799', N'ItemNumeric', NULL, NULL, 100, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'962a34f9-c8b4-48da-781a-08d6fea07799', 0, NULL, N'AndCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'351a3530-a0a3-4ede-781b-08d6fea07799', 0, N'962a34f9-c8b4-48da-781a-08d6fea07799', N'MayorEqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'270a71c8-3ac4-4240-781c-08d6fea07799', 0, N'351a3530-a0a3-4ede-781b-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'(SELECT Count(*) FROM Account WHERE FavCategory = ''Dogs'') ', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'02f1d6dc-781a-4212-781d-08d6fea07799', 1, N'351a3530-a0a3-4ede-781b-08d6fea07799', N'ItemNumeric', NULL, NULL, 30, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'8865bc31-605a-4806-781e-08d6fea07799', 1, N'962a34f9-c8b4-48da-781a-08d6fea07799', N'AndCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'9199b278-a0dd-418e-781f-08d6fea07799', 0, N'8865bc31-605a-4806-781e-08d6fea07799', N'MayorEqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'1ecb9b6b-1e94-41f7-7820-08d6fea07799', 0, N'9199b278-a0dd-418e-781f-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'(SELECT Count(*) FROM Account WHERE FavCategory = ''Cats'')', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'070fdc4e-574d-4781-7821-08d6fea07799', 1, N'9199b278-a0dd-418e-781f-08d6fea07799', N'ItemNumeric', NULL, NULL, 30, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'634d25b9-c148-476a-7822-08d6fea07799', 1, N'8865bc31-605a-4806-781e-08d6fea07799', N'MayorEqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'947ba92d-02d4-4a98-7823-08d6fea07799', 0, N'634d25b9-c148-476a-7822-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'(SELECT Count(*) FROM Account) ', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'02c1693b-86e1-40af-7824-08d6fea07799', 1, N'634d25b9-c148-476a-7822-08d6fea07799', N'ItemNumeric', NULL, NULL, 100, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'63cd54d3-7897-485c-7825-08d6fea07799', 0, NULL, N'MinorCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'18a184d6-64a6-41ca-7826-08d6fea07799', 0, N'63cd54d3-7897-485c-7825-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'(SELECT AVG(LinePrice) FROM OrdersLine WHERE ItemId IN (Select ItemId FROM Item WHERE ItemStatus = ''P'')) ', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'84b2f2b2-4823-42a8-7827-08d6fea07799', 1, N'63cd54d3-7897-485c-7825-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'(SELECT AVG(LinePrice) FROM OrdersLine) ', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'9ed93669-9d16-4d24-7828-08d6fea07799', 0, NULL, N'EqualsCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'58c50559-10d1-41f2-7829-08d6fea07799', 0, N'9ed93669-9d16-4d24-7828-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'(SELECT AVG(LinePrice) FROM OrdersLine WHERE ItemId IN (Select ItemId FROM Item WHERE ItemStatus = ''P'')) ', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'9c2aa3da-f6e2-4da6-782a-08d6fea07799', 1, N'9ed93669-9d16-4d24-7828-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'(SELECT AVG(LinePrice) FROM OrdersLine) ', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'18913552-2665-45dd-782b-08d6fea07799', 0, NULL, N'MayorCondition', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'a3cf872f-d4c8-4df9-782c-08d6fea07799', 0, N'18913552-2665-45dd-782b-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'(SELECT AVG(LinePrice) FROM OrdersLine WHERE ItemId IN (Select ItemId FROM Item WHERE ItemStatus = ''P'')) ', NULL)
INSERT [dbo].[Components] ([Id], [Position], [ConditionId], [Discriminator], [Boolean], [Date], [NumberValue], [QueryTextValue], [TextValue]) VALUES (N'5a64d382-6c16-47db-782d-08d6fea07799', 1, N'18913552-2665-45dd-782b-08d6fea07799', N'ItemQuery', NULL, NULL, NULL, N'(SELECT AVG(LinePrice) FROM OrdersLine) ', NULL)
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'950676a0-ddc9-4a7a-625c-08d6fe9be8c6', N'RED', N'2b4a6879-5cc4-4cd4-5c4c-08d6fe9be8c7', N'07e53c72-8369-45e2-3b69-08d6fe9bc0df')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'e658ca07-e395-494e-da05-08d6fe9f83d3', N'YELLOW', N'4815ca06-19c1-48b1-3dea-08d6fe9f83d5', N'07e53c72-8369-45e2-3b69-08d6fe9bc0df')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'24c0f70c-efd2-4375-3a01-08d6fea07798', N'GREEN', N'd55f3544-f6e5-4af6-77e4-08d6fea07799', N'07e53c72-8369-45e2-3b69-08d6fe9bc0df')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'84bbc145-ad75-4767-3a02-08d6fea07798', N'RED', N'bd325477-e9ee-4c9e-77eb-08d6fea07799', N'00cb7aef-0293-497f-c0f8-08d6fea0ca05')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'e5ac2070-719e-44a9-3a04-08d6fea07798', N'YELLOW', N'3a7c9eb5-4aa5-409a-77f1-08d6fea07799', N'00cb7aef-0293-497f-c0f8-08d6fea0ca05')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'6000ba8c-4af8-4d52-3a05-08d6fea07798', N'GREEN', N'f940f150-6381-463a-77f8-08d6fea07799', N'00cb7aef-0293-497f-c0f8-08d6fea0ca05')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'23c19d4a-d691-4b84-3a06-08d6fea07798', N'RED', N'b918d590-93a3-49f7-77fb-08d6fea07799', N'8718e66c-d7ab-431e-c0f9-08d6fea0ca05')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'd2e98afa-ffef-4aba-3a07-08d6fea07798', N'YELLOW', N'c7a75440-d8a7-4260-77fe-08d6fea07799', N'8718e66c-d7ab-431e-c0f9-08d6fea0ca05')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'7fa3fabe-2909-4203-3a08-08d6fea07798', N'GREEN', N'e83fd112-7eb9-4bb5-7805-08d6fea07799', N'8718e66c-d7ab-431e-c0f9-08d6fea0ca05')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'49e14fac-a9e5-4f89-3a09-08d6fea07798', N'RED', N'40703330-26ab-4ae5-7808-08d6fea07799', N'111a412f-7065-4ee1-c0fa-08d6fea0ca05')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'6042b7e9-0330-413e-3a0a-08d6fea07798', N'YELLOW', N'b835aeb2-2ec3-441e-780f-08d6fea07799', N'111a412f-7065-4ee1-c0fa-08d6fea0ca05')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'6ca19dfb-3fc5-47f6-3a0b-08d6fea07798', N'GREEN', N'962a34f9-c8b4-48da-781a-08d6fea07799', N'111a412f-7065-4ee1-c0fa-08d6fea0ca05')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'7469f81d-3171-49ef-3a0c-08d6fea07798', N'RED', N'63cd54d3-7897-485c-7825-08d6fea07799', N'ef38f225-8f19-426b-c0fb-08d6fea0ca05')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'52f65df3-52d1-406e-3a0d-08d6fea07798', N'YELLOW', N'9ed93669-9d16-4d24-7828-08d6fea07799', N'ef38f225-8f19-426b-c0fb-08d6fea0ca05')
INSERT [dbo].[IndicatorItems] ([Id], [Name], [ConditionId], [IndicatorId]) VALUES (N'2507195f-78c8-4543-3a0e-08d6fea07798', N'GREEN', N'18913552-2665-45dd-782b-08d6fea07799', N'ef38f225-8f19-426b-c0fb-08d6fea0ca05')
INSERT [dbo].[Indicators] ([Id], [Name], [AreaId]) VALUES (N'07e53c72-8369-45e2-3b69-08d6fe9bc0df', N'Idiomas preferidos', N'8d61166d-d2b3-44a3-c62c-08d6fe9b7615')
INSERT [dbo].[Indicators] ([Id], [Name], [AreaId]) VALUES (N'00cb7aef-0293-497f-c0f8-08d6fea0ca05', N'Venta de Reptiles', N'c10cd57d-4835-4f5e-7513-08d6fea0c38b')
INSERT [dbo].[Indicators] ([Id], [Name], [AreaId]) VALUES (N'8718e66c-d7ab-431e-c0f9-08d6fea0ca05', N'Control de precio del Item EST-13', N'f214da36-90f5-42a4-7514-08d6fea0c38b')
INSERT [dbo].[Indicators] ([Id], [Name], [AreaId]) VALUES (N'111a412f-7065-4ee1-c0fa-08d6fea0ca05', N'Categorias Favoritas', N'5e342da5-b784-4db5-7515-08d6fea0c38b')
INSERT [dbo].[Indicators] ([Id], [Name], [AreaId]) VALUES (N'ef38f225-8f19-426b-c0fb-08d6fea0ca05', N'Promedio precios ItemStatus P', N'0658ee82-2fed-48e2-7516-08d6fea0c38b')
INSERT [dbo].[UserAreas] ([UserId], [AreaId]) VALUES (N'c03a3cc5-ae2b-4bdf-f640-08d6fe9ad798', N'8d61166d-d2b3-44a3-c62c-08d6fe9b7615')
INSERT [dbo].[UserAreas] ([UserId], [AreaId]) VALUES (N'c03a3cc5-ae2b-4bdf-f640-08d6fe9ad798', N'c10cd57d-4835-4f5e-7513-08d6fea0c38b')
INSERT [dbo].[UserAreas] ([UserId], [AreaId]) VALUES (N'c03a3cc5-ae2b-4bdf-f640-08d6fe9ad798', N'f214da36-90f5-42a4-7514-08d6fea0c38b')
INSERT [dbo].[UserAreas] ([UserId], [AreaId]) VALUES (N'c03a3cc5-ae2b-4bdf-f640-08d6fe9ad798', N'5e342da5-b784-4db5-7515-08d6fea0c38b')
INSERT [dbo].[UserAreas] ([UserId], [AreaId]) VALUES (N'c03a3cc5-ae2b-4bdf-f640-08d6fe9ad798', N'0658ee82-2fed-48e2-7516-08d6fea0c38b')
INSERT [dbo].[UserAreas] ([UserId], [AreaId]) VALUES (N'aab50587-3570-499e-f641-08d6fe9ad798', N'5e342da5-b784-4db5-7515-08d6fea0c38b')
INSERT [dbo].[UserAreas] ([UserId], [AreaId]) VALUES (N'9c3cbd50-1774-4ea5-f642-08d6fe9ad798', N'c10cd57d-4835-4f5e-7513-08d6fea0c38b')
INSERT [dbo].[UserAreas] ([UserId], [AreaId]) VALUES (N'9c3cbd50-1774-4ea5-f642-08d6fe9ad798', N'f214da36-90f5-42a4-7514-08d6fea0c38b')
INSERT [dbo].[UserIndicators] ([IndicatorId], [UserId], [Alias], [Position], [IsVisible]) VALUES (N'07e53c72-8369-45e2-3b69-08d6fe9bc0df', N'c03a3cc5-ae2b-4bdf-f640-08d6fe9ad798', N'Idiomas preferidos', 2, 1)
INSERT [dbo].[UserIndicators] ([IndicatorId], [UserId], [Alias], [Position], [IsVisible]) VALUES (N'00cb7aef-0293-497f-c0f8-08d6fea0ca05', N'c03a3cc5-ae2b-4bdf-f640-08d6fe9ad798', N'Venta de Reptiles Manager B', 0, 1)
INSERT [dbo].[UserIndicators] ([IndicatorId], [UserId], [Alias], [Position], [IsVisible]) VALUES (N'8718e66c-d7ab-431e-c0f9-08d6fea0ca05', N'c03a3cc5-ae2b-4bdf-f640-08d6fe9ad798', N'Control de precio del Item EST-13', 2147483647, 0)
INSERT [dbo].[UserIndicators] ([IndicatorId], [UserId], [Alias], [Position], [IsVisible]) VALUES (N'111a412f-7065-4ee1-c0fa-08d6fea0ca05', N'c03a3cc5-ae2b-4bdf-f640-08d6fe9ad798', N'Categorias Favoritas Manager B', 0, 1)
INSERT [dbo].[UserIndicators] ([IndicatorId], [UserId], [Alias], [Position], [IsVisible]) VALUES (N'ef38f225-8f19-426b-c0fb-08d6fea0ca05', N'c03a3cc5-ae2b-4bdf-f640-08d6fe9ad798', N'Promedio precios ItemStatus P', 2147483647, 0)
INSERT [dbo].[Users] ([Id], [Name], [LastName], [Username], [Password], [Email], [Role], [IsDeleted]) VALUES (N'228f04b0-fd01-42a4-610c-08d6e1266362', N'manager', N'manager', N'manager', N'manager', N'manager@manager.com', 1, 0)
INSERT [dbo].[Users] ([Id], [Name], [LastName], [Username], [Password], [Email], [Role], [IsDeleted]) VALUES (N'228f04b0-fd01-42a4-610c-08d6e1266369', N'admin', N'admin', N'admin', N'admin', N'admin@admin.com', 0, 0)
INSERT [dbo].[Users] ([Id], [Name], [LastName], [Username], [Password], [Email], [Role], [IsDeleted]) VALUES (N'a231f516-ef5f-40d8-f63f-08d6fe9ad798', N'Manager A', N'Manager A', N'managerA', N'pass', N'managerA@mail.com', 1, 0)
INSERT [dbo].[Users] ([Id], [Name], [LastName], [Username], [Password], [Email], [Role], [IsDeleted]) VALUES (N'c03a3cc5-ae2b-4bdf-f640-08d6fe9ad798', N'Manager B', N'Manager B', N'managerB', N'pass', N'managerB@mail.com', 1, 0)
INSERT [dbo].[Users] ([Id], [Name], [LastName], [Username], [Password], [Email], [Role], [IsDeleted]) VALUES (N'aab50587-3570-499e-f641-08d6fe9ad798', N'Manager C', N'Manager C', N'managerC', N'pass', N'managerC@mail.com', 1, 0)
INSERT [dbo].[Users] ([Id], [Name], [LastName], [Username], [Password], [Email], [Role], [IsDeleted]) VALUES (N'9c3cbd50-1774-4ea5-f642-08d6fe9ad798', N'Manager D', N'Manager D', N'managerD', N'pass', N'managerD@mail.com', 1, 0)
INSERT [dbo].[Users] ([Id], [Name], [LastName], [Username], [Password], [Email], [Role], [IsDeleted]) VALUES (N'd2d45c5a-1898-4b25-f643-08d6fe9ad798', N'Manager E', N'Manager E', N'managerE', N'pass', N'managerE@mail.com', 1, 0)
/****** Object:  Index [IX_AuthTokens_UserId]    Script Date: 02/07/2019 2:33:45 ******/
CREATE NONCLUSTERED INDEX [IX_AuthTokens_UserId] ON [dbo].[AuthTokens]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Components_ConditionId]    Script Date: 02/07/2019 2:33:45 ******/
CREATE NONCLUSTERED INDEX [IX_Components_ConditionId] ON [dbo].[Components]
(
	[ConditionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_IndicatorItems_ConditionId]    Script Date: 02/07/2019 2:33:45 ******/
CREATE NONCLUSTERED INDEX [IX_IndicatorItems_ConditionId] ON [dbo].[IndicatorItems]
(
	[ConditionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_IndicatorItems_IndicatorId]    Script Date: 02/07/2019 2:33:45 ******/
CREATE NONCLUSTERED INDEX [IX_IndicatorItems_IndicatorId] ON [dbo].[IndicatorItems]
(
	[IndicatorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Indicators_AreaId]    Script Date: 02/07/2019 2:33:45 ******/
CREATE NONCLUSTERED INDEX [IX_Indicators_AreaId] ON [dbo].[Indicators]
(
	[AreaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserAreas_UserId]    Script Date: 02/07/2019 2:33:45 ******/
CREATE NONCLUSTERED INDEX [IX_UserAreas_UserId] ON [dbo].[UserAreas]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserIndicators_UserId]    Script Date: 02/07/2019 2:33:45 ******/
CREATE NONCLUSTERED INDEX [IX_UserIndicators_UserId] ON [dbo].[UserIndicators]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuthTokens]  WITH CHECK ADD  CONSTRAINT [FK_AuthTokens_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AuthTokens] CHECK CONSTRAINT [FK_AuthTokens_Users_UserId]
GO
ALTER TABLE [dbo].[Components]  WITH CHECK ADD  CONSTRAINT [FK_Components_Components_ConditionId] FOREIGN KEY([ConditionId])
REFERENCES [dbo].[Components] ([Id])
GO
ALTER TABLE [dbo].[Components] CHECK CONSTRAINT [FK_Components_Components_ConditionId]
GO
ALTER TABLE [dbo].[IndicatorItems]  WITH CHECK ADD  CONSTRAINT [FK_IndicatorItems_Components_ConditionId] FOREIGN KEY([ConditionId])
REFERENCES [dbo].[Components] ([Id])
GO
ALTER TABLE [dbo].[IndicatorItems] CHECK CONSTRAINT [FK_IndicatorItems_Components_ConditionId]
GO
ALTER TABLE [dbo].[IndicatorItems]  WITH CHECK ADD  CONSTRAINT [FK_IndicatorItems_Indicators_IndicatorId] FOREIGN KEY([IndicatorId])
REFERENCES [dbo].[Indicators] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IndicatorItems] CHECK CONSTRAINT [FK_IndicatorItems_Indicators_IndicatorId]
GO
ALTER TABLE [dbo].[Indicators]  WITH CHECK ADD  CONSTRAINT [FK_Indicators_Areas_AreaId] FOREIGN KEY([AreaId])
REFERENCES [dbo].[Areas] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Indicators] CHECK CONSTRAINT [FK_Indicators_Areas_AreaId]
GO
ALTER TABLE [dbo].[UserAreas]  WITH CHECK ADD  CONSTRAINT [FK_UserAreas_Areas_AreaId] FOREIGN KEY([AreaId])
REFERENCES [dbo].[Areas] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserAreas] CHECK CONSTRAINT [FK_UserAreas_Areas_AreaId]
GO
ALTER TABLE [dbo].[UserAreas]  WITH CHECK ADD  CONSTRAINT [FK_UserAreas_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserAreas] CHECK CONSTRAINT [FK_UserAreas_Users_UserId]
GO
ALTER TABLE [dbo].[UserIndicators]  WITH CHECK ADD  CONSTRAINT [FK_UserIndicators_Indicators_IndicatorId] FOREIGN KEY([IndicatorId])
REFERENCES [dbo].[Indicators] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserIndicators] CHECK CONSTRAINT [FK_UserIndicators_Indicators_IndicatorId]
GO
ALTER TABLE [dbo].[UserIndicators]  WITH CHECK ADD  CONSTRAINT [FK_UserIndicators_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserIndicators] CHECK CONSTRAINT [FK_UserIndicators_Users_UserId]
GO
USE [master]
GO
ALTER DATABASE [ConDatos] SET  READ_WRITE 
GO
