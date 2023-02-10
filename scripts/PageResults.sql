CREATE PROCEDURE dbo.uspSelectPagedAdverseEvent
  @PageNumber INT = 1,
  @PageSize   INT = 100
AS
BEGIN
  SET NOCOUNT ON;

  ;WITH pg AS
  (
    SELECT LiveRowId
    FROM dbo.AdverseEvent
    ORDER BY LiveRowId
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY
  )
  SELECT c.*
  FROM dbo.AdverseEvent AS c
  WHERE EXISTS (SELECT 1 FROM pg WHERE pg.LiveRowId = c.LiveRowId)
  ORDER BY c.LiveRowId OPTION (RECOMPILE);
END
GO

CREATE PROCEDURE dbo.uspSelectPagedART
  @PageNumber INT = 1,
  @PageSize   INT = 100
AS
BEGIN
  SET NOCOUNT ON;

  ;WITH pg AS
  (
    SELECT LiveRowId
    FROM dbo.ART
    ORDER BY LiveRowId
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY
  )
  SELECT c.*
  FROM dbo.ART AS c
  WHERE EXISTS (SELECT 1 FROM pg WHERE pg.LiveRowId = c.LiveRowId)
  ORDER BY c.LiveRowId OPTION (RECOMPILE);
END
GO

CREATE PROCEDURE dbo.uspSelectPagedBaselines
  @PageNumber INT = 1,
  @PageSize   INT = 100
AS
BEGIN
  SET NOCOUNT ON;

  ;WITH pg AS
  (
    SELECT LiveRowId
    FROM dbo.Baselines
    ORDER BY LiveRowId
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY
  )
  SELECT c.*
  FROM dbo.Baselines AS c
  WHERE EXISTS (SELECT 1 FROM pg WHERE pg.LiveRowId = c.LiveRowId)
  ORDER BY c.LiveRowId OPTION (RECOMPILE);
END
GO

CREATE PROCEDURE dbo.uspSelectPagedLabs
  @PageNumber INT = 1,
  @PageSize   INT = 100
AS
BEGIN
  SET NOCOUNT ON;

  ;WITH pg AS
  (
    SELECT LiveRowId
    FROM dbo.Labs
    ORDER BY LiveRowId
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY
  )
  SELECT c.*
  FROM dbo.Labs AS c
  WHERE EXISTS (SELECT 1 FROM pg WHERE pg.LiveRowId = c.LiveRowId)
  ORDER BY c.LiveRowId OPTION (RECOMPILE);
END
GO

CREATE PROCEDURE dbo.uspSelectPagedVisits
  @PageNumber INT = 1,
  @PageSize   INT = 100
AS
BEGIN
  SET NOCOUNT ON;

  ;WITH pg AS
  (
    SELECT LiveRowId
    FROM dbo.Visits
    ORDER BY LiveRowId
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY
  )
  SELECT c.*
  FROM dbo.Visits AS c
  WHERE EXISTS (SELECT 1 FROM pg WHERE pg.LiveRowId = c.LiveRowId)
  ORDER BY c.LiveRowId OPTION (RECOMPILE);
END
GO

CREATE PROCEDURE dbo.uspSelectPagedPatients
  @PageNumber INT = 1,
  @PageSize   INT = 100
AS
BEGIN
  SET NOCOUNT ON;

  ;WITH pg AS
  (
    SELECT LiveRowId
    FROM dbo.Patients
    ORDER BY LiveRowId
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY
  )
  SELECT c.*
  FROM dbo.Patients AS c
  WHERE EXISTS (SELECT 1 FROM pg WHERE pg.LiveRowId = c.LiveRowId)
  ORDER BY c.LiveRowId OPTION (RECOMPILE);
END
GO

CREATE PROCEDURE dbo.uspSelectPagedPatientStatus
  @PageNumber INT = 1,
  @PageSize   INT = 100
AS
BEGIN
  SET NOCOUNT ON;

  ;WITH pg AS
  (
    SELECT LiveRowId
    FROM dbo.PatientStatus
    ORDER BY LiveRowId
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY
  )
  SELECT c.*
  FROM dbo.PatientStatus AS c
  WHERE EXISTS (SELECT 1 FROM pg WHERE pg.LiveRowId = c.LiveRowId)
  ORDER BY c.LiveRowId OPTION (RECOMPILE);
END
GO

CREATE PROCEDURE dbo.uspSelectPagedPharmacy
  @PageNumber INT = 1,
  @PageSize   INT = 100
AS
BEGIN
  SET NOCOUNT ON;

  ;WITH pg AS
  (
    SELECT LiveRowId
    FROM dbo.Pharmacy
    ORDER BY LiveRowId
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY
  )
  SELECT c.*
  FROM dbo.Pharmacy AS c
  WHERE EXISTS (SELECT 1 FROM pg WHERE pg.LiveRowId = c.LiveRowId)
  ORDER BY c.LiveRowId OPTION (RECOMPILE);
END
GO

CREATE PROCEDURE dbo.uspSelectPagedVisits
  @PageNumber INT = 1,
  @PageSize   INT = 100
AS
BEGIN
  SET NOCOUNT ON;

  ;WITH pg AS
  (
    SELECT LiveRowId
    FROM dbo.Visits
    ORDER BY LiveRowId
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY
  )
  SELECT c.*
  FROM dbo.Visits AS c
  WHERE EXISTS (SELECT 1 FROM pg WHERE pg.LiveRowId = c.LiveRowId)
  ORDER BY c.LiveRowId OPTION (RECOMPILE);
END
GO
