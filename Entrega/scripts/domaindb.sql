/****** Object:  Database [IndicatorsManagerDb]    Script Date: 19/06/2019 14:49:30 ******/
CREATE DATABASE [IndicatorsManagerDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'IndicatorsManagerDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLSERVER2014\MSSQL\DATA\IndicatorsManagerDb.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'IndicatorsManagerDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLSERVER2014\MSSQL\DATA\IndicatorsManagerDb_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [IndicatorsManagerDb] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [IndicatorsManagerDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [IndicatorsManagerDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [IndicatorsManagerDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [IndicatorsManagerDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [IndicatorsManagerDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [IndicatorsManagerDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [IndicatorsManagerDb] SET  MULTI_USER 
GO
ALTER DATABASE [IndicatorsManagerDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [IndicatorsManagerDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [IndicatorsManagerDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [IndicatorsManagerDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [IndicatorsManagerDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [IndicatorsManagerDb] SET QUERY_STORE = OFF
GO
/****** Object:  Table [dbo].[Areas]    Script Date: 19/06/2019 14:49:30 ******/
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
/****** Object:  Table [dbo].[AuthTokens]    Script Date: 19/06/2019 14:49:30 ******/
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
/****** Object:  Table [dbo].[Components]    Script Date: 19/06/2019 14:49:30 ******/
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
/****** Object:  Table [dbo].[IndicatorItems]    Script Date: 19/06/2019 14:49:31 ******/
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
/****** Object:  Table [dbo].[Indicators]    Script Date: 19/06/2019 14:49:31 ******/
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
/****** Object:  Table [dbo].[UserAreas]    Script Date: 19/06/2019 14:49:31 ******/
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
/****** Object:  Table [dbo].[UserIndicators]    Script Date: 19/06/2019 14:49:31 ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 19/06/2019 14:49:31 ******/
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
INSERT [dbo].[Users] ([Id], [Name], [LastName], [Username], [Password], [Email], [Role], [IsDeleted]) VALUES (N'228f04b0-fd01-42a4-610c-08d6e1266362', N'manager', N'manager', N'manager', N'manager', N'manager@manager.com', 1, 0)
INSERT [dbo].[Users] ([Id], [Name], [LastName], [Username], [Password], [Email], [Role], [IsDeleted]) VALUES (N'228f04b0-fd01-42a4-610c-08d6e1266369', N'admin', N'admin', N'admin', N'admin', N'admin@admin.com', 0, 0)
/****** Object:  Index [IX_AuthTokens_UserId]    Script Date: 19/06/2019 14:49:31 ******/
CREATE NONCLUSTERED INDEX [IX_AuthTokens_UserId] ON [dbo].[AuthTokens]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Components_ConditionId]    Script Date: 19/06/2019 14:49:31 ******/
CREATE NONCLUSTERED INDEX [IX_Components_ConditionId] ON [dbo].[Components]
(
	[ConditionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_IndicatorItems_ConditionId]    Script Date: 19/06/2019 14:49:31 ******/
CREATE NONCLUSTERED INDEX [IX_IndicatorItems_ConditionId] ON [dbo].[IndicatorItems]
(
	[ConditionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_IndicatorItems_IndicatorId]    Script Date: 19/06/2019 14:49:31 ******/
CREATE NONCLUSTERED INDEX [IX_IndicatorItems_IndicatorId] ON [dbo].[IndicatorItems]
(
	[IndicatorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Indicators_AreaId]    Script Date: 19/06/2019 14:49:31 ******/
CREATE NONCLUSTERED INDEX [IX_Indicators_AreaId] ON [dbo].[Indicators]
(
	[AreaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserAreas_UserId]    Script Date: 19/06/2019 14:49:31 ******/
CREATE NONCLUSTERED INDEX [IX_UserAreas_UserId] ON [dbo].[UserAreas]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserIndicators_UserId]    Script Date: 19/06/2019 14:49:31 ******/
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
ALTER DATABASE [IndicatorsManagerDb] SET  READ_WRITE 
GO
