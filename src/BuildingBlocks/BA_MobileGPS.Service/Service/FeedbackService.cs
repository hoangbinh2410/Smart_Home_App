using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IRequestProvider _IRequestProvider;

        public FeedbackService(IRequestProvider IRequestProvider)
        {
            this._IRequestProvider = IRequestProvider;
        }

        public async Task<FeedbackResponse> SaveFeedbackAsync(FeedbackRequest feedbackRequest)
        {
            FeedbackResponse result = FeedbackResponse.Success;
            //try
            //{
            //    result = await _IRequestProvider.PostAsync<FeedbackRequest, FeedbackResponse>(ApiUri.POST_SAVE_FEEDBACK, feedbackRequest);
            //}
            //catch (Exception e)
            //{
            //    Logger.WriteError(MethodInfo.GetCurrentMethod().Name, e);
            //}
            return result;
        }

        public async Task<List<FeedbackTypeResponse>> GetFeedbackTypeAsync(Guid UserID, string Culture, long? LastUpdate)
        {
            List<FeedbackTypeResponse> result = new List<FeedbackTypeResponse>();
            //try
            //{
            //    string url = $"{ApiUri.GET_FEEDBACK_TYPE}?culture={Culture}&LastUpdate={LastUpdate}&UserID={UserID}";
            //    var menu = await _IRequestProvider.GetAsync<List<FeedbackTypeResponse>>(url);

            //    if (menu != null)
            //    {
            //        result = menu;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            //}
            return result;
        }
    }
}