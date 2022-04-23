using MongoDB.Bson.Serialization.Attributes;

namespace Net6.Api.Mongo.Collections;

public class People
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("age")]    
    public int Age { get; set; }

    [BsonElement("phonenumbers")]
    public List<string> PhoneNumbers { get; set; }
}