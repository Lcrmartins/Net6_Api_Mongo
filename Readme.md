# .NET6 Web API CRUD Operation With MongoDB

Thank's to [Naveen Bommidi](https://www.blogger.com/profile/05470018898693148623), hose article I transcripted here from https://www.learmoreseekmore.com/2022/03/dot6-web-api-crud-operation-with-mongodb.html in 2022-04-23.

In this article, we are going to implement .NET6 Web API CRUD operation using MongoDB as database.

## MongoDB:

MongoDB is a source-available cross-platform document-oriented database. It is also called a NoSQL database or Non-Relational database.

In MongoDB `Collection` is equivalent to the Table in SQL database.

In MongoDB data is stored as `Document` that is equivalent to a table record in an SQL database. The `Document` contains JSON data which will be stored in BSON(Binary JSON) format.

## Create A .NET6 Web API Application:

Let's create a .Net6 Web API sample application to accomplish our demo. We can use either Visual Studio 2022 or Visual Studio Code(using .NET CLI commands) to create any.Net6 application. For this demo, I'm using the `Visual Studio Code`(using the .NET CLI command) editor.

```PowerShell
dotnet new webapi -o Your_Project_Name
```

## MongoDB Docker Image:

In this demo, I will consume the MongoDB that's run as a Docker container.

## Install MongoDB NuGet:

```PowerShell
dotnet add package MongoDB.Driver --version 2.14.1
```

## Add MongoDB Configurations:

Let's configure MongoDB settings like `connectionstring`, `databasename`, etc into our API project. In the `appSettings.Development.json` file add MongoDB settings.

_appSettings.Development.json:_

```JSON
"MongoDBSettings":{
  "ConnectionString":"mongodb://localhost:8007",
  "DatabaseName":"myworld"
}
```

. Here connection string to dockerized MongoDB is `mongodb://localhost:8007`.

. Here my database name is `myworld`.

Let's create an entity for the above MongoDB settings like `MogoDBSettngs.cs` inside of the `Models` folder.

_Models/MongoDBSettings.cs:_

```C#
namespace Dot6.MongoDb.API.CRUD.Models;
 
public class MongoDBSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
```
Now register the `MongoDBSettings` instance by mapping the MongoDB json settings to it in the `Program.cs`

_Program.cs:_

```C#
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings")
);
```

## Register IMongoDatabase:

Now let's register the `MongoDB.Driver.IMongoDatabase` in the `Program.cs`, so that we can inject the `MongoDB.Driver.IMongoDatabase` into our application where ever we need it.

_Program.cs:_

```C#
builder.Services.AddSingleton<IMongoDatabase>(options => {
    var settings =  builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
    var client = new MongoClient(settings.ConnectionString);
    return client.GetDatabase(settings.DatabaseName);
});
```
. (Line: 1) The `MongoDB.Driver.IMongoDatabase` is registered as singleton instance.

. (Line: 2) Fetching the `MongoDBSettings`.

. (Line: 3) Initialized the `MongoDB.Driver.MongoClient` by passing the connection string as an input value.

. (Line: 4) Creating the instance for `IMongoDatabase` using the `MongClient.GetDatabase()` method that takes the database name as the input parameter.

## Create Entity For MongoDB Collection:

Now let's create an entity or class for the MongoDB collection. For this demo, I have created a collection(equivalent to the table of SQL) like `people` in MongoDB. So let's create an entity for the `people` collection like `People.cs` in the `Collection` folder.

_Collection/People.cs:_

```C#
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
 
namespace Dot6.MongoDb.API.CRUD.Collections;
 
public class People
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
 
    [BsonElement("name")]
    public string Name { get; set; }
 
    [BsonElement("age")]
    public int Age{get;set;}
 
    [BsonElement("phonenumbers")]
    public List<string> PhoneNumbers{get;set;}
}
```
. (Line: 8) The `BsonId` attribute decorated on the property makes as the primary key for the document in the MongoDB.

