using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journey.Infrastructure.Enums;

namespace Journey.Application.UseCases.Activities
{
    public class CompleteActivityForTripUseCase
    {
        public void Execute(Guid tripId, Guid activityId)
        {
            var dbContext = new JourneyDbContext();

            var activity = dbContext
                .Activities
                .FirstOrDefault(activity => activity.Id == activityId && activity.TripId == tripId);

            if (activity == null)
            {
                throw new NotFoundException("Activity not found");
            }

            activity.Status = Infrastructure.Enums.ActivityStatus.Done;
            dbContext.SaveChanges();
        }
    }
}
