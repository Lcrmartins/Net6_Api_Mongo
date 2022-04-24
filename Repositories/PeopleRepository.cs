using Net6.Api.Mongo.Collections;
using MongoDB.Driver;

namespace Net6.Api.Mongo.Repositories;

public class PeopleRepository:IPeopleRepository
{
    private readonly IMongoCollection<People> _peopleCollection;

    public PeopleRepository(IMongoDatabase mongoDatabase)
    {
        _peopleCollection = mongoDatabase.GetCollection<People>("people");
    }

    public async Task<List<People>> GetAllAsync()
    {
        return await _peopleCollection.Find(_ => true).ToListAsync();
    }

    public async Task<People> GetByIdAsync(string id)
    {
        return await _peopleCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateNewPeopleAsync(People newPeople)
    {
        await _peopleCollection.InsertOneAsync(newPeople);
    }

    public async Task UpdatePeopleAsync(People peopleToUpdate) 
    {
        await _peopleCollection.ReplaceOneAsync(x => x.Id == peopleToUpdate.Id, peopleToUpdate);
    }

    public async Task DeletePeopleAsync(string id)
    {
        await _peopleCollection.DeleteOneAsync(x => x.Id == id);
    }
}