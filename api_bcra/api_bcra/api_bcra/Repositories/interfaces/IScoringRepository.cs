using api_bcra.Models;

namespace api_bcra.Repositories.interfaces
{
    public interface IScoringRepository
    {
        Task<Entity> GetEntity(int id);
        Task<Entity> GetEntityByName(string name);
        Task<Person> GetPersonById(int id);
        Task<Person> GetPersonByCuit(string cuit);
        Task<Person> InsertNewPerson(Person person);
        Task<Entity> InsertNewEntity(Entity entity);
        Task<Query> InsertNewQuery(Query query);
        Task<Scoring> InsertNewScoring(Scoring scoring);
    }
}
