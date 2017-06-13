using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace FaceAggregator.Services
{
    /// <summary>
    /// Service for Accounts
    /// </summary>
    /// <seealso cref="FaceAggregator.Services.IAccountService" />
    public class AccountService : IAccountService
    {
        /// <summary>
        /// Gets the container name for photos.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns></returns>
        public string GetContainerNamePhotos(ClaimsPrincipal claimsPrincipal)
        {
            string result = GetEmailAddress(claimsPrincipal).Replace('.', '-').Replace('@', '-') + "-photos";
            return result.ToLower();
        }

        /// <summary>
        /// Gets the container for name face.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns></returns>
        public string GetContainerNameFace(ClaimsPrincipal claimsPrincipal)
        {
            string result = GetEmailAddress(claimsPrincipal).Replace('.', '-').Replace('@', '-') + "-face";
            return result.ToLower();
        }

        /// <summary>
        /// Gets the container name for results.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns></returns>
        public string GetContainerNameResults(ClaimsPrincipal claimsPrincipal)
        {
            string result = GetEmailAddress(claimsPrincipal).Replace('.', '-').Replace('@', '-') + "-results";
            return result.ToLower();
        }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        private string GetEmailAddress(ClaimsPrincipal claim)
        {
            var lol = claim.Claims.FirstOrDefault(e => e.Type.Contains("emailaddress"));
            if (lol != null)
                return lol.Value;
            return "";
        }

    }
}