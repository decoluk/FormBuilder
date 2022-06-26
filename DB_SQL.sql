 
CREATE DATABASE [GenericFormBuilder]
 
USE [GenericFormBuilder]
GO
 
CREATE TABLE [dbo].[eLog](
	[lg_id] [bigint] IDENTITY(1,1) NOT NULL,
	[lg_date] [datetime] NOT NULL,
	[lg_data] [xml] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mfbEntity]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mfbEntity](
	[fbe_id] [bigint] NOT NULL,
	[fbe_name] [nvarchar](100) NOT NULL,
	[fbe_mapping_db] [nvarchar](100) NOT NULL,
	[fbe_mapping_table] [nvarchar](100) NOT NULL,
	[fbe_desc] [nvarchar](max) NOT NULL,
	[fbe_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[fbe_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mfbEntityColumn]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mfbEntityColumn](
	[fbec_id] [bigint] NOT NULL,
	[fbe_id] [bigint] NOT NULL,
	[fbec_parent_entity_fbe_id] [bigint] NULL,
	[fbft_id] [bigint] NOT NULL,
	[fbec_name] [nvarchar](100) NOT NULL,
	[fbec_db_field_name] [nvarchar](100) NOT NULL,
	[fbec_db_field_len] [int] NOT NULL,
	[fbec_default_value] [nvarchar](100) NOT NULL,
	[fbec_is_key] [bit] NOT NULL,
	[fbec_is_cust_column] [bit] NOT NULL,
	[fbec_sort_seq] [numeric](3, 2) NOT NULL,
	[fbec_desc] [nvarchar](max) NOT NULL,
	[fbec_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[fbec_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mfbFieldType]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mfbFieldType](
	[fbft_id] [bigint] NOT NULL,
	[fblv_id] [bigint] NULL,
	[fbft_name] [nvarchar](100) NOT NULL,
	[fbft_is_listview] [bit] NOT NULL,
	[fbft_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[fbft_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mfbListView]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mfbListView](
	[fblv_id] [bigint] NOT NULL,
	[fblv_name] [nvarchar](100) NOT NULL,
	[fblv_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[fblv_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mfbListViewItem]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mfbListViewItem](
	[fblvi_id] [bigint] NOT NULL,
	[fblv_id] [bigint] NOT NULL,
	[fbec_id] [bigint] NOT NULL,
	[fblvi_name] [nvarchar](100) NOT NULL,
	[fblvi_fontzie] [int] NOT NULL,
	[fblvi_width] [int] NOT NULL,
	[fblvi_is_cust_column] [bit] NOT NULL,
	[fblvi_is_button] [bit] NOT NULL,
	[fblvi_seq] [numeric](3, 2) NOT NULL,
	[fblvi_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[fblvi_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mFormBuilder]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mFormBuilder](
	[fb_id] [bigint] NOT NULL,
	[fb_name] [nvarchar](100) NOT NULL,
	[fb_key] [nvarchar](100) NOT NULL,
	[fb_date] [datetime] NOT NULL,
	[fb_desc] [nvarchar](max) NOT NULL,
	[fb_is_auto_gen] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mSeqNo]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mSeqNo](
	[sq_id] [int] IDENTITY(1,1) NOT NULL,
	[co_id] [int] NOT NULL,
	[sq_table] [nvarchar](100) NULL,
	[sq_num] [bigint] NOT NULL,
	[sq_date] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[eLog] ADD  DEFAULT (getdate()) FOR [lg_date]
GO
ALTER TABLE [dbo].[mfbEntity] ADD  DEFAULT ('') FOR [fbe_desc]
GO
ALTER TABLE [dbo].[mfbEntity] ADD  DEFAULT (getdate()) FOR [fbe_date]
GO
ALTER TABLE [dbo].[mfbEntityColumn] ADD  DEFAULT ((0)) FOR [fbec_db_field_len]
GO
ALTER TABLE [dbo].[mfbEntityColumn] ADD  DEFAULT ('') FOR [fbec_default_value]
GO
ALTER TABLE [dbo].[mfbEntityColumn] ADD  DEFAULT ((0)) FOR [fbec_is_key]
GO
ALTER TABLE [dbo].[mfbEntityColumn] ADD  DEFAULT ((0)) FOR [fbec_is_cust_column]
GO
ALTER TABLE [dbo].[mfbEntityColumn] ADD  DEFAULT ((0)) FOR [fbec_sort_seq]
GO
ALTER TABLE [dbo].[mfbEntityColumn] ADD  DEFAULT ('') FOR [fbec_desc]
GO
ALTER TABLE [dbo].[mfbEntityColumn] ADD  DEFAULT (getdate()) FOR [fbec_date]
GO
ALTER TABLE [dbo].[mfbFieldType] ADD  DEFAULT ((0)) FOR [fbft_is_listview]
GO
ALTER TABLE [dbo].[mfbFieldType] ADD  DEFAULT (getdate()) FOR [fbft_date]
GO
ALTER TABLE [dbo].[mfbListView] ADD  DEFAULT (getdate()) FOR [fblv_date]
GO
ALTER TABLE [dbo].[mfbListViewItem] ADD  DEFAULT ((0)) FOR [fblvi_fontzie]
GO
ALTER TABLE [dbo].[mfbListViewItem] ADD  DEFAULT ((0)) FOR [fblvi_width]
GO
ALTER TABLE [dbo].[mfbListViewItem] ADD  DEFAULT ((0)) FOR [fblvi_is_cust_column]
GO
ALTER TABLE [dbo].[mfbListViewItem] ADD  DEFAULT ((0)) FOR [fblvi_is_button]
GO
ALTER TABLE [dbo].[mfbListViewItem] ADD  DEFAULT ((0)) FOR [fblvi_seq]
GO
ALTER TABLE [dbo].[mfbListViewItem] ADD  DEFAULT (getdate()) FOR [fblvi_date]
GO
ALTER TABLE [dbo].[mFormBuilder] ADD  DEFAULT ('') FOR [fb_name]
GO
ALTER TABLE [dbo].[mFormBuilder] ADD  DEFAULT ('') FOR [fb_key]
GO
ALTER TABLE [dbo].[mFormBuilder] ADD  DEFAULT (getdate()) FOR [fb_date]
GO
ALTER TABLE [dbo].[mFormBuilder] ADD  DEFAULT ('') FOR [fb_desc]
GO
ALTER TABLE [dbo].[mFormBuilder] ADD  DEFAULT (getdate()) FOR [fb_is_auto_gen]
GO
ALTER TABLE [dbo].[mSeqNo] ADD  DEFAULT ((0)) FOR [sq_num]
GO
ALTER TABLE [dbo].[mSeqNo] ADD  DEFAULT (getdate()) FOR [sq_date]
GO
/****** Object:  StoredProcedure [dbo].[sp_SQLCommandGen]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 
CREATE PROCEDURE [dbo].[sp_SQLCommandGen](@pTable NVARCHAR(100))
AS
BEGIN
--EXEC sp_SQLCommandGen 'mfbEntity'  
	DECLARE @sSQL AS NVARCHAR(MAX)

	SELECT c.column_id,t.name,c.name,ty.name,
	(CASE WHEN ty.name = 'bigint' THEN  
		'@'+ c.name +'		= CONVERT(BIGINT,T.C.value('''+ c.name +'[1]'', ''NVARCHAR(20)'')),' ELSE
			CASE WHEN ty.name = 'numeric' THEN 
				'@'+ c.name +'		= T.C.value('''+ c.name +'[1]'', ''NVARCHAR(20)''),' 
			ELSE
				CASE WHEN c.max_length = -1 THEN 
					'@'+ c.name +'		= T.C.value('''+ c.name +'[1]'', ''NVARCHAR(MAX)''),' 
				ELSE
					CASE WHEN ty.name = 'int' THEN 
						'@'+ c.name +'		= T.C.value('''+ c.name +'[1]'', ''NVARCHAR(12)''),' 
					ELSE
						CASE WHEN ty.name = 'bit' THEN 
							'@'+ c.name +'		= (CASE WHEN T.C.value('''+ c.name +'[1]'', ''NVARCHAR(1)'') = '''' THEN 0 ELSE CASE WHEN T.C.value('''+ c.name +'[1]'', ''NVARCHAR(10)'') = ''false'' THEN 0 ELSE CASE WHEN T.C.value('''+ c.name +'[1]'', ''NVARCHAR(10)'') = ''true'' THEN 1 ELSE CONVERT(BIT,T.C.value('''+ c.name +'[1]'', ''NVARCHAR(1)'')) END END  END),' 
						ELSE
							CASE WHEN ty.name = 'datetime' THEN 
								'@'+ c.name +'		= (CASE WHEN T.C.value('''+ c.name +'[1]'', ''NVARCHAR(25)'') IS NOT NULL THEN CAST(CAST(T.C.value('''+ c.name +'[1]'', ''NVARCHAR(25)'') AS CHAR(8)) AS DATETIME) ELSE NULL END),' 
							ELSE
								CASE WHEN ty.name = 'nvarchar' THEN 
									'@'+ c.name +'		= ISNULL(T.C.value('''+ c.name +'[1]'', ''NVARCHAR('+ CONVERT(NVARCHAR,c.max_length) +')''),''''),' 
								ELSE
									CASE WHEN ty.name = 'varchar' THEN 
										'@'+ c.name +'		= ISNULL(T.C.value('''+ c.name +'[1]'', ''NVARCHAR('+ CONVERT(NVARCHAR,c.max_length/2) +')''),''''),'							
									ELSE
										CASE WHEN ty.name = 'uniqueidentifier' THEN 
											'@'+ c.name +'		= ISNULL(T.C.value('''+ c.name +'[1]'', ''NVARCHAR('+ CONVERT(NVARCHAR,c.max_length) +')''),''''),'							
										ELSE
											'@'+ c.name +'		= ISNULL(T.C.value('''+ c.name +'[1]'', ''NVARCHAR('+ CONVERT(NVARCHAR,c.max_length/2) +')''),''''),'							
										END
									END
								END
							END
						END
					END
				END
			END
	END),
	(CASE WHEN ty.name = 'bigint' THEN  
		'public int? '+ c.name + ';' 
		ELSE
			CASE WHEN ty.name = 'numeric' THEN 
				'public decimal '+ c.name + ';' 
			ELSE
				CASE WHEN c.max_length = -1 THEN 
					'public string '+ c.name + ';' 
				ELSE
					CASE WHEN ty.name = 'int' THEN 
						'public int? '+ c.name + ';' 
					ELSE
						CASE WHEN ty.name = 'bit' THEN 
							'public bool '+ c.name + ';' 
						ELSE
							CASE WHEN ty.name = 'datetime' THEN 
								'public DateTime? '+ c.name + ';' 
							ELSE
								CASE WHEN ty.name = 'uniqueidentifier' THEN
									'public Guid? '+ c.name + ';' 
								ELSE
									'public string '+ c.name + ';' 
								END
							END
						END
					END
				END
			END
	END),
	(CASE WHEN ty.name = 'bigint' THEN  
		'DECLARE @'+ c.name +'	BIGINT' ELSE
			CASE WHEN ty.name = 'numeric' THEN 
				'DECLARE @'+ c.name +'	NUMERIC(20,4)'
			ELSE
				CASE WHEN c.max_length = -1 THEN 
					'DECLARE @'+ c.name +'	XML'
				ELSE
					CASE WHEN ty.name = 'int' THEN 
						'DECLARE @'+ c.name +'	INT'
					ELSE
						CASE WHEN ty.name = 'bit' THEN 
							'DECLARE @'+ c.name +'	BIT'
						ELSE
							CASE WHEN ty.name = 'datetime' THEN 
								'DECLARE @'+ c.name +'	DATETIME'
							ELSE
								CASE WHEN ty.name = 'uniqueidentifier' THEN
									'DECLARE @'+ c.name +'	uniqueidentifier'	
								ELSE
									'DECLARE @'+ c.name +'	NVARCHAR('+ CONVERT(NVARCHAR,c.max_length/2) +')'
								END
							END
						END
					END
				END
			END
	END)
	,
		(CASE WHEN ty.name = 'bigint' THEN  
		'<'+ c.name +'>' + '</'+ c.name +'>'  ELSE
			CASE WHEN ty.name = 'numeric' THEN 
				'<'+ c.name +'>' + '</'+ c.name +'>'
			ELSE
				CASE WHEN c.max_length = -1 THEN 
					'<'+ c.name +'>' + '</'+ c.name +'>'
				ELSE
					CASE WHEN ty.name = 'int' THEN 
						'<'+ c.name +'>' + '</'+ c.name +'>'
					ELSE
						CASE WHEN ty.name = 'bit' THEN 
							'<'+ c.name +'>' + '</'+ c.name +'>'
						ELSE
							CASE WHEN ty.name = 'datetime' THEN 
								'<'+ c.name +'>' + '</'+ c.name +'>'
							ELSE
								'<'+ c.name +'>' + '</'+ c.name +'>'
							END
						END
					END
				END
			END
	END)
	,c.is_nullable,
		(CASE WHEN ty.name = 'bigint' THEN  
			c.name +'	BIGINT,' ELSE
			CASE WHEN ty.name = 'numeric' THEN 
				c.name +'	NUMERIC(20,4),'
			ELSE
				CASE WHEN c.max_length = -1 THEN 
					c.name +'	XML,'
				ELSE
					CASE WHEN ty.name = 'int' THEN 
						c.name +'	INT,'
					ELSE
						CASE WHEN ty.name = 'smallint' THEN
							c.name +'	smallint,'	
						ELSE
							CASE WHEN ty.name = 'float' THEN
								c.name +'	float,'	
							ELSE
								CASE WHEN ty.name = 'varchar' THEN
									c.name +'	VARCHAR('+ CONVERT(NVARCHAR,c.max_length) +'),'
								ELSE
									CASE WHEN ty.name = 'char' THEN
										c.name +'	CHAR('+ CONVERT(NVARCHAR,c.max_length) +'),'
									ELSE
										CASE WHEN ty.name = 'bit' THEN 
											c.name +'	BIT,'
										ELSE
											CASE WHEN ty.name = 'datetime' THEN 
												c.name +'	DATETIME,'
											ELSE
												c.name +'	NVARCHAR('+ CONVERT(NVARCHAR,c.max_length/2) +'),'
											END
										END
									END
								END
							END
						END
					END
				END
			END
	END),
	'DECLARE @tbl_' + @pTable + ' TABLE (','INSERT INTO @tbl_' + @pTable + ' SELECT * FROM ' + @pTable
	
	 FROM 
	sys.columns c INNER JOIN 
	sys.tables t ON c.object_id = t.object_id INNER JOIN 
	sys.types ty ON c.system_type_id = ty.system_type_id
	WHERE t.name IS NOT NULL AND ty.name <> 'sysname'
	AND t.name = @pTable
		ORDER BY t.name,c.column_id


END

                                                      
GO
/****** Object:  StoredProcedure [dbo].[spaEntity]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spaEntity]
(
	@pXML		XML,
	@pRtnXML	XML OUTPUT
)AS
/*
DECLARE @pRtnXML XML
EXEC spaDataSyncImportToClient 
 '<ROOT>
			<coCode>801</coCode>
			<usCode>8</usCode>
			<TYPE>IMPORT</TYPE>
			<dsicCode>63245</dsicCode>
	</ROOT>',@pRtnXML OUTPUT
SELECT @pRtnXML
*/
BEGIN
	DECLARE @TYPE			NVARCHAR(100)	
	DECLARE @SQL_STORED_PROCEDURE NVARCHAR(50)
	DECLARE @co_id			BIGINT
	DECLARE @us_id			BIGINT
	
	DECLARE @RtnRESULT		NVARCHAR(20)
	DECLARE @VIEWRESULT		XML
	SET @RtnRESULT = 'FAIL'
	
	SELECT 
		@TYPE			= T.C.value('TYPE[1]', 'NVARCHAR(100)'),
		@SQL_STORED_PROCEDURE = T.C.value('SQL_STORED_PROCEDURE[1]', 'NVARCHAR(100)'),
		@co_id			= CONVERT(BIGINT,T.C.value('co_id[1]', 'NVARCHAR(20)')),
		@us_id			= CONVERT(INT,T.C.value('us_id[1]', 'NVARCHAR(20)'))
		FROM @pXML.nodes('/ROOT') T(C)

	BEGIN TRY
		IF (@SQL_STORED_PROCEDURE = 'GET')		
		BEGIN
			SET @RtnRESULT = 'SUCCESS'
		END
		ELSE IF (@TYPE = 'GET_PRODUCT')		
		BEGIN
			--SET @VIEWRESULT = (SELECT *,
			--		(SELECT * FROM POS_RetailPrice c2 WHERE c1.pt_code = c2.pt_code FOR XML PATH('POS_RetailPrice'),TYPE) ,
			--		(SELECT * FROM POS_SpecialProduct c3 WHERE c1.pt_code = c3.pt_code FOR XML PATH('POS_SpecialProduct'),TYPE) 
			--	 FROM POS_ProductUPC c1 FOR XML PATH('POS_ProductUPC'), ROOT('POS_ProductUPC_COLLECTION'))
			SET @RtnRESULT = 'SUCCESS'		
		END
		
		SET @pRtnXML = (
			SELECT 
			@RtnRESULT	AS RESULT,
			@VIEWRESULT	AS VIEWRESULT
			FOR XML PATH('XML'), ROOT('ROOT'))
		
	END TRY
	BEGIN CATCH
			SET @pRtnXML = 
				(SELECT 
					'FAIL'	AS RESULT,
					ERROR_NUMBER() AS ErrorNumber
					,ERROR_SEVERITY() AS ErrorSeverity
					,ERROR_STATE() AS ErrorState
					,ERROR_PROCEDURE() AS ErrorProcedure
					,ERROR_LINE() AS ErrorLine
					,ERROR_MESSAGE() AS ErrorMessage
					FOR XML PATH('XML'), ROOT('ROOT'))
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
	END CATCH

	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION;
END


GO
/****** Object:  StoredProcedure [dbo].[spaExecuteSP]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spaExecuteSP]
(
	@pXML		XML,
	@pRtnXML	XML OUTPUT
)AS
/*
DECLARE @pRtnXML XML
EXEC spaExecuteSP 
 '<ROOT>
	<SQL_STORED_PROCEDURE>spaEntity</SQL_STORED_PROCEDURE>
			<co_id>801</co_id>
			<us_id>8</us_id>
			<TYPE>ADD_ENTITY</TYPE>
	</ROOT>',@pRtnXML OUTPUT
SELECT @pRtnXML
*/
BEGIN
	DECLARE @TYPE			NVARCHAR(100)	
	DECLARE @SQL_STORED_PROCEDURE NVARCHAR(50)
	DECLARE @co_id			BIGINT
	DECLARE @us_id			BIGINT
	
	DECLARE @RtnRESULT		NVARCHAR(20)
	DECLARE @VIEWRESULT		XML
	SET @RtnRESULT = 'FAIL'
	
	INSERT INTO eLog(lg_data) VALUES (@pXML)

	SELECT 
		@TYPE			= T.C.value('TYPE[1]', 'NVARCHAR(100)'),
		@SQL_STORED_PROCEDURE = T.C.value('SQL_STORED_PROCEDURE[1]', 'NVARCHAR(100)'),
		@co_id			= CONVERT(BIGINT,T.C.value('co_id[1]', 'NVARCHAR(20)')), -- company id
		@us_id			= CONVERT(INT,T.C.value('us_id[1]', 'NVARCHAR(20)'))  --user id
		FROM @pXML.nodes('/ROOT') T(C)

	BEGIN TRY
		IF (@SQL_STORED_PROCEDURE = 'spaFormBuilder')		
		BEGIN
			EXECUTE spaFormBuilder @pXML,@pRtnXML OUTPUT	
		END
		ELSE IF (@SQL_STORED_PROCEDURE = 'spaEntity')		
		BEGIN
			EXECUTE spaEntity @pXML,@pRtnXML OUTPUT	
		END
	END TRY
	BEGIN CATCH
			SET @pRtnXML = 
				(SELECT 
					'FAIL'	AS RESULT,
					ERROR_NUMBER() AS ErrorNumber
					,ERROR_SEVERITY() AS ErrorSeverity
					,ERROR_STATE() AS ErrorState
					,ERROR_PROCEDURE() AS ErrorProcedure
					,ERROR_LINE() AS ErrorLine
					,ERROR_MESSAGE() AS ErrorMessage
					FOR XML PATH('XML'), ROOT('ROOT'))	
	END CATCH

END


GO
/****** Object:  StoredProcedure [dbo].[spaFormBuilder]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spaFormBuilder]
(
	@pXML		XML,
	@pRtnXML	XML OUTPUT
)AS
/*
DECLARE @pRtnXML XML
EXEC spaDataSyncImportToClient 
 '<ROOT>
			<coCode>801</coCode>
			<usCode>8</usCode>
			<TYPE>IMPORT</TYPE>
			<dsicCode>63245</dsicCode>
	</ROOT>',@pRtnXML OUTPUT
SELECT @pRtnXML
*/
BEGIN
	DECLARE @TYPE			NVARCHAR(100)	
	DECLARE @SQL_STORED_PROCEDURE NVARCHAR(50)
	DECLARE @co_id			BIGINT
	DECLARE @us_id			BIGINT
	
	DECLARE @RtnRESULT		NVARCHAR(20)
	DECLARE @VIEWRESULT		XML
	SET @RtnRESULT = 'FAIL'
	
	SELECT 
		@TYPE			= T.C.value('TYPE[1]', 'NVARCHAR(100)'),
		@SQL_STORED_PROCEDURE = T.C.value('SQL_STORED_PROCEDURE[1]', 'NVARCHAR(100)'),
		@co_id			= CONVERT(BIGINT,T.C.value('co_id[1]', 'NVARCHAR(20)')),
		@us_id			= CONVERT(INT,T.C.value('us_id[1]', 'NVARCHAR(20)'))
		FROM @pXML.nodes('/ROOT') T(C)

	BEGIN TRY
		IF (@SQL_STORED_PROCEDURE = 'GET')		
		BEGIN
			SET @RtnRESULT = 'SUCCESS'
		END
		ELSE IF (@TYPE = 'GET_PRODUCT')		
		BEGIN
			--SET @VIEWRESULT = (SELECT *,
			--		(SELECT * FROM POS_RetailPrice c2 WHERE c1.pt_code = c2.pt_code FOR XML PATH('POS_RetailPrice'),TYPE) ,
			--		(SELECT * FROM POS_SpecialProduct c3 WHERE c1.pt_code = c3.pt_code FOR XML PATH('POS_SpecialProduct'),TYPE) 
			--	 FROM POS_ProductUPC c1 FOR XML PATH('POS_ProductUPC'), ROOT('POS_ProductUPC_COLLECTION'))
			SET @RtnRESULT = 'SUCCESS'		
		END
		
		SET @pRtnXML = (
			SELECT 
			@RtnRESULT	AS RESULT,
			@VIEWRESULT	AS VIEWRESULT
			FOR XML PATH('XML'), ROOT('ROOT'))
		
	END TRY
	BEGIN CATCH
			SET @pRtnXML = 
				(SELECT 
					'FAIL'	AS RESULT,
					ERROR_NUMBER() AS ErrorNumber
					,ERROR_SEVERITY() AS ErrorSeverity
					,ERROR_STATE() AS ErrorState
					,ERROR_PROCEDURE() AS ErrorProcedure
					,ERROR_LINE() AS ErrorLine
					,ERROR_MESSAGE() AS ErrorMessage
					FOR XML PATH('XML'), ROOT('ROOT'))
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
	END CATCH

	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION;
END


GO
/****** Object:  StoredProcedure [dbo].[spaPermission]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spaPermission]
(
	@pXML		XML,
	@pRtnXML	XML OUTPUT
)AS
/*
DECLARE @pRtnXML XML
EXEC spaExecuteSP 
 '<ROOT>
	<SQL_STORED_PROCEDURE>spaEntity</SQL_STORED_PROCEDURE>
			<co_id>801</co_id>
			<us_id>8</us_id>
			<TYPE>ADD_ENTITY</TYPE>
	</ROOT>',@pRtnXML OUTPUT
SELECT @pRtnXML
*/
BEGIN
	DECLARE @TYPE			NVARCHAR(100)	
	DECLARE @SQL_STORED_PROCEDURE NVARCHAR(50)
	DECLARE @co_id			BIGINT
	DECLARE @us_id			BIGINT
	
	DECLARE @RtnRESULT		NVARCHAR(20)
	DECLARE @VIEWRESULT		XML
	SET @RtnRESULT = 'FAIL'
	
	SELECT 
		@TYPE			= T.C.value('TYPE[1]', 'NVARCHAR(100)'),
		@SQL_STORED_PROCEDURE = T.C.value('SQL_STORED_PROCEDURE[1]', 'NVARCHAR(100)'),
		@co_id			= CONVERT(BIGINT,T.C.value('co_id[1]', 'NVARCHAR(20)')), -- company id
		@us_id			= CONVERT(INT,T.C.value('us_id[1]', 'NVARCHAR(20)'))  --user id
		FROM @pXML.nodes('/ROOT') T(C)

	BEGIN TRY
		IF (@SQL_STORED_PROCEDURE = 'spaFormBuilder')		
		BEGIN
			PRINT 'DONE'
		END
	END TRY
	BEGIN CATCH
			SET @pRtnXML = 
				(SELECT 
					'FAIL'	AS RESULT,
					ERROR_NUMBER() AS ErrorNumber
					,ERROR_SEVERITY() AS ErrorSeverity
					,ERROR_STATE() AS ErrorState
					,ERROR_PROCEDURE() AS ErrorProcedure
					,ERROR_LINE() AS ErrorLine
					,ERROR_MESSAGE() AS ErrorMessage
					FOR XML PATH('XML'), ROOT('ROOT'))	
	END CATCH

END


GO
/****** Object:  StoredProcedure [dbo].[spaSeqNo]    Script Date: 2022/6/26 下午 08:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spaSeqNo](
@co_id BIGINT,
@sq_table NVARCHAR(100),
@RtnKey	BIGINT OUTPUT)
AS
BEGIN
	IF (NOT EXISTS(SELECT * FROM  mSeqNo(NOLOCK) WHERE co_id = @co_id AND sq_table = @sq_table))
	BEGIN
		INSERT INTO mSeqNo(co_id,sq_table,sq_num)
		VALUES ( @co_id,@sq_table ,1)
		SET @RtnKey = 1
	END
	ELSE
	BEGIN
		UPDATE mSeqNo SET sq_num = sq_num +1 ,@RtnKey = sq_num +1 WHERE 
		co_id = @co_id AND sq_table = @sq_table 
	END
END

GO
USE [master]
GO
ALTER DATABASE [GenericFormBuilder] SET  READ_WRITE 
GO
