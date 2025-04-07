using api_bcra.Repositories.interfaces;
using api_bcra.Services.interfaces;
using api_bcra.Services.Responses;
using api_bcra.Request.interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using api_bcra.Models;

namespace api_bcra.Services
{
    public class MainService : IMainService
    {
        private readonly IScoringRepository _scoringRepository;
        private readonly IBCRAClient _bcraclient;
        public MainService (IScoringRepository scoringRepository, IBCRAClient bcraclient)
        {
            _scoringRepository = scoringRepository;
            _bcraclient = bcraclient;
        }
        public async Task<ScoringResponse> GetDebts(string cuit, int userId)
        {
            try
            {
                var data = await _bcraclient.GetFullHistory(cuit);
                var json = JsonConvert.DeserializeObject<dynamic>(data);
                var status_code = (int)json?.status;

                var query = new Query { DateQuery = DateTime.Now, CUIT = cuit, IdUser = userId };

                await _scoringRepository.InsertNewQuery(query);

                if (status_code == 200)
                {
                    var person_id = 0;

                    var exists_person = await _scoringRepository.GetPersonByCuit(cuit);

                    if (exists_person == null)
                    {
                        var fullname = json.results?.denominacion == null ? "" : json.results?.denominacion;
                        var new_person = await _scoringRepository.InsertNewPerson(new Models.Person { Cuit = cuit, Fullname = fullname });
                        person_id = new_person.Id;
                    } else
                    {
                        person_id = exists_person.Id;
                    }

                    var periodos = (JArray)json.results.periodos;
                    var scoring_list = new List<Scoring>();

                    foreach (var periodo in periodos)
                    {
                        var periodo_date = periodo["periodo"].ToString();
                        var entidades = (JArray)periodo["entidades"];

                        if (entidades.Count > 0)
                        {
                            foreach (var entidad in entidades)
                            {
                                var new_scoring = new Scoring { };
                                new_scoring.Person = person_id;

                                var entity_name = entidad["entidad"].ToString();
                                var exists_entity = await _scoringRepository.GetEntityByName(entity_name);
                                
                                if (exists_entity == null)
                                {
                                    var new_entity = await _scoringRepository.InsertNewEntity(new Models.Entity { Name = entity_name });
                                    new_scoring.Entity = new_entity.Id;
                                } else
                                {
                                    new_scoring.Entity = exists_entity.Id;
                                }

                                var situation = (int)entidad["situacion"];
                                var amount = (int)entidad["monto"];
                                var enRevision = (bool)entidad["enRevision"];
                                var procesoJud = (bool)entidad["procesoJud"];

                                new_scoring.Period = periodo_date;
                                new_scoring.Situation = situation;
                                new_scoring.Amount = amount;
                                new_scoring.Checking = enRevision;
                                new_scoring.ProcJu = procesoJud;

                                var insert_scoring = await _scoringRepository.InsertNewScoring(new_scoring);
                                scoring_list.Add(insert_scoring);
                            }
                        }
                    }

                    return new ScoringResponse { Scoring = data, StatusCode = status_code };
                } else if (status_code == 400 ||  status_code == 404)
                {
                    return new ScoringResponse { ErrorMessage = json.errorMessages[0], StatusCode = status_code };
                } else
                {
                    return new ScoringResponse { ErrorMessage = json.errorMessages[0], StatusCode = status_code };
                }
            }
            catch (Exception ex)
            {
                return new ScoringResponse { ErrorMessage = ex.Message, Scoring = null, StatusCode = 500 };
            }
        }
    }
}