. (Line: 9) The `BsonRepresentation(BsonType.ObjectId)` attribute decorated on the property allow the parameter as type of string instead of the `ObjectId`(in the MongoDB document). It Handles the conversion from string to ObjectId for MongoDB.

. The `BsonElement` attribute is to map the MongoDB property to entity property for reading the data.

## Create A Repository For A MongoDB Collection:

Here we are going to use a repository pattern for communicating with MongoDB. For this demo, I have created a collection(equivalent to the table of SQL) like `people` in MongoDB. So let's create a repository file for the `people` collection like `PeopleRepository.cs` which contains all the logic communicating with MongoDB.

_Repository/IPeopleRepository.cs:_

```C#
using Dot6.MongoDb.API.CRUD.Collections;
namespace Dot6.MongoDb.API.CRUD.Repository;
 
public interface IPeopleRepository
{
    
}
```

_Repository/PeopleRepository.cs:_

```C#
using Dot6.MongoDb.API.CRUD.Collections;
using MongoDB.Driver;
 
namespace Dot6.MongoDb.API.CRUD.Repository;
 
public class PeopleRepository:IPeopleRepository
{
    private readonly IMongoCollection<People> _peopleCollection;
 
    public PeopleRepository(IMongoDatabase mongoDatabase)
    {
        _peopleCollection = mongoDatabase.GetCollection<People>("people");
    }
}
```

. (Line: 8) Declare a variable `_peopleCollection` of type `MongoDB.Driver.IMongoCollection<People>`, this will give control over the `people` collection in the MongoDB.

. (Line: 9) Injected the `MongoDB.Driver.IMongoDatabase`.

. (Line: 12) Creating the instance for `IMongoCollection` from the `IMongoDatabase.GetCollection<People>()` method where we pass the collection name as the input parameter.

Now register our repository in the `Program.cs` file.

_Program.cs:_

```C#
builder.Services.AddSingleton<IPeopleRepository, PeopleRepository>();
```

## Create A API Controller:

Let's create an API controller like `PeopleController.cs`

_Controllers/PeopleController.cs:_

```C#
using Dot6.MongoDb.API.CRUD.Collections;
using Dot6.MongoDb.API.CRUD.Repository;
using Microsoft.AspNetCore.Mvc;
 
namespace Dot6.MongoDb.API.CRUD.Controllers;
 
[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    private readonly IPeopleRepository _ipeopleRepository;
    public PeopleController(IPeopleRepository ipeopleRepository)
    {
        _ipeopleRepository = ipeopleRepository;
    }
}
```

. Here `IPeopleRepository` injected into our controller.

## Read Operation:

Let's implement an API action method that will fetch all the documents from MongoDB.

Let's add a method definition for fetching all documents into the `IPeopleRepository.cs`.

_Repository/IPeopleRepository.cs:_
```C#
Task<List<People>> GetAllAsync();
```

Now let's implement the `GetAllAsync()` method into the `PeopleRepository.cs`

_PeopleRepository.cs:_
```C#
public async Task<List<People>> GetAllAsync()
{
	return await _peopleCollection.Find(_ => true).ToListAsync();
}
```
. Here `MongoDB.Driver.Find()` method filters the documents from the collections. If we pass `true` as an input parameter it will fetch all the documents from the database.

Let's create our action method to fetch all the documents of MongoDB.

_Controllers/PeopleController.cs:_

```C#
[HttpGet]
public async Task<IActionResult> Get()
{
	var people = await _ipeopleRepository.GetAllAsync();
	return Ok(people);
}
```

## Create Operation:

Let's implement the API action method to create a new document into the collection of MongoDB. Here along with the create action method, we will add one more additional action method that is get action method by `Id` value that helps to fetch the newly created document.

Let's define method definitions for creating the document and get the document by `id`.

_Repository/IPeopleRepository.cs:_

```C#
Task<People> GetByIdAsync(string id);
Task CreateNewPeopleAsync(People newPeople);
```

Now let's implement `GetByIdAsync()`, `CreateNewPeopleAsync()` method in `PeopleRepository.cs`.

_Repository/PeopleRepository.cs:_

```C#
public async Task<People> GetByIdAsync(string id)
{
	return await _peopleCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
}
 
public async Task CreateNewPeopleAsync(People newPeople)
{
	await _peopleCollection.InsertOneAsync(newPeople);
}
```

. (Line: 3) The `MongoDB.Driver.Find()` method fetches the document that matched with the `id` value.

. (Line: 8) The `MongoDB.Driver.InsertOneAsync()` method insert the new document into the collection.

Let's add our create and get by id action methods into our controller.

_Controller/PeopleController.cs:_

```C#
[HttpGet]
[Route("{id}")]
public async Task<IActionResult> Get(string id)
{
	var people = await _ipeopleRepository.GetByIdAsync(id);
	if (people == null)
	{
		return NotFound();
	}
 
	return Ok(people);
}
 
[HttpPost]
public async Task<IActionResult> Post(People newPeople)
{
	await _ipeopleRepository.CreateNewPeopleAsync(newPeople);
	return CreatedAtAction(nameof(Get), new { id = newPeople.Id }, newPeople);
}
```

. (Line: 3-12) Action method return the document by `id` value.

. (Line: 14-19) Insert the new document into the collection

. (Line: 18) The `CreatedAction` method returns the status code of 201(created), it also frames the endpoint for fetching documents by id and returns in the response header.

## Update Operation:

Let's implement an action method to update the existing document in the collection.

Let's define the method definition for updating a document in `IPeopleRepository.cs`

_Repository/IPeopleRepository:_

```C#
Task UpdatePeopleAsync(People peopleToUpdate);
```

Now let's implement `UpdatePeopleAsync()` method in the `PeopleRepository.cs`.

_Repository/PeopleRepository:_

```C#
public async Task UpdatePeopleAsync(People peopleToUpdate)
{
	await _peopleCollection.ReplaceOneAsync(x => x.Id == peopleToUpdate.Id, peopleToUpdate);
}
```

. Here `MongoDB.Driver.ReplacOneAsync()` method updates the document of the collection. The first input parameter finds the document, the second input parameter is the latest changes to the document that needs to be saved.

Add a new action method to our controller.

_Controllers/PeopleController.cs:_

```C#
[HttpPut]
public async Task<IActionResult> Put(People updatePeople)
{
	var people = await _ipeopleRepository.GetByIdAsync(updatePeople.Id);
	if (people == null)
	{
		return NotFound();
	}
 
	await _ipeopleRepository.UpdatePeopleAsync(updatePeople);
	return NoContent();
}
```

. (Line: 4-8) Checking the document we are trying to update is valid or not. If not valid we return a response like `NotFound`.

. (Line: 10) Updating the document into the collection.

## Delete Operation:

Let's implement the API action method that deletes the document from the collection of MongoDB.

Let's define the delete method definition in the `IPeopleRepository`

_Repository/IPeopleRepository:_

```C#
Task DeletePeopleAsync(string id);
Let's implement the delete method in the `PeopleRepository`.
```

_Repository/PeopleRepository:_

```C#
public async Task DeletePeopleAsync(string id)
{
	await _peopleCollection.DeleteOneAsync(x => x.Id == id);
}
```

. Here `MongoDB.Driver.DeleteOneAsync` method deletes the specified document from the collection.

Add a new action method into our controller.

_Controllers/PeopleController.cs:_

```C#
[HttpDelete]
public async Task<IActionResult> Delete(string id)
{
	var people = await _ipeopleRepository.GetByIdAsync(id);
	if (people == null)
	{
		return NotFound();
	}
 
	await _ipeopleRepository.DeletePeopleAsync(id);
	return NoContent();
}
```

. (Line: 4-8) Checks the document that going to be deleted from the collection exists or not. If not exist then returns response as `NotFound()`.

. (Line: 10) Deletes the document from the collection.

## Notes by me:

I altered the response codes `NoContent()` by the response code `Ok("response")`. The motivation is that using `Ok()`, it's possible to insert response messages. 

LABELS: ASP.NET CORE  ASP.NET CORE WEB API  ASP.NETCORE6  DOTNET6  MONGODB  WEB API
