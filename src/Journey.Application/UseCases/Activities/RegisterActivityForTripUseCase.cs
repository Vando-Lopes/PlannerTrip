using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Infrastructure.Entities;
using Journey.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Journey.Exception.ExceptionsBase;
using Journey.Application.UseCases.Validators;
using FluentValidation.Results;
using Journey.Communication.Enums;

namespace Journey.Application.UseCases.Activities
{
    public class RegisterActivityForTripUseCase
    {
        public ResponseActivityJson Execute(Guid tripId, RequestRegisterActivityJson requestRegisterActivityJson)
        {
            var dbContext = new JourneyDbContext();

            var trip = dbContext.Trips
                .FirstOrDefault(trip => trip.Id == tripId);
            
            Validate(trip!, requestRegisterActivityJson);

            var entity = new Activity()
            {
                TripId = tripId,
                Name = requestRegisterActivityJson.Name,
                Date = requestRegisterActivityJson.Date,
            };

            dbContext.Activities.Add(entity);
            dbContext.SaveChanges();
            return new ResponseActivityJson()
            {
                Name = requestRegisterActivityJson.Name,
                Date = requestRegisterActivityJson.Date,
                Id = entity.Id,
                Status = (ActivityStatus)entity.Status
            };
        }

        private void Validate(Trip trip, RequestRegisterActivityJson requestRegisterActivityJson)
        {
            var validator = new RegisterActiviryValidator();
            var result = validator.Validate(requestRegisterActivityJson);

            if (trip == null)
            {
                throw new NotFoundException("Trip not found");
            }

            if (!(requestRegisterActivityJson.Date >= trip.StartDate && requestRegisterActivityJson.Date <= trip.EndDate))
            {
                result.Errors.Add(new ValidationFailure("Date", "Data da atividade deve está dentro da data da viagem."));
            }

            if (!result.IsValid) 
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
