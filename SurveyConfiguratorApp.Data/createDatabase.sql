IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'survey')
BEGIN
    CREATE DATABASE [survey]
END
GO

USE [survey]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Question]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Question] (
        [Id]         INT           IDENTITY (1, 1) NOT NULL,
        [Order]      INT           NOT NULL,
        [Text]       VARCHAR (1500) NOT NULL,
        [TypeNumber] INT           NOT NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [unique_order] UNIQUE NONCLUSTERED ([Order] ASC)
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QuestionFaces]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[QuestionFaces] (
        [FacesNumber] INT NOT NULL,
        [QuestionId]  INT NOT NULL,
        CONSTRAINT [FK_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [Check_Faces_Number] CHECK ([FacesNumber] >= (2) AND [FacesNumber] <= (5))
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QuestionSlider]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[QuestionSlider] (

        [StartValue]   INT           NOT NULL,
        [EndValue]     INT           NOT NULL,
        [StartCaption] VARCHAR (500) NOT NULL,
        [EndCaption]   VARCHAR (500) NOT NULL,
        [QuestionId]   INT           NOT NULL,

        CONSTRAINT [FK_QuestionId_Slider] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [Check_Slider_EndValue] CHECK ([EndValue] >= [StartValue] AND [EndValue] <= (100)),
        CONSTRAINT [Check_Slider_StartValue] CHECK ([StartValue] >= (0) AND [StartValue] < [EndValue])
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QuestionStars]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[QuestionStars] (
        [StarsNumber] INT NOT NULL,
        [QuestionId]  INT NOT NULL,
        CONSTRAINT [FK_QuestionId_Stars] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [Check_Stars_Number] CHECK ([StarsNumber] >= (1) AND [StarsNumber] <= (10))
    );
END











