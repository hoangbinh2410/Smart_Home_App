using BA_MobileGPS.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IFeedbackService
    {
        Task<FeedbackResponse> SaveFeedbackAsync(FeedbackRequest feedbackRequest);

        Task<List<FeedbackTypeResponse>> GetFeedbackTypeAsync(Guid UserID, string Culture, long? LastUpdate);
    }
}