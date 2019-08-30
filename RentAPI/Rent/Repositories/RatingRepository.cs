using Rent.Data;
using Rent.Helpers;
using Rent.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Repositories
{
    public class RatingRepository
    {
        RentContext _context;


        public RatingRepository(RentContext context)
        {
            _context = context;

        }

        public async Task<Rating> CreateRating(string title)
        {
            var rating = new Rating { Title = title };
            _context.Rating.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<RatingItem> CreateRatingItem(int ratingID, RatingItem item) 
        {
            item.RatingID = ratingID;
            _context.RatingItem.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }







        /*
        public async Task<ICollection<Rating>> GetRatingsForLocation(int locationID)
        {
            var location = _context.Location.Find(locationID);
            if (location == null)
                return null;

            var ratings = _context.QualityReport.Include(q => q.Rating).Where(q => q.LocationID == locationID && q.RatingID != null).Select(q => q.Rating);

            return ratings.ToList();
        }

        public async Task<bool> AddRatingToQualityReport(int qualityReportID, Rating rating)
        {
            var qualityReport = _context.QualityReport.Find(qualityReportID);

            if(qualityReport == null || qualityReport.RatingID != null)
            {
                return false;
            }

            _context.Rating.Add(rating);
            await _context.SaveChangesAsync();

            qualityReport.RatingID = rating.ID;
            _context.QualityReport.Update(qualityReport);
            await _context.SaveChangesAsync();

            return true;
        }*/

    }
}
