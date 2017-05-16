using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace FaceAggregator.Services
{
    public interface IAccountService
    {
        string GetContainerNamePhotos(ClaimsPrincipal claimsPrincipal);
        string GetContainerNameFace(ClaimsPrincipal claimsPrincipal);
        string GetContainerNameResults(ClaimsPrincipal claimsPrincipal);
    }
}