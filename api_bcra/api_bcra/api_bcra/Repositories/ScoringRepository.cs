using System;
using api_bcra.Context;
using api_bcra.Models;
using api_bcra.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace api_bcra.Repositories
{
    public class ScoringRepository : IScoringRepository
    {
        private readonly BCRADbContext _context;
        public ScoringRepository(BCRADbContext context) {
            _context = context;
        }
        public async Task<Entity> GetEntity(int id)
        {
            var entity = await _context.Entities.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new Exception();
            }
            return entity;
        }

        public async Task<Entity> GetEntityByName(string name)
        {
            var entity = await _context.Entities.Where(e => e.Name.Equals(name)).FirstOrDefaultAsync();
            return entity;
        }

        public async Task<Person> GetPersonByCuit(string cuit)
        {
            var person = await _context.People.Where(e => e.Cuit.Equals(cuit)).FirstOrDefaultAsync();
            return person;
        }

        public async Task<Person> GetPersonById(int id)
        {
            var person = await _context.People.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (person == null)
            {
                throw new Exception();
            }
            return person;
        }

        public async Task<Entity> InsertNewEntity(Entity entity)
        {
            try
            {
                _context.Entities.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Person> InsertNewPerson(Person person)
        {
            try
            {
                _context.People.Add(person);
                await _context.SaveChangesAsync();
                return person;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Models.Query> InsertNewQuery(Models.Query query)
        {
            try
            {
                _context.Queries.Add(query);
                await _context.SaveChangesAsync();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Scoring> InsertNewScoring(Scoring scoring)
        {
            try
            {
                _context.Scorings.Add(scoring);
                await _context.SaveChangesAsync();
                return scoring;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
