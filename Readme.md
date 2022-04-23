# .NET6 Web API CRUD Operation With MongoDB

In this article, we are going to implement .NET6 Web API CRUD operation using MongoDB as database.

## MongoDB:

MongoDB is a source-available cross-platform document-oriented database. It is also called a NoSQL database or Non-Relational database.

In MongoDB 'Collection' is equivalent to the Table in SQL database.

In MongoDB data is stored as 'Document' that is equivalent to a table record in an SQL database. The 'Document' contains JSON data which will be stored in BSON(Binary JSON) format.

## Create A .NET6 Web API Application:

Let's create a .Net6 Web API sample application to accomplish our demo. We can use either Visual Studio 2022 or Visual Studio Code(using .NET CLI commands) to create any.Net6 application. For this demo, I'm using the 'Visual Studio Code'(using the .NET CLI command) editor.

```CLI command
dotnet new webapi -o Your_Project_Name
```

## MongoDB Docker Image:

In this demo, I will consume the MongoDB that's run as a Docker container.

## Install MongoDB NuGet:

```.NET CLI Command
dotnet add package MongoDB.Driver --version 2.14.1
```

## Add MongoDB Configurations:

Let's configure MongoDB settings like 'connectionstring', 'databasename', etc into our API project. In the 'appSettings.Development.json' file add MongoDB settings.

_appSettings.Development.json:_

```appSettings.Development.json:
"MongoDBSettings":{
  "ConnectionString":"mongodb://localhost:8007",
  "DatabaseName":"myworld"
}
```

