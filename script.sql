USE [sample_csp_db]
GO
/****** Object:  Table [dbo].[users]    Script Date: 08/17/20 14:55:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
  [id] [nvarchar](50) NOT NULL,
  [name] [nvarchar](50) NOT NULL,
  [password] [nvarchar](50) NOT NULL,
  [email] [nvarchar](50) NULL,
  [height] [int] NULL,
  [weight] [float] NULL,
  [birthday] [date] NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
  [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user_orgs]    Script Date: 08/17/20 14:55:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_orgs](
  [user_id] [nvarchar](50) NOT NULL,
  [organization] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user_roles]    Script Date: 08/17/20 14:55:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_roles](
  [user_id] [nvarchar](50) NOT NULL,
  [role] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO

INSERT [dbo].[users] ([id], [name], [password], [email], [height], [weight], [birthday]) VALUES (N'617D1974-FA27-4856-B469-A1C0DE5E38B8', N'alice', N'YWxpY2U=', N'alice@example.com', 178, 75, CAST(N'2000-01-01' AS Date))
INSERT [dbo].[users] ([id], [name], [password], [email], [height], [weight], [birthday]) VALUES (N'F3140E05-BC14-4703-90CA-ED42985D79EF', N'bob', N'Ym9i', N'bob@sample.com', 172, 72, CAST(N'2001-05-05' AS Date))
INSERT [dbo].[user_orgs] ([user_id], [organization]) VALUES (N'617D1974-FA27-4856-B469-A1C0DE5E38B8', N'org1')
INSERT [dbo].[user_orgs] ([user_id], [organization]) VALUES (N'F3140E05-BC14-4703-90CA-ED42985D79EF', N'org2')
INSERT [dbo].[user_roles] ([user_id], [role]) VALUES (N'617D1974-FA27-4856-B469-A1C0DE5E38B8', N'developer')
INSERT [dbo].[user_roles] ([user_id], [role]) VALUES (N'617D1974-FA27-4856-B469-A1C0DE5E38B8', N'tester')
INSERT [dbo].[user_roles] ([user_id], [role]) VALUES (N'F3140E05-BC14-4703-90CA-ED42985D79EF', N'tester')

