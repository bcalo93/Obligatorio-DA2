/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2017 (14.0.1000)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/
USE [master]
GO
/****** Object:  Database [LogDb]    Script Date: 02/07/2019 2:36:36 ******/
CREATE DATABASE [LogDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LogDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLSERVER2014\MSSQL\DATA\LogDb.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'LogDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLSERVER2014\MSSQL\DATA\LogDb_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [LogDb] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LogDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LogDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LogDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LogDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LogDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LogDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [LogDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LogDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LogDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LogDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LogDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LogDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LogDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LogDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LogDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LogDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LogDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LogDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LogDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LogDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LogDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LogDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LogDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LogDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [LogDb] SET  MULTI_USER 
GO
ALTER DATABASE [LogDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LogDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LogDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LogDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [LogDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LogDb] SET QUERY_STORE = OFF
GO
USE [LogDb]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 02/07/2019 2:36:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](max) NULL,
	[LogType] [nvarchar](max) NULL,
	[LogDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'fb6bee6e-7481-4c3b-a6bb-08d6f458f8be', N'admin', N'login', CAST(N'2019-06-18T22:53:48.6641258' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'56d02996-5169-4d75-a6bc-08d6f458f8be', N'admin', N'login', CAST(N'2019-06-18T22:54:52.0232691' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'6e935eff-3d95-4599-1a7b-08d6f4d7226d', N'admin', N'login', CAST(N'2019-06-19T13:56:55.1908455' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'b33f6cf1-26a7-4bac-ac7c-08d6f4dad5a1', N'testmanager', N'login', CAST(N'2019-06-19T14:23:24.3640787' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'603a54aa-5793-4335-ac7d-08d6f4dad5a1', N'testmanager', N'login', CAST(N'2019-06-19T14:23:34.1697014' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'4f307b3e-f912-40b9-ac7e-08d6f4dad5a1', N'testmanager', N'login', CAST(N'2019-06-19T14:23:35.5142795' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'3feb50f1-0671-411a-ac7f-08d6f4dad5a1', N'testmanager', N'login', CAST(N'2019-06-19T14:23:36.6669052' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'0dbbe91e-e023-450c-ac80-08d6f4dad5a1', N'admin', N'login', CAST(N'2019-06-19T14:23:38.2299041' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'61f16813-be44-40bc-be66-08d6f4ded16e', N'manager', N'login', CAST(N'2019-06-19T14:51:55.3034620' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'1049c466-21c4-404e-be67-08d6f4ded16e', N'admin', N'login', CAST(N'2019-06-19T14:51:59.1113405' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'fb7a4d28-1d5a-4517-a51d-08d6f4ef52a6', N'admin', N'login', CAST(N'2019-06-19T16:50:04.0135350' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'c338b10c-535c-4cf9-a51e-08d6f4ef52a6', N'admin', N'import', CAST(N'2019-06-19T16:54:01.7674062' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'639c7f8c-d6e9-4db7-a51f-08d6f4ef52a6', N'admin', N'import', CAST(N'2019-06-19T16:54:30.4775899' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'0929ce3e-fd42-48a0-601a-08d6f4f74df1', N'admin', N'import', CAST(N'2019-06-19T17:47:12.1215648' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'd5e01799-6cd5-4aed-601b-08d6f4f74df1', N'admin', N'import', CAST(N'2019-06-19T17:49:05.7691495' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'bceb63f9-8ebb-4bea-e15c-08d6f4fcc760', N'admin', N'login', CAST(N'2019-06-19T18:26:23.3359310' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'f43b6944-1b4a-4c7a-e15d-08d6f4fcc760', N'admin', N'import', CAST(N'2019-06-19T18:31:11.9797555' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'67bce52c-b2c5-4f87-e15e-08d6f4fcc760', N'admin', N'import', CAST(N'2019-06-19T18:31:29.4875117' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'286fbc28-9304-4cf0-f7cb-08d6f58e2929', N'admin', N'login', CAST(N'2019-06-20T11:47:04.4201057' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'ec65eba6-aad9-461b-f7cc-08d6f58e2929', N'admin', N'import', CAST(N'2019-06-20T11:48:35.0782776' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'2150a223-7dda-457f-f7cd-08d6f58e2929', N'admin', N'import', CAST(N'2019-06-20T11:49:14.3489622' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'a5fb1570-fe41-4989-b330-08d6f5e716e5', N'manager', N'login', CAST(N'2019-06-20T22:23:38.9473025' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'266d8c4f-a61e-4657-8a67-08d6fe9a92b9', N'admin', N'login', CAST(N'2019-07-02T00:08:35.9496830' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'bcc114c7-997b-4a28-a7e0-08d6fea8584a', N'managerB', N'login', CAST(N'2019-07-02T01:47:10.8686747' AS DateTime2))
INSERT [dbo].[Logs] ([Id], [Username], [LogType], [LogDate]) VALUES (N'609b1025-ce78-4cf9-a7e1-08d6fea8584a', N'admin', N'login', CAST(N'2019-07-02T01:50:06.3209474' AS DateTime2))
USE [master]
GO
ALTER DATABASE [LogDb] SET  READ_WRITE 
GO
