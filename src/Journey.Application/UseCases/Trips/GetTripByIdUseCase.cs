﻿using Journey.Communication.Responses;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Application.UseCases.Trips
{
    public class GetTripByIdUseCase
    {
        public ResponseTripJson Execute(Guid id)
        {
            var dbContext = new JourneyDbContext();
            var trip = dbContext.Trips
                .Include(trip => trip.Activities)
                .FirstOrDefault(trip => trip.Id == id);

            if (trip == null) 
            {
                throw new NotFoundException("Trip not found");
            }

            return new ResponseTripJson
            {
                Id = trip.Id,
                Name = trip.Name,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                Activities = trip.Activities.Select(activity => new ResponseActivityJson
                {
                    Id = activity.Id,
                    Name = activity.Name,
                    Date = activity.Date,
                    Status = (Communication.Enums.ActivityStatus)activity.Status
                }).ToList()
            };
        }
    }
}