using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class FeedbackRequest
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public int XNCode { get; set; }

        public string VehiclePlace { get; set; }

        public string PhoneNumber { get; set; }

        public List<FeedbackTypeViewModel> FeedbackTypes { get; set; }

        public string Comment { get; set; }
        public bool IsPutToBAP { get; set; }
        public bool IsStatusPutToBAP { get; set; }

        /// <summary>
        /// Số lượng max feedback trong ngày
        /// </summary>
        /// <value>
        /// The maximum feedback.
        /// </value>
        /// Name     Date         Comments
        /// TruongPV  3/5/2019   created
        /// </Modified>
        public int MaxFeedback { get; set; }
    }
}