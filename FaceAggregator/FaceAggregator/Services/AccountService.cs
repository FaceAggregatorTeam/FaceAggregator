using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace FaceAggregator.Services
{
    public class AccountService : IAccountService
    {
        public string GetContainerNamePhotos(ClaimsPrincipal claimsPrincipal)
        {
            string result = GetEmailAddress(claimsPrincipal).Replace('.', '-').Replace('@', '-') + "-photos";
            return result.ToLower();
        }

        public string GetContainerNameFace(ClaimsPrincipal claimsPrincipal)
        {
            string result = GetEmailAddress(claimsPrincipal).Replace('.', '-').Replace('@', '-') + "-face";
            return result.ToLower();
        }

        public string GetContainerNameResults(ClaimsPrincipal claimsPrincipal)
        {
            string result = GetEmailAddress(claimsPrincipal).Replace('.', '-').Replace('@', '-') + "-results";
            return result.ToLower();
        }

        private string GetEmailAddress(ClaimsPrincipal claim)
        {
            var lol = claim.Claims.FirstOrDefault(e => e.Type.Contains("emailaddress"));
            if (lol != null)
                return lol.Value;
            return "";
        }

    }
}