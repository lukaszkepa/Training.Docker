use master
go
if not exists(select * from sys.databases where name = 'DockerCourseDb')
begin
    CREATE DATABASE [DockerCourseDb];-- ON  PRIMARY 
        --( NAME = N'DockerCourseDb', FILENAME = N'/folder_for_dbs/DockerCourseDb.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB ) 
        --LOG ON 
        --( NAME = N'DockerCourseDb_log', FILENAME = N'/folder_for_dbs/DockerCourseDb_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
        --COLLATE Polish_CI_AS;
end
go
use DockerCourseDb
go
if not exists(select * from information_schema.tables where table_name = 'OrdersAggregatedData')
begin
    CREATE TABLE [OrdersAggregatedData] (
        Id int IDENTITY(1, 1) NOT NULL,
        CustomerName varchar(1000) NOT NULL,
        OrderPlacementDate datetime2 NOT NULL,
        TotalPrice money NOT NULL
    );
    ALTER TABLE [OrdersAggregatedData] ADD CONSTRAINT OrdersAggregatedData_PK PRIMARY KEY (Id);
end
go


