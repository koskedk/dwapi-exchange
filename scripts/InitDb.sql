ALTER TABLE [dbo].[AdverseEvent] ADD LiveRowId bigint IDENTITY(1,1) not null
Go
ALTER TABLE [dbo].[ART] ADD LiveRowId bigint IDENTITY(1,1) not null
Go
ALTER TABLE [dbo].[Baselines] ADD LiveRowId bigint IDENTITY(1,1) not null
Go
ALTER TABLE [dbo].[Labs] ADD LiveRowId bigint IDENTITY(1,1) not null
Go
ALTER TABLE [dbo].[Patients] ADD LiveRowId bigint IDENTITY(1,1) not null
ALTER TABLE [dbo].[Patients] ADD Age int  null
Go
ALTER TABLE [dbo].[PatientStatus] ADD LiveRowId bigint IDENTITY(1,1) not null
Go
ALTER TABLE [dbo].[Pharmacy] ADD LiveRowId bigint IDENTITY(1,1) not null
Go
ALTER TABLE [dbo].[Visits] ADD LiveRowId bigint IDENTITY(1,1) not null
Go

Update Patients 
set Age=CASE 
			WHEN dateadd(year, datediff (year, DOB, getdate()), DOB) > getdate() THEN datediff(year, DOB, getdate()) - 1
            ELSE datediff(year, DOB, getdate())
		END
