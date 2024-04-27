
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/25/2024 14:19:57
-- Generated from EDMX file: C:\Users\emir1\source\repos\Vizeproje\Vizeproje\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Universite];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[UniversiteModelStoreContainer].[Giris_Tb]', 'U') IS NOT NULL
    DROP TABLE [UniversiteModelStoreContainer].[Giris_Tb];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Giris_Tb'
CREATE TABLE [dbo].[Giris_Tb] (
    [Ad] varchar(50)  NOT NULL,
    [sifre] nchar(10)  NOT NULL,
    [kullanici] nchar(10)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Ad], [sifre], [kullanici] in table 'Giris_Tb'
ALTER TABLE [dbo].[Giris_Tb]
ADD CONSTRAINT [PK_Giris_Tb]
    PRIMARY KEY CLUSTERED ([Ad], [sifre], [kullanici] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------