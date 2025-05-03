using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class Otp
    {
        /// <summary>
        /// gets and Sets OtpId
        /// Primary key for the Otp
        /// </summary>
        [Key]
        public Guid OtpId { get; set; }

        /// <summary>
        /// gets and Sets Email
        /// Email of User
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// gets and Sets OtpCode
        /// Stores the generated OTP Code
        /// </summary>
        public string OtpCode { get; set; }

        /// <summary>
        /// gets and Sets CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
