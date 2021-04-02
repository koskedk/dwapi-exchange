ALTER TABLE [dbo].[AdverseEvent] ADD LiveRowId bigint IDENTITY(1,1) not null;
ALTER TABLE [dbo].[AdverseEvent] ADD PRIMARY KEY (LiveRowId);
GO

ALTER TABLE [dbo].[ART] ADD LiveRowId bigint IDENTITY(1,1) not null;
ALTER TABLE [dbo].[ART] ADD PRIMARY KEY (LiveRowId);
GO

ALTER TABLE [dbo].[Baselines] ADD LiveRowId bigint IDENTITY(1,1) not null;
ALTER TABLE [dbo].[Baselines] ADD PRIMARY KEY (LiveRowId);
GO

ALTER TABLE [dbo].[Labs] ADD LiveRowId bigint IDENTITY(1,1) not null;
ALTER TABLE [dbo].[Labs] ADD PRIMARY KEY (LiveRowId);
GO

ALTER TABLE [dbo].[Patients] ADD LiveRowId bigint IDENTITY(1,1) not null;
ALTER TABLE [dbo].[Patients] ADD PRIMARY KEY (LiveRowId);
ALTER TABLE [dbo].[Patients] ADD Age int null;
UPDATE [dbo].[Patients]
set Age=CASE
            WHEN dateadd(year, datediff (year, DOB, getdate()), DOB) > getdate() THEN datediff(year, DOB, getdate()) - 1
            ELSE datediff(year, DOB, getdate())
    END
GO

ALTER TABLE [dbo].[PatientStatus] ADD LiveRowId bigint IDENTITY(1,1) not null;
ALTER TABLE [dbo].[PatientStatus] ADD PRIMARY KEY (LiveRowId);
GO

ALTER TABLE [dbo].[Pharmacy] ADD LiveRowId bigint IDENTITY(1,1) not null;
ALTER TABLE [dbo].[Pharmacy] ADD PRIMARY KEY (LiveRowId);
GO

ALTER TABLE [dbo].[Visits] ADD LiveRowId bigint IDENTITY(1,1) not null;
ALTER TABLE [dbo].[Visits] ADD PRIMARY KEY (LiveRowId);
GO
