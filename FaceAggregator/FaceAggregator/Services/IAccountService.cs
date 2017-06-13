using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace FaceAggregator.Services
{
    /// <summary>
    /// Interafce for Accounts
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Gets the container name for photos.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns></returns>
        string GetContainerNamePhotos(ClaimsPrincipal claimsPrincipal);
        /// <summary>
        /// Gets the container name for face.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns></returns>
        string GetContainerNameFace(ClaimsPrincipal claimsPrincipal);
        /// <summary>
        /// Gets the container name for results.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns></returns>
        string GetContainerNameResults(ClaimsPrincipal claimsPrincipal);
    }
}