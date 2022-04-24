using Net6.Api.Mongo.Collections;
namespace Net6.Api.Mongo.Repositories;

public interface IPeopleRepository
{
    Task<List<People>> GetAllAsync();
    Task<People> GetByIdAsync(string id);
    Task CreateNewPeopleAsync(People newPeople);
    Task UpdatePeopleAsync(People peopleToUpdate);
    Task DeletePeopleAsync(string id);
}