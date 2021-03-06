USE [ProveFee]
GO
/****** Object:  Table [dbo].[T_Pro_Class]    Script Date: 2015/11/26 11:09:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_Class](
	[ClassID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[ProfessionID] [int] NULL,
	[Name] [nvarchar](32) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Pro_Class] PRIMARY KEY CLUSTERED 
(
	[ClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Pro_Config]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_Config](
	[ConfigID] [int] IDENTITY(1,1) NOT NULL,
	[VoucherNum] [int] NULL,
	[NoteNum] [int] NULL,
	[PrintNum] [int] NULL,
 CONSTRAINT [PK_T_Pro_Config] PRIMARY KEY CLUSTERED 
(
	[ConfigID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Pro_DisableNote]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_DisableNote](
	[DisableNoteID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[FeeID] [int] NULL,
	[NoteNum] [nvarchar](8) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Pro_DisableNote] PRIMARY KEY CLUSTERED 
(
	[DisableNoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Pro_Fee]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_Fee](
	[FeeID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[DeptID] [int] NULL,
	[VoucherNum] [nvarchar](16) NULL,
	[NoteNum] [nvarchar](8) NULL,
	[FeeTime] [datetime] NULL,
	[ProveID] [int] NULL,
	[FeeMode] [int] NULL,
	[ShouldMoney] [decimal](18, 2) NULL,
	[PaidMoney] [decimal](18, 2) NULL,
	[DiscountMoney] [decimal](18, 2) NULL,
	[Teacher] [nvarchar](32) NULL,
	[PrintNum] [int] NULL,
	[AffirmID] [int] NULL,
	[AffirmTime] [datetime] NULL,
	[Explain] [nvarchar](max) NULL,
	[Remark] [nvarchar](max) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Pro_Fee] PRIMARY KEY CLUSTERED 
(
	[FeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Pro_FeeDetail]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_FeeDetail](
	[FeeDetailID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[FeeID] [int] NULL,
	[ItemDetailID] [int] NULL,
	[Money] [decimal](18, 2) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Pro_FeeDetail] PRIMARY KEY CLUSTERED 
(
	[FeeDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Pro_Item]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_Item](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[DeptID] [int] NULL,
	[ParentID] [int] NULL,
	[Name] [nvarchar](32) NULL,
	[EnglishName] [nvarchar](4) NULL,
	[Remark] [nvarchar](max) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Pro_Sort] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Pro_ItemDetail]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_ItemDetail](
	[ItemDetailID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[ItemID] [int] NULL,
	[Detail] [int] NULL,
	[Sort] [int] NULL,
	[Money] [decimal](18, 2) NULL,
	[Remark] [nvarchar](max) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Pro_ItemDetail] PRIMARY KEY CLUSTERED 
(
	[ItemDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Pro_Note]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_Note](
	[NoteID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[DeptID] [int] NULL,
	[Sort] [tinyint] NULL,
	[InFile] [nvarchar](256) NULL,
	[OutFile] [nvarchar](256) NULL,
	[SuccessNum] [int] NULL,
	[ErrorNum] [int] NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Pro_Note] PRIMARY KEY CLUSTERED 
(
	[NoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Pro_Offset]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_Offset](
	[OffsetID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[FeeID] [int] NULL,
	[ByFeeID] [int] NULL,
	[Money] [decimal](18, 2) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Pro_Offset] PRIMARY KEY CLUSTERED 
(
	[OffsetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Pro_Profession]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_Profession](
	[ProfessionID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[DeptID] [int] NULL,
	[Name] [nvarchar](32) NULL,
	[EnglishName] [nvarchar](4) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Pro_Profession] PRIMARY KEY CLUSTERED 
(
	[ProfessionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Pro_Prove]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_Prove](
	[ProveID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NOT NULL,
	[DeptID] [int] NULL,
	[ClassID] [int] NULL,
	[EnrollTime] [datetime] NULL,
	[StudentID] [int] NULL,
	[ItemID] [int] NULL,
	[IsForce] [tinyint] NULL,
	[Remark] [nvarchar](max) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Pro_FeeManage] PRIMARY KEY CLUSTERED 
(
	[ProveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Pro_Refund]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_Refund](
	[RefundID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[FeeID] [int] NULL,
	[Sort] [int] NULL,
	[RefundMoney] [decimal](18, 2) NULL,
	[RefundTime] [datetime] NULL,
	[PayObject] [nvarchar](32) NULL,
	[Remark] [nvarchar](max) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Pro_Refund] PRIMARY KEY CLUSTERED 
(
	[RefundID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Pro_Student]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Pro_Student](
	[StudentID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[DeptID] [int] NULL,
	[Name] [nvarchar](8) NULL,
	[IDCard] [nvarchar](32) NULL,
	[Sex] [int] NULL,
	[Mobile] [nvarchar](16) NULL,
	[QQ] [nvarchar](16) NULL,
	[WeChat] [nvarchar](32) NULL,
	[Address] [nvarchar](128) NULL,
	[Remark] [nvarchar](max) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Pro_Student] PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Sys_Button]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Sys_Button](
	[ButtonID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[ButtonType] [tinyint] NULL,
	[ShowType] [tinyint] NULL,
	[MenuID] [int] NULL,
	[Num] [int] NULL,
	[Code] [nvarchar](32) NULL,
	[Name] [nvarchar](16) NULL,
	[IconPath] [nvarchar](256) NULL,
	[Queue] [int] NULL,
 CONSTRAINT [PK_T_Sys_Button] PRIMARY KEY CLUSTERED 
(
	[ButtonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Sys_Dept]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Sys_Dept](
	[DeptID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[ParentID] [int] NULL,
	[Name] [nvarchar](16) NULL,
	[ShortName] [nvarchar](8) NULL,
	[Code] [nvarchar](4) NULL,
	[Queue] [int] NULL,
	[Remark] [nvarchar](max) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Sys_Dept] PRIMARY KEY CLUSTERED 
(
	[DeptID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Sys_Log]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Sys_Log](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](16) NULL,
	[FieldName] [nvarchar](32) NULL,
	[ValueOld] [nvarchar](max) NULL,
	[ValueNew] [nvarchar](max) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Sys_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Sys_Menu]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Sys_Menu](
	[MenuID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[ParentID] [int] NULL,
	[Name] [nvarchar](16) NULL,
	[MenuType] [tinyint] NULL,
	[PagePath] [nvarchar](256) NULL,
	[IconPath] [nvarchar](256) NULL,
	[OpenIconPath] [nvarchar](256) NULL,
	[Queue] [int] NULL,
 CONSTRAINT [PK_T_Sys_Menu] PRIMARY KEY CLUSTERED 
(
	[MenuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Sys_Power]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Sys_Power](
	[PowerID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NULL,
	[MenuID] [int] NULL,
	[ButtonID] [int] NULL,
 CONSTRAINT [PK_T_Sys_Power] PRIMARY KEY CLUSTERED 
(
	[PowerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Sys_Purview]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Sys_Purview](
	[PurviewID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[RoleID] [int] NULL,
	[DeptID] [int] NULL,
	[Range] [tinyint] NULL,
 CONSTRAINT [PK_T_Sys_UserDept] PRIMARY KEY CLUSTERED 
(
	[PurviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Sys_Refe]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Sys_Refe](
	[RefeID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[RefeTypeID] [int] NULL,
	[RefeName] [nvarchar](16) NULL,
	[Value] [int] NULL,
	[Queue] [int] NULL,
	[Remark] [nvarchar](max) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Sys_Refe] PRIMARY KEY CLUSTERED 
(
	[RefeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Sys_RefeType]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Sys_RefeType](
	[RefeTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[ModuleName] [nvarchar](16) NULL,
	[TypeName] [nvarchar](16) NULL,
	[Remark] [nvarchar](max) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Sys_RefeType] PRIMARY KEY CLUSTERED 
(
	[RefeTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Sys_Role]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Sys_Role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[RoleType] [tinyint] NULL,
	[Name] [nvarchar](16) NULL,
	[Description] [nvarchar](128) NULL,
	[Remark] [nvarchar](max) NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Sys_Role] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Sys_User]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Sys_User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [tinyint] NULL,
	[LoginName] [nvarchar](16) NULL,
	[Password] [nvarchar](32) NULL,
	[Name] [nvarchar](8) NULL,
	[UserType] [tinyint] NULL,
	[Sex] [tinyint] NULL,
	[Mobile] [nvarchar](16) NULL,
	[Remark] [nvarchar](max) NULL,
	[LoginTime] [datetime] NULL,
	[CreateID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateID] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_T_Sys_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Sys_UserRole]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Sys_UserRole](
	[UserRoleID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[RoleID] [int] NULL,
 CONSTRAINT [PK_T_Sys_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  UserDefinedFunction [dbo].[GetChildrenDeptID]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[GetChildrenDeptID] ( @DeptID INT )
RETURNS TABLE
AS
RETURN
    (
	WITH  a AS ( SELECT   DeptID ,
                            ParentID
                   FROM     T_Sys_Dept
                   WHERE    DeptID = @DeptID
                   UNION ALL
                   SELECT   x.DeptID ,
                            x.ParentID
                   FROM     T_Sys_Dept x ,
                            a
                   WHERE    x.ParentID = a.DeptID
                 )
    SELECT  DeptID
    FROM    a
)

GO
/****** Object:  UserDefinedFunction [dbo].[GetChildrenItemID]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[GetChildrenItemID] ( @ItemID INT )
RETURNS TABLE 
AS
RETURN 
(
	WITH  a AS ( SELECT   ItemID ,
                            ParentID
                   FROM     T_Pro_Item
                   WHERE    ItemID = @ItemID
                   UNION ALL
                   SELECT   x.ItemID ,
                            x.ParentID
                   FROM     T_Pro_Item x ,
                            a
                   WHERE    x.ParentID = a.ItemID
                 )
    SELECT  ItemID
    FROM    a
)

GO
/****** Object:  UserDefinedFunction [dbo].[GetParentDeptID]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[GetParentDeptID] ( @DeptID INT )
RETURNS TABLE
AS
RETURN
    (
		WITH    a AS ( SELECT   DeptID ,
                                    ParentID
                           FROM     T_Sys_Dept
                           WHERE    DeptID = @DeptID
                           UNION ALL
                           SELECT   x.DeptID ,
                                    x.ParentID
                           FROM     T_Sys_Dept x ,
                                    a
                           WHERE    x.DeptID = a.ParentID
                         )
    SELECT  DeptID
    FROM    a
)

GO
/****** Object:  UserDefinedFunction [dbo].[GetParentItemID]    Script Date: 2015/11/26 11:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[GetParentItemID] ( @ItemID INT )
RETURNS TABLE 
AS
RETURN 
       (
		WITH    a AS ( SELECT   ItemID ,
                                    ParentID
                           FROM     T_Pro_Item
                           WHERE    ItemID = @ItemID
                           UNION ALL
                           SELECT   x.ItemID ,
                                    x.ParentID
                           FROM     T_Pro_Item x ,
                                    a
                           WHERE    x.ItemID = a.ParentID
                         )
    SELECT  ItemID
    FROM    a
)


GO
SET IDENTITY_INSERT [dbo].[T_Pro_Config] ON 

GO
INSERT [dbo].[T_Pro_Config] ([ConfigID], [VoucherNum], [NoteNum], [PrintNum]) VALUES (1, 0, 0, 2)
GO
SET IDENTITY_INSERT [dbo].[T_Pro_Config] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Button] ON 

GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (1, 1, 2, 2, 2, 1, N'view', N'查看', N'icon-tip', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (2, 1, 2, 1, 2, 1, N'add', N'添加', N'icon-add', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (3, 1, 2, 1, 2, 1, N'edit', N'修改', N'icon-edit', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (4, 1, 2, 1, 2, 1, N'enable', N'启用', N'icon-ok', 4)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (5, 1, 2, 1, 2, 1, N'disable', N'停用', N'icon-no', 5)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (6, 1, 2, 1, 3, 1, N'add', N'添加', N'icon-add', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (7, 1, 2, 1, 3, 1, N'edit', N'修改', N'icon-edit', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (8, 1, 2, 1, 3, 1, N'view', N'查看', N'icon-tip', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (9, 1, 2, 1, 3, 1, N'enable', N'启用', N'icon-ok', 4)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (10, 1, 2, 1, 3, 1, N'disable', N'停用', N'icon-no', 5)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (11, 1, 2, 1, 4, 1, N'add', N'添加', N'icon-add', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (12, 1, 2, 1, 4, 1, N'edit', N'修改', N'icon-edit', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (13, 1, 2, 1, 4, 1, N'view', N'查看', N'icon-tip', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (14, 1, 2, 1, 4, 1, N'enable', N'启用', N'icon-ok', 4)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (15, 1, 2, 1, 4, 1, N'disable', N'停用', N'icon-no', 5)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (16, 1, 2, 1, 4, 1, N'reset', N'重置密码', N'icon-lock', 6)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (17, 1, 2, 2, 5, 1, N'view', N'查看', N'icon-tip', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (18, 1, 2, 1, 5, 1, N'save', N'保存', N'icon-save', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (19, 1, 2, 1, 5, 1, N'del', N'删除', N'icon-cancel', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (20, 1, 2, 2, 6, 1, N'viewRole', N'查看', N'icon-tip', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (21, 1, 2, 1, 6, 1, N'addRole', N'引入', N'icon-add', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (22, 1, 2, 1, 6, 1, N'delRole', N'删除', N'icon-cancel', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (23, 1, 2, 2, 6, 2, N'viewDept', N'查看', N'icon-tip', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (24, 1, 2, 1, 6, 2, N'addDept', N'引入', N'icon-add', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (25, 1, 2, 1, 6, 2, N'delDept', N'删除', N'icon-cancel', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (26, 1, 2, 2, 7, 1, N'viewRefeType', N'查看', N'icon-tip', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (27, 1, 2, 1, 7, 1, N'addRefeType', N'添加', N'icon-add', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (28, 1, 2, 1, 7, 1, N'editRefeType', N'修改', N'icon-edit', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (29, 1, 2, 1, 7, 1, N'enableRefeType', N'启用', N'icon-ok', 4)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (30, 1, 2, 1, 7, 1, N'disableRefeType', N'停用', N'icon-no', 5)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (31, 1, 2, 2, 7, 2, N'viewRefe', N'查看', N'icon-tip', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (32, 1, 2, 1, 7, 2, N'addRefe', N'添加', N'icon-add', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (33, 1, 2, 1, 7, 2, N'editRefe', N'修改', N'icon-edit', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (34, 1, 2, 1, 7, 2, N'enableRefe', N'启用', N'icon-ok', 4)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (35, 1, 2, 1, 7, 2, N'disableRefe', N'停用', N'icon-no', 5)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (36, 1, 2, 2, 8, 1, N'view', N'查看', N'icon-tip', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (37, 1, 1, 2, 10, 1, N'viewProfession', N'查看', N'icon-tip', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (38, 1, 1, 1, 10, 1, N'addProfession', N'添加', N'icon-add', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (39, 1, 1, 1, 10, 1, N'editProfession', N'修改', N'icon-edit', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (40, 1, 1, 1, 10, 1, N'enableProfession', N'启用', N'icon-ok', 4)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (41, 1, 1, 1, 10, 1, N'disableProfession', N'停用', N'icon-no', 5)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (42, 1, 1, 1, 12, 1, N'add', N'添加', N'icon-add', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (43, 1, 1, 1, 12, 1, N'edit', N'修改', N'icon-edit', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (44, 1, 1, 1, 12, 1, N'view', N'查看', N'icon-tip', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (45, 1, 1, 1, 12, 1, N'enable', N'启用', N'icon-ok', 4)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (46, 1, 1, 1, 12, 1, N'disable', N'停用', N'icon-no', 5)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (47, 1, 1, 1, 13, 1, N'add', N'添加', N'icon-add', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (48, 1, 1, 1, 13, 1, N'edit', N'修改', N'icon-edit', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (49, 1, 1, 2, 13, 1, N'view', N'查看', N'icon-tip', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (50, 1, 1, 1, 13, 1, N'enable', N'启用', N'icon-ok', 4)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (51, 1, 1, 1, 13, 1, N'disable', N'停用', N'icon-no', 5)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (52, 1, 1, 2, 14, 1, N'view', N'查看', N'icon-tip', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (53, 1, 1, 1, 14, 1, N'edit', N'修改', N'icon-edit', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (54, 1, 1, 1, 16, 1, N'add', N'添加', N'icon-add', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (55, 1, 1, 1, 16, 1, N'edit', N'修改', N'icon-edit', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (56, 1, 1, 1, 16, 1, N'print', N'打印', N'icon-print', 5)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (57, 1, 1, 1, 16, 1, N'reprint', N'重新打印', N'icon-print', 6)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (58, 1, 1, 1, 16, 1, N'affirm', N'结账', N'icon-redo', 7)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (59, 1, 1, 1, 16, 1, N'returnaffirm', N'反结账', N'icon-undo', 8)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (60, 1, 1, 1, 16, 1, N'disable', N'作废', N'icon-no', 9)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (61, 1, 1, 1, 16, 1, N'reset', N'重置打印次数', N'icon-reload', 10)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (62, 1, 1, 1, 16, 1, N'derive', N'导出', N'icon-save', 11)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (63, 1, 1, 1, 17, 1, N'add', N'添加', N'icon-add', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (64, 1, 1, 1, 17, 1, N'edit', N'修改', N'icon-edit', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (65, 1, 1, 2, 17, 1, N'view', N'查看', N'icon-tip', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (66, 1, 1, 1, 17, 1, N'enable', N'启用', N'icon-ok', 4)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (67, 1, 1, 1, 17, 1, N'disable', N'停用', N'icon-no', 5)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (68, 1, 1, 2, 18, 1, N'view', N'查看', N'icon-tip', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (69, 1, 1, 2, 16, 1, N'feetime', N'编辑收费时间', N'icon-edit', 12)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (70, 1, 1, 1, 13, 2, N'adddetail', N'添加', N'icon-add', 6)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (71, 1, 1, 1, 13, 2, N'editdetail', N'修改', N'icon-edit', 7)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (72, 1, 1, 2, 13, 2, N'viewdetail', N'查看', N'icon-tip', 8)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (73, 1, 1, 1, 13, 2, N'enabledetail', N'启用', N'icon-ok', 9)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (74, 1, 1, 1, 13, 2, N'disabledetail', N'停用', N'icon-no', 10)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (75, 1, 1, 1, 16, 1, N'upload', N'导入', N'icon-up', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (76, 1, 1, 2, 10, 2, N'viewClass', N'查看', N'icon-tip', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (77, 1, 1, 1, 10, 2, N'addClass', N'添加', N'icon-add', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (78, 1, 1, 1, 10, 2, N'editClass', N'修改', N'icon-edit', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (79, 1, 1, 1, 10, 2, N'enableClass', N'启用', N'icon-ok', 4)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (80, 1, 1, 1, 10, 2, N'disableClass', N'停用', N'icon-no', 5)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (81, 1, 1, 1, 20, 1, N'add', N'添加', N'icon-add', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (82, 1, 1, 1, 20, 1, N'edit', N'修改', N'icon-edit', 3)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (83, 1, 1, 1, 20, 1, N'view', N'查看', N'icon-tip', 5)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (84, 1, 1, 1, 20, 1, N'upload', N'导入证书状态', N'icon-up', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (85, 1, 1, 1, 20, 1, N'editStatus', N'修改证书状态', N'icon-edit', 4)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (86, 1, 1, 1, 20, 1, N'derive', N'导出', N'icon-save', 6)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (87, 1, 1, 1, 22, 1, N'derive', N'导出', N'icon-save', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (88, 1, 1, 1, 23, 1, N'derive', N'导出', N'icon-save', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (89, 1, 1, 1, 24, 1, N'derive', N'导出', N'icon-save', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (90, 1, 1, 1, 25, 1, N'deriveUp', N'导出上传文档', N'icon-down', 1)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (91, 1, 1, 1, 25, 1, N'deriveDown', N'导出错误文档', N'icon-down', 2)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (92, 1, 1, 1, 16, 1, N'deriveTemplate', N'导出收费模板', N'icon-down', 13)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (93, 1, 1, 1, 20, 1, N'deriveTemplate', N'导出变更状态模板', N'icon-down', 7)
GO
INSERT [dbo].[T_Sys_Button] ([ButtonID], [Status], [ButtonType], [ShowType], [MenuID], [Num], [Code], [Name], [IconPath], [Queue]) VALUES (94, 1, 1, 1, 16, 1, N'view', N'查看', N'icon-tip', 4)
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Button] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Dept] ON 

GO
INSERT [dbo].[T_Sys_Dept] ([DeptID], [Status], [ParentID], [Name], [ShortName], [Code], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (1, 1, 0, N'希望教育产业集团', N'希望教育', N'', 1, N'', 0, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000A4AB00EF4CF0 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Dept] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Menu] ON 

GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (1, 1, 0, N'系统管理', 2, N'', N'icon-tip', N'', 100)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (2, 1, 1, N'部门管理', 2, N'Dept/DeptList', N'', N'', 1)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (3, 1, 1, N'角色管理', 2, N'Role/RoleList', N'', N'', 2)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (4, 1, 1, N'用户管理', 2, N'User/UserList', N'', N'', 3)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (5, 1, 1, N'权限设置', 2, N'Power/PowerSet', N'', N'', 4)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (6, 1, 1, N'权限范围设置', 2, N'User/UserSet', N'', N'', 5)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (7, 1, 1, N'基础信息管理', 2, N'Refe/RefeInfo', N'', N'', 6)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (8, 1, 1, N'系统日志', 2, N'Log/LogInfo', N'', N'', 7)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (9, 1, 0, N'基础信息设置', 1, N'', N'icon-tip', N'', 10)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (10, 1, 9, N'专业班级管理', 1, N'Profession/ProfessionList', N'', N'', 1)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (11, 1, 0, N'学生信息管理', 1, N'', N'icon-tip', N'', 3)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (12, 1, 11, N'学生信息', 1, N'Student/StudentList', N'', N'', 1)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (13, 1, 9, N'收费项目管理', 1, N'Item/ItemList', N'', N'', 2)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (14, 1, 9, N'基础设置', 1, N'Config/ConfigList', N'', N'', 3)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (15, 1, 0, N'费用管理', 1, N'', N'icon-tip', N'', 1)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (16, 1, 15, N'费用收取管理', 1, N'Fee/FeeList', N'', N'', 1)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (17, 1, 15, N'费用核销管理', 1, N'Refund/RefundList', N'', N'', 2)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (18, 1, 15, N'作废票据管理', 1, N'DisableNote/DisableNoteList', N'', N'', 3)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (19, 1, 0, N'证书管理', 1, N'', N'icon-tip', N'', 2)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (20, 1, 19, N'证书信息', 1, N'Prove/ProveList', N'', N'', 1)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (21, 1, 0, N'统计报表', 1, N'', N'icon-tip', N'', 4)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (22, 1, 21, N'校区统计报表', 1, N'Report/Dept', N'', N'', 1)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (23, 1, 21, N'证书统计报表', 1, N'Report/Prove', N'', N'', 2)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (24, 1, 21, N'班级统计报表', 1, N'Report/Class', N'', N'', 3)
GO
INSERT [dbo].[T_Sys_Menu] ([MenuID], [Status], [ParentID], [Name], [MenuType], [PagePath], [IconPath], [OpenIconPath], [Queue]) VALUES (25, 1, 21, N'导入记录', 1, N'Note/NoteList', N'', N'', 4)
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Menu] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Power] ON 

GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (1, 1, 1, 0)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (2, 1, 2, 0)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (3, 1, 2, 1)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (4, 1, 2, 2)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (5, 1, 2, 3)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (6, 1, 2, 4)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (7, 1, 2, 5)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (8, 1, 3, 0)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (9, 1, 3, 6)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (10, 1, 3, 7)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (11, 1, 3, 8)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (12, 1, 3, 9)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (13, 1, 3, 10)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (14, 1, 4, 0)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (15, 1, 4, 11)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (16, 1, 4, 12)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (17, 1, 4, 13)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (18, 1, 4, 14)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (19, 1, 4, 15)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (20, 1, 4, 16)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (21, 1, 5, 0)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (22, 1, 5, 17)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (23, 1, 5, 18)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (24, 1, 5, 19)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (25, 1, 6, 0)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (26, 1, 6, 20)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (27, 1, 6, 21)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (28, 1, 6, 22)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (29, 1, 6, 23)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (30, 1, 6, 24)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (31, 1, 6, 25)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (32, 1, 7, 0)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (33, 1, 7, 26)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (34, 1, 7, 27)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (35, 1, 7, 28)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (36, 1, 7, 29)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (37, 1, 7, 30)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (38, 1, 7, 31)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (39, 1, 7, 32)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (40, 1, 7, 33)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (41, 1, 7, 34)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (42, 1, 7, 35)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (43, 1, 8, 0)
GO
INSERT [dbo].[T_Sys_Power] ([PowerID], [RoleID], [MenuID], [ButtonID]) VALUES (44, 1, 8, 36)
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Power] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Purview] ON 

GO
INSERT [dbo].[T_Sys_Purview] ([PurviewID], [UserID], [RoleID], [DeptID], [Range]) VALUES (1, 1, 1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Purview] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Refe] ON 

GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (1, 1, 1, N'启用', 1, 1, N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000000000000000 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (2, 1, 1, N'停用', 2, 2, N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000000000000000 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (3, 1, 2, N'操作类', 1, 1, N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000000000000000 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (4, 1, 2, N'管理类', 2, 2, N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000000000000000 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (5, 1, 3, N'男', 1, 1, N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000000000000000 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (6, 1, 3, N'女', 2, 2, N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000000000000000 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (7, 1, 3, N'保密', 3, 3, N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000000000000000 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (8, 1, 4, N'部门权限', 1, 1, N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000000000000000 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (9, 1, 4, N'个人权限', 2, 2, N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000A4AB00F1C110 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (10, 1, 5, N'固定金额', 1, 1, N'', 1, CAST(0x0000A54F009A70B8 AS DateTime), 1, CAST(0x0000A54F009A70B8 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (11, 2, 5, N'设置下限金额', 2, 2, N'', 1, CAST(0x0000A54F009A84A4 AS DateTime), 1, CAST(0x0000A54F009A84A4 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (12, 1, 6, N'现金', 1, 1, N'', 1, CAST(0x0000A54F00E07DEC AS DateTime), 1, CAST(0x0000A54F00E07DEC AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (13, 1, 6, N'刷卡', 2, 2, N'', 1, CAST(0x0000A54F00E09B38 AS DateTime), 1, CAST(0x0000A54F00E09B38 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (14, 1, 6, N'银行转账', 3, 3, N'', 1, CAST(0x0000A54F00E0A36C AS DateTime), 1, CAST(0x0000A54F00E0A36C AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (15, 1, 6, N'充抵', 4, 4, N'', 1, CAST(0x0000A54F00E0ABA0 AS DateTime), 1, CAST(0x0000A54F00E0ABA0 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (16, 1, 7, N'正常', 1, 1, N'', 1, CAST(0x0000A54F00F2DCA8 AS DateTime), 1, CAST(0x0000A54F00F2DCA8 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (17, 1, 7, N'结账', 2, 2, N'', 1, CAST(0x0000A54F00F2F8C8 AS DateTime), 1, CAST(0x0000A54F00F2F8C8 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (18, 1, 7, N'作废', 9, 9, N'', 1, CAST(0x0000A54F00F32424 AS DateTime), 1, CAST(0x0000A54F00FF1DC4 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (19, 1, 8, N'报名费', 1, 1, N'', 1, CAST(0x0000A55200C272E8 AS DateTime), 1, CAST(0x0000A55200C272E8 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (20, 1, 8, N'培训费', 2, 2, N'', 1, CAST(0x0000A55200C27B1C AS DateTime), 1, CAST(0x0000A55200C27B1C AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (21, 1, 8, N'考试费', 3, 3, N'', 1, CAST(0x0000A55200C2847C AS DateTime), 1, CAST(0x0000A55200C2847C AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (22, 1, 9, N'未交费', 1, 1, N'', 1, CAST(0x0000A55600B15B20 AS DateTime), 1, CAST(0x0000A55600B15B20 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (23, 1, 9, N'已交费', 2, 2, N'', 1, CAST(0x0000A55600B18B2C AS DateTime), 1, CAST(0x0000A55600B18B2C AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (24, 1, 9, N'考试未通过', 3, 3, N'', 1, CAST(0x0000A55600B1948C AS DateTime), 1, CAST(0x0000A55600B1948C AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (25, 1, 9, N'考试通过未领证', 4, 4, N'', 1, CAST(0x0000A55600B19DEC AS DateTime), 1, CAST(0x0000A55600B19DEC AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (26, 1, 9, N'考试通过已领证', 5, 5, N'', 1, CAST(0x0000A55600B1A74C AS DateTime), 1, CAST(0x0000A55600B1A74C AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (27, 1, 9, N'停用', 9, 9, N'', 1, CAST(0x0000A55600B1B1D8 AS DateTime), 1, CAST(0x0000A55600B1B1D8 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (28, 1, 10, N'更正错账', 1, 1, N'', 1, CAST(0x0000A55A00B71074 AS DateTime), 1, CAST(0x0000A55A00B71074 AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (29, 1, 10, N'退款', 2, 2, N'', 1, CAST(0x0000A55A00B72A3C AS DateTime), 1, CAST(0x0000A55A00B72A3C AS DateTime))
GO
INSERT [dbo].[T_Sys_Refe] ([RefeID], [Status], [RefeTypeID], [RefeName], [Value], [Queue], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (30, 1, 8, N'教材费', 4, 4, N'', 1, CAST(0x0000A55B00FA75F8 AS DateTime), 1, CAST(0x0000A55B00FA75F8 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Refe] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Sys_RefeType] ON 

GO
INSERT [dbo].[T_Sys_RefeType] ([RefeTypeID], [Status], [ModuleName], [TypeName], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (1, 1, N'基础数据', N'状态', N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000A4C700EC13F0 AS DateTime))
GO
INSERT [dbo].[T_Sys_RefeType] ([RefeTypeID], [Status], [ModuleName], [TypeName], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (2, 1, N'基础数据', N'系统类别', N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000000000000000 AS DateTime))
GO
INSERT [dbo].[T_Sys_RefeType] ([RefeTypeID], [Status], [ModuleName], [TypeName], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (3, 1, N'基础数据', N'性别', N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000000000000000 AS DateTime))
GO
INSERT [dbo].[T_Sys_RefeType] ([RefeTypeID], [Status], [ModuleName], [TypeName], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (4, 1, N'基础数据', N'权限范围', N'', 1, CAST(0x0000000000000000 AS DateTime), 1, CAST(0x0000A4AB00F1BB34 AS DateTime))
GO
INSERT [dbo].[T_Sys_RefeType] ([RefeTypeID], [Status], [ModuleName], [TypeName], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (5, 1, N'基础数据', N'项目分类', N'', 1, CAST(0x0000A54F009A5498 AS DateTime), 1, CAST(0x0000A54F009A5498 AS DateTime))
GO
INSERT [dbo].[T_Sys_RefeType] ([RefeTypeID], [Status], [ModuleName], [TypeName], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (6, 1, N'费用数据', N'交费方式', N'', 1, CAST(0x0000A54F00E075B8 AS DateTime), 1, CAST(0x0000A54F00E075B8 AS DateTime))
GO
INSERT [dbo].[T_Sys_RefeType] ([RefeTypeID], [Status], [ModuleName], [TypeName], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (7, 1, N'费用数据', N'交费状态', N'', 1, CAST(0x0000A54F00F2BF5C AS DateTime), 1, CAST(0x0000A55200C26604 AS DateTime))
GO
INSERT [dbo].[T_Sys_RefeType] ([RefeTypeID], [Status], [ModuleName], [TypeName], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (8, 1, N'费用数据', N'收费项目', N'', 1, CAST(0x0000A55200C2559C AS DateTime), 1, CAST(0x0000A55200C2559C AS DateTime))
GO
INSERT [dbo].[T_Sys_RefeType] ([RefeTypeID], [Status], [ModuleName], [TypeName], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (9, 1, N'证书数据', N'证书状态', N'', 1, CAST(0x0000A55600B14F68 AS DateTime), 1, CAST(0x0000A55600B14F68 AS DateTime))
GO
INSERT [dbo].[T_Sys_RefeType] ([RefeTypeID], [Status], [ModuleName], [TypeName], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (10, 1, N'核销数据', N'核销类别', N'', 1, CAST(0x0000A55A00B6F580 AS DateTime), 1, CAST(0x0000A55A00B6F580 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[T_Sys_RefeType] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Role] ON 

GO
INSERT [dbo].[T_Sys_Role] ([RoleID], [Status], [RoleType], [Name], [Description], [Remark], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (1, 1, 2, N'超级管理员', N'', N'', 0, CAST(0x0000000000000000 AS DateTime), 0, CAST(0x0000000000000000 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[T_Sys_Role] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Sys_User] ON 

GO
INSERT [dbo].[T_Sys_User] ([UserID], [Status], [LoginName], [Password], [Name], [UserType], [Sex], [Mobile], [Remark], [LoginTime], [CreateID], [CreateTime], [UpdateID], [UpdateTime]) VALUES (1, 1, N'sysadmin', N'3B712DE48137572F3849AABD5666A4E3', N'超级管理员', 2, 3, N'', N'', CAST(0x0000A55B010198EC AS DateTime), 0, CAST(0x0000000000000000 AS DateTime), 0, CAST(0x0000000000000000 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[T_Sys_User] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Sys_UserRole] ON 

GO
INSERT [dbo].[T_Sys_UserRole] ([UserRoleID], [UserID], [RoleID]) VALUES (1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[T_Sys_UserRole] OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（1：正常；2：停用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Class', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Class', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Class', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Class', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Class', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Class', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（1：正常；2：停用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_DisableNote', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'票据号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_DisableNote', @level2type=N'COLUMN',@level2name=N'NoteNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_DisableNote', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_DisableNote', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_DisableNote', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_DisableNote', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（1：未结账；2：结账；9：停用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'凭证号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'VoucherNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'票据号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'NoteNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收费时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'FeeTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交费方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'FeeMode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应收金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'ShouldMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实收金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'PaidMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优惠金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'DiscountMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'教师' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'Teacher'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'打印次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'PrintNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结账人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'AffirmID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结账时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'AffirmTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'Explain'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Fee', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（1：正常；2：停用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_FeeDetail', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收费项' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_FeeDetail', @level2type=N'COLUMN',@level2name=N'ItemDetailID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_FeeDetail', @level2type=N'COLUMN',@level2name=N'Money'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_FeeDetail', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_FeeDetail', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_FeeDetail', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_FeeDetail', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（1：正常；2：停用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Item', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Item', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首字母' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Item', @level2type=N'COLUMN',@level2name=N'EnglishName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Item', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Item', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Item', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Item', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Item', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（1：正常；2：停用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_ItemDetail', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收费项' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_ItemDetail', @level2type=N'COLUMN',@level2name=N'Detail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类（1：固定金额；2：设置下限金额）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_ItemDetail', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_ItemDetail', @level2type=N'COLUMN',@level2name=N'Money'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_ItemDetail', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_ItemDetail', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_ItemDetail', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_ItemDetail', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_ItemDetail', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'记录状态（1：正常；2：停用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Note', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型（1：收费数据；2：证书状态数据）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Note', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导入文件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Note', @level2type=N'COLUMN',@level2name=N'InFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误文件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Note', @level2type=N'COLUMN',@level2name=N'OutFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成功数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Note', @level2type=N'COLUMN',@level2name=N'SuccessNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Note', @level2type=N'COLUMN',@level2name=N'ErrorNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Note', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Note', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（1：正常；2：停用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Offset', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Offset', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Offset', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Offset', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Offset', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（1：正常；2：停用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Profession', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Profession', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首字母' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Profession', @level2type=N'COLUMN',@level2name=N'EnglishName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Profession', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Profession', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Profession', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Profession', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（1：未交费；2：已交费；3：考试未通过；4：考试通过未领证；5：考试通过已领证；9：停用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Prove', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否强制保存（1：否；2：是）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Prove', @level2type=N'COLUMN',@level2name=N'IsForce'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Prove', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Prove', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Prove', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Prove', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Prove', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（1：正常；2：停用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Refund', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'核销方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Refund', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'核销金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Refund', @level2type=N'COLUMN',@level2name=N'RefundMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'核销时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Refund', @level2type=N'COLUMN',@level2name=N'RefundTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付对象' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Refund', @level2type=N'COLUMN',@level2name=N'PayObject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Refund', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Refund', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Refund', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Refund', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Refund', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（1：正常；2：停用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身份证号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'IDCard'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'Sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'Mobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'qq号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'QQ'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'WeChat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'家庭地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Pro_Student', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按钮状态（0：停用；1：启用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Button', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单分类（1：业务类；2：系统类）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Button', @level2type=N'COLUMN',@level2name=N'ButtonType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类（1：权限设置+按钮显示，2：权限设置）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Button', @level2type=N'COLUMN',@level2name=N'ShowType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Button', @level2type=N'COLUMN',@level2name=N'MenuID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按钮分组' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Button', @level2type=N'COLUMN',@level2name=N'Num'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按钮编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Button', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按钮名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Button', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按钮图标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Button', @level2type=N'COLUMN',@level2name=N'IconPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Button', @level2type=N'COLUMN',@level2name=N'Queue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门状态（0：停用；1：启用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Dept', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级部门ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Dept', @level2type=N'COLUMN',@level2name=N'ParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Dept', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门简称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Dept', @level2type=N'COLUMN',@level2name=N'ShortName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Dept', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Dept', @level2type=N'COLUMN',@level2name=N'Queue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Dept', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Dept', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Dept', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Dept', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Dept', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Log', @level2type=N'COLUMN',@level2name=N'TableName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字段名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Log', @level2type=N'COLUMN',@level2name=N'FieldName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改前数据' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Log', @level2type=N'COLUMN',@level2name=N'ValueOld'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改后数据' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Log', @level2type=N'COLUMN',@level2name=N'ValueNew'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Log', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Log', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单状态（0：停用；1：启用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Menu', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级菜单ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Menu', @level2type=N'COLUMN',@level2name=N'ParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Menu', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单分类（1：业务类；2：系统类）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Menu', @level2type=N'COLUMN',@level2name=N'MenuType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单链接' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Menu', @level2type=N'COLUMN',@level2name=N'PagePath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单关闭图标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Menu', @level2type=N'COLUMN',@level2name=N'IconPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单打开图标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Menu', @level2type=N'COLUMN',@level2name=N'OpenIconPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Menu', @level2type=N'COLUMN',@level2name=N'Queue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Power', @level2type=N'COLUMN',@level2name=N'RoleID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Power', @level2type=N'COLUMN',@level2name=N'MenuID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按钮ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Power', @level2type=N'COLUMN',@level2name=N'ButtonID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Purview', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Purview', @level2type=N'COLUMN',@level2name=N'RoleID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Purview', @level2type=N'COLUMN',@level2name=N'DeptID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限范围（1：部门权限；2：个人权限）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Purview', @level2type=N'COLUMN',@level2name=N'Range'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（0：停用；1：启用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Refe', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Refe', @level2type=N'COLUMN',@level2name=N'RefeTypeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Refe', @level2type=N'COLUMN',@level2name=N'RefeName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Refe', @level2type=N'COLUMN',@level2name=N'Queue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Refe', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Refe', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Refe', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Refe', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Refe', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子码表状态（0：停用；1：启用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_RefeType', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模块名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_RefeType', @level2type=N'COLUMN',@level2name=N'ModuleName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_RefeType', @level2type=N'COLUMN',@level2name=N'TypeName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_RefeType', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_RefeType', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_RefeType', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_RefeType', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_RefeType', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色状态（0：停用；1：启用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Role', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色分类（1：业务类；2：系统类）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Role', @level2type=N'COLUMN',@level2name=N'RoleType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Role', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Role', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Role', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Role', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Role', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Role', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_Role', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户状态（0：停用；1：启用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'LoginName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户分类（1：业务类；2：系统类）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'UserType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别（1：男；2：女）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'Sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'Mobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'LoginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'CreateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'UpdateID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_User', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_UserRole', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Sys_UserRole', @level2type=N'COLUMN',@level2name=N'RoleID'
GO
