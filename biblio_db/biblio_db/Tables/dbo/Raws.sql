CREATE TABLE [dbo].[Raws](
	[RecordId] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[Publishers] [nvarchar](512) NULL,
	[YearOfPublishing] [int] NULL,
	[Authors] [nvarchar](512) NULL,
	[NumOfBooks] [int] NULL,
	[WebFace] [nvarchar](max) NULL,
	[AuthorFamily] [nvarchar](512) NULL,
	[AuthorTrails] [nvarchar](512) NULL,
	[BookShelfSeat] [nvarchar](512) NULL,
 CONSTRAINT [PK_Raws] PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO