using Journey.Application.UseCases.Validators;
using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Journey.Infrastructure.Entities;

namespace Journey.Application.UseCases.Trips
{
    public class RegisterTripUseCase
    {
        public ResponseShortTripJson Execute(RequestRegisterTripJson requestRegisterTripJson)
        {
            Validate(requestRegisterTripJson);

            var dbContext = new JourneyDbContext();
            var entity = new Trip()
            {
                Name = requestRegisterTripJson.Name,
                StartDate = requestRegisterTripJson.StartDate,
                EndDate = requestRegisterTripJson.EndDate,
            };

            dbContext.Add(entity);
            dbContext.SaveChanges();

            return new ResponseShortTripJson
            {
                EndDate = entity.EndDate,
                StartDate = entity.StartDate,
                Name = entity.Name,
                Id = entity.Id
            };
        }

        private void Validate(RequestRegisterTripJson requestRegisterTripJson)
        {
            var validator = new RegisterTripValidator();
            var result = validator.Validate(requestRegisterTripJson);

            if (!result.IsValid) 
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
