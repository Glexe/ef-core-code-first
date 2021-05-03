IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Doctors] (
    [DoctorID] int NOT NULL IDENTITY,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Doctors] PRIMARY KEY ([DoctorID])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210501080337_AddedDoctorTable', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Patients] (
    [PatientID] int NOT NULL IDENTITY,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [BirthDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Patients] PRIMARY KEY ([PatientID])
);
GO

CREATE TABLE [Prescriptions] (
    [PrescriptionID] int NOT NULL IDENTITY,
    [Date] datetime2 NOT NULL,
    [DueDate] datetime2 NOT NULL,
    [IdPatient] int NOT NULL,
    [IdDoctor] int NOT NULL,
    [IdPatientNavigationPatientID] int NULL,
    [IdDoctorNavigationDoctorID] int NULL,
    CONSTRAINT [PK_Prescriptions] PRIMARY KEY ([PrescriptionID]),
    CONSTRAINT [FK_Prescriptions_Doctors_IdDoctorNavigationDoctorID] FOREIGN KEY ([IdDoctorNavigationDoctorID]) REFERENCES [Doctors] ([DoctorID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescriptions_Patients_IdPatientNavigationPatientID] FOREIGN KEY ([IdPatientNavigationPatientID]) REFERENCES [Patients] ([PatientID]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Prescriptions_IdDoctorNavigationDoctorID] ON [Prescriptions] ([IdDoctorNavigationDoctorID]);
GO

CREATE INDEX [IX_Prescriptions_IdPatientNavigationPatientID] ON [Prescriptions] ([IdPatientNavigationPatientID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210501081545_AddedPatientAndPrescriptionTables', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Prescriptions] DROP CONSTRAINT [FK_Prescriptions_Doctors_IdDoctorNavigationDoctorID];
GO

ALTER TABLE [Prescriptions] DROP CONSTRAINT [FK_Prescriptions_Patients_IdPatientNavigationPatientID];
GO

DROP INDEX [IX_Prescriptions_IdDoctorNavigationDoctorID] ON [Prescriptions];
GO

DROP INDEX [IX_Prescriptions_IdPatientNavigationPatientID] ON [Prescriptions];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Prescriptions]') AND [c].[name] = N'IdDoctorNavigationDoctorID');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Prescriptions] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Prescriptions] DROP COLUMN [IdDoctorNavigationDoctorID];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Prescriptions]') AND [c].[name] = N'IdPatientNavigationPatientID');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Prescriptions] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Prescriptions] DROP COLUMN [IdPatientNavigationPatientID];
GO

CREATE TABLE [Medicaments] (
    [MedicamentID] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(100) NOT NULL,
    [Type] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Medicaments] PRIMARY KEY ([MedicamentID])
);
GO

CREATE TABLE [Prescription_Medicaments] (
    [IdMedicament] int NOT NULL,
    [IdPrescription] int NOT NULL,
    [Dose] int NULL,
    [Details] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Prescription_Medicaments] PRIMARY KEY ([IdMedicament], [IdPrescription]),
    CONSTRAINT [FK_Prescription_Medicaments_Medicaments_IdMedicament] FOREIGN KEY ([IdMedicament]) REFERENCES [Medicaments] ([MedicamentID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Prescription_Medicaments_Prescriptions_IdPrescription] FOREIGN KEY ([IdPrescription]) REFERENCES [Prescriptions] ([PrescriptionID]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Prescriptions_IdDoctor] ON [Prescriptions] ([IdDoctor]);
GO

CREATE INDEX [IX_Prescriptions_IdPatient] ON [Prescriptions] ([IdPatient]);
GO

CREATE INDEX [IX_Prescription_Medicaments_IdPrescription] ON [Prescription_Medicaments] ([IdPrescription]);
GO

ALTER TABLE [Prescriptions] ADD CONSTRAINT [FK_Prescriptions_Doctors_IdDoctor] FOREIGN KEY ([IdDoctor]) REFERENCES [Doctors] ([DoctorID]) ON DELETE CASCADE;
GO

ALTER TABLE [Prescriptions] ADD CONSTRAINT [FK_Prescriptions_Patients_IdPatient] FOREIGN KEY ([IdPatient]) REFERENCES [Patients] ([PatientID]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210501085148_FinishedDatabaseModelling', N'5.0.5');
GO

COMMIT;
GO

