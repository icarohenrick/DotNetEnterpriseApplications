﻿using System;
using System.Security.Claims;

namespace NFE.WepAPI.Core.Usuario
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentException(nameof(principal));

            //var claim = principal.FindFirst("sub");

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);

            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentException(nameof(principal));

            var claim = principal.FindFirst("email");

            //var claim = principal.FindFirst(ClaimTypes.Email);

            return claim?.Value;
        }

        public static string GetUserToken(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentException(nameof(principal));

            var claim = principal.FindFirst("JWT");

            //var claim = principal.FindFirst(ClaimTypes.);

            return claim?.Value;
        }

        public static string GetUserRefreshToken(this ClaimsPrincipal principal)
        {
            if(principal is null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst("RefreshToken");
            return claim?.Value;
        }
    }
}
