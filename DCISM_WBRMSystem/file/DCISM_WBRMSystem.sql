USE [master]
GO
/****** Object:  Database [WBRMSystem]    Script Date: 1/10/2020 5:41:06 PM ******/
CREATE DATABASE [WBRMSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WBRMSystem', FILENAME = N'F:\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\WBRMSystem.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WBRMSystem_log', FILENAME = N'F:\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\WBRMSystem_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [WBRMSystem] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WBRMSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WBRMSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WBRMSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WBRMSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WBRMSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WBRMSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [WBRMSystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WBRMSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WBRMSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WBRMSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WBRMSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WBRMSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WBRMSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WBRMSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WBRMSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WBRMSystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WBRMSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WBRMSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WBRMSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WBRMSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WBRMSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WBRMSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WBRMSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WBRMSystem] SET RECOVERY FULL 
GO
ALTER DATABASE [WBRMSystem] SET  MULTI_USER 
GO
ALTER DATABASE [WBRMSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WBRMSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WBRMSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WBRMSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WBRMSystem] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'WBRMSystem', N'ON'
GO
ALTER DATABASE [WBRMSystem] SET QUERY_STORE = OFF
GO
USE [WBRMSystem]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [WBRMSystem]
GO
/****** Object:  Table [dbo].[Barcode]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Barcode](
	[Id_Barcode] [int] IDENTITY(1,1) NOT NULL,
	[Barcode_Content] [varchar](50) NULL,
	[Url_Img] [varchar](50) NULL,
 CONSTRAINT [PK_Barcode] PRIMARY KEY CLUSTERED 
(
	[Id_Barcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[Id_Cart] [int] IDENTITY(1,1) NOT NULL,
	[Requestor_Name] [varchar](50) NULL,
 CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED 
(
	[Id_Cart] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart_Item]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart_Item](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Id_Item] [int] NOT NULL,
	[Id_Cart] [int] NOT NULL,
	[Status_Before_Released] [varchar](500) NULL,
	[Is_Returned] [bit] NULL,
	[Date_Returned] [datetime] NULL,
	[Status_After_Returned] [varchar](500) NULL,
 CONSTRAINT [PK_Cart_Item] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Condition]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Condition](
	[Id_Condition] [int] NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_Condition] PRIMARY KEY CLUSTERED 
(
	[Id_Condition] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Faculty]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Faculty](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ID_Number] [varchar](50) NOT NULL,
	[First_Name] [varchar](50) NOT NULL,
	[Last_Name] [varchar](50) NOT NULL,
	[Middle_Name] [varchar](50) NULL,
	[Email_Add] [varchar](50) NULL,
	[Address] [varchar](100) NULL,
	[Contact_No] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Faculty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inventory]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Id_Transaction] [int] NOT NULL,
	[Id_Category] [int] NOT NULL,
	[Id_SubCategory] [int] NULL,
	[No_Of_Stock] [int] NULL,
	[No_Of_Damaged] [int] NULL,
	[No_Of_Borrowed] [int] NULL,
 CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[Id_Item] [int] IDENTITY(1,1) NOT NULL,
	[Id_Category] [int] NULL,
	[Category] [varchar](50) NULL,
	[Id_SubCategory] [int] NULL,
	[Id_Barcode] [int] NULL,
	[Item_Name] [varchar](50) NOT NULL,
	[Brand_Model] [varchar](50) NULL,
	[Date_Acquired] [varchar](50) NULL,
	[Custodian] [varchar](50) NULL,
	[Price] [money] NULL,
	[Serial_Number] [varchar](50) NULL,
	[On_Hold_Period] [int] NULL,
	[Remarks] [varchar](50) NULL,
	[Room_No] [varchar](50) NULL,
	[Id_Condition] [int] NULL,
	[Id_Status] [int] NULL,
	[Date_Added] [varchar](50) NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[Id_Item] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemCategory]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemCategory](
	[Id_Category] [int] IDENTITY(1,1) NOT NULL,
	[Category_Name] [varchar](50) NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_ItemCategory] PRIMARY KEY CLUSTERED 
(
	[Id_Category] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemSubCategory]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemSubCategory](
	[Id_SubCategory] [int] IDENTITY(1,1) NOT NULL,
	[Id_Category] [int] NOT NULL,
	[SubCategory_Name] [varchar](50) NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_ItemSubCategory] PRIMARY KEY CLUSTERED 
(
	[Id_SubCategory] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Request]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Request](
	[Id_Request] [int] IDENTITY(1,1) NOT NULL,
	[Id_Transaction] [int] NOT NULL,
	[Id_Cart] [int] NOT NULL,
	[Request_Date] [datetime] NULL,
	[Received_Date] [datetime] NULL,
	[Purpose] [varchar](50) NULL,
	[Id_Status] [int] NOT NULL,
 CONSTRAINT [PK_Request] PRIMARY KEY CLUSTERED 
(
	[Id_Request] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reservation]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservation](
	[Id_Reservation] [int] IDENTITY(1,1) NOT NULL,
	[Id_Transaction] [int] NOT NULL,
	[Id_Room_Cart] [int] NOT NULL,
	[Reservation_Date] [datetime] NULL,
	[Purpose] [varchar](50) NULL,
 CONSTRAINT [PK_Reservation] PRIMARY KEY CLUSTERED 
(
	[Id_Reservation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reservation_Cart]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservation_Cart](
	[Id_Room_Cart] [int] IDENTITY(1,1) NOT NULL,
	[Requestor_Name] [varchar](50) NULL,
 CONSTRAINT [PK_Reservation_Cart] PRIMARY KEY CLUSTERED 
(
	[Id_Room_Cart] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reservation_Cart_Room]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservation_Cart_Room](
	[Id] [int] NOT NULL,
	[Id_Room_Cart] [int] NOT NULL,
	[Id_Room] [int] NOT NULL,
	[Start_Date] [datetime] NULL,
	[End_Date] [datetime] NULL,
	[Start_Time] [time](7) NULL,
	[End_Time] [time](7) NULL,
	[Is_Approved] [bit] NULL,
	[Date_Approved] [datetime] NULL,
	[Approved_By] [varchar](50) NULL,
 CONSTRAINT [PK_Reservation_Cart_Room] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID_Role] [int] IDENTITY(1,1) NOT NULL,
	[Role_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID_Role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[Id_Room] [int] IDENTITY(1,1) NOT NULL,
	[Room_No] [varchar](50) NULL,
	[Capacity] [int] NULL,
	[Room_Type] [varchar](50) NULL,
	[Id_Condition] [int] NOT NULL,
	[Is_Available] [bit] NULL,
 CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED 
(
	[Id_Room] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room_Schedule]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room_Schedule](
	[Id_Schedule] [int] IDENTITY(1,1) NOT NULL,
	[Id_Room] [int] NOT NULL,
	[ClassName] [varchar](50) NULL,
	[Teacher] [varchar](50) NULL,
	[What_Day] [varchar](50) NULL,
	[Start_Time] [time](7) NULL,
	[End_Time] [time](7) NULL,
 CONSTRAINT [PK_Room_Schedule] PRIMARY KEY CLUSTERED 
(
	[Id_Schedule] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[Id_Status] [int] IDENTITY(1,1) NOT NULL,
	[Id_Transaction_Status] [int] NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[Id_Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ID_Number] [varchar](50) NOT NULL,
	[First_Name] [varchar](50) NOT NULL,
	[Last_Name] [varchar](50) NOT NULL,
	[Middle_Name] [varchar](50) NULL,
	[CourseAndYear] [varchar](50) NULL,
	[Email_Add] [varchar](50) NULL,
	[Address] [varchar](50) NULL,
	[Contact_No] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id_Transaction] [int] IDENTITY(1,1) NOT NULL,
	[ID_Number] [varchar](50) NOT NULL,
	[Transaction_Type] [varchar](50) NULL,
	[Purpose] [varchar](50) NULL,
	[Transaction_Date] [datetime] NULL,
	[Start_Date] [datetime] NULL,
	[End_Date] [datetime] NULL,
	[Status] [varchar](50) NULL,
	[Is_Approved] [bit] NULL,
	[Approved_Date] [datetime] NULL,
	[Approved_By] [varchar](50) NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id_Transaction] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction_Status]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction_Status](
	[Id_Transaction_Status] [int] IDENTITY(1,1) NOT NULL,
	[Transaction_Type] [varchar](50) NULL,
 CONSTRAINT [PK_Transaction_Status] PRIMARY KEY CLUSTERED 
(
	[Id_Transaction_Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 1/10/2020 5:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ID_Number] [varchar](50) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Is_Active] [bit] NOT NULL,
	[ID_Role] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [WBRMSystem] SET  READ_WRITE 
GO
