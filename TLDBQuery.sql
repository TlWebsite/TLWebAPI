CREATE DATABASE TLWebSiteDB
use TLWebSiteDB

Create Table[ImageType](
[PKID] int IDENTITY(1,1) NOT NULL,
[ImageTypeName] varchar(100),
CONSTRAINT PK_ImageType Primary Key Clustered (PKID)
)

Create Table[Image](
[PKID] int IDENTITY(1,1) NOT NULL,
[ImageName] varchar(45),
[CreatedDate] Datetime default(GETDATE()),
[ImageID] varchar(100),
[ImageTypeID] int,
[ImageURL] varchar(max),
CONSTRAINT PK_Image Primary Key Clustered (PKID),
CONSTRAINT FK_ImageTypeID Foreign Key (ImageTypeID) REFERENCES [ImageType](PKID)
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,
CONSTRAINT UQ_ExamTemplateID Unique(ImageID)
)
Alter Table [image] add [ImageDescription] varchar(max)
Alter Table [ImageType] Add Constraint [UQ_ImageTypeName] Unique (ImageTypeName)

Create Table[ImageTag](
[PKID] int IDENTITY(1,1) NOT NULL,
[ImageTagName] varchar(100),
CONSTRAINT PK_ImageTag Primary Key Clustered (PKID),
Constraint UQ_ImageTagName Unique(ImageTagName)
)

Create Table [Image_ImageTag](
[ImagePKID] int,
[ImageTagPKID] int,
CONSTRAINT PK_Image_ImageTag Primary Key Clustered (ImagePKID,ImageTagPKID),
CONSTRAINT FK_ImagePKID Foreign Key (ImagePKID) REFERENCES [Image](PKID)
    ON DELETE CASCADE    
    ON UPDATE CASCADE ,
CONSTRAINT FK_ImageTagID Foreign Key (ImageTagPKID) REFERENCES [ImageTag](PKID)
	ON DELETE CASCADE    
	ON UPDATE CASCADE ,
)