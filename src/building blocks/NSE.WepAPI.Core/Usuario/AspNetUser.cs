using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace NSE.WepAPI.Core.Usuario
{
    public class AspNetUser : IAspNetUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor) => _accessor = accessor;
        
        public string Name => _accessor.HttpContext.User.Identity.Name;

        public Guid ObterUserId()
            => !EstaAutenticado() ? Guid.Empty : Guid.Parse(_accessor.HttpContext.User.GetUserId());
        
        public string ObterUserEmail()
            => !EstaAutenticado() ? string.Empty : _accessor.HttpContext.User.GetUserEmail();
        
        public string ObterUserToken()
            => !EstaAutenticado() ? string.Empty : _accessor.HttpContext.User.GetUserToken();

        public string ObterUserRefreshToken()
             => !EstaAutenticado() ? string.Empty : _accessor.HttpContext.User.GetUserRefreshToken();

        public bool EstaAutenticado()
            => _accessor.HttpContext.User.Identity.IsAuthenticated;

        public bool PossuiRole(string role)
            => _accessor.HttpContext.User.IsInRole(role);

        public HttpContext ObterHttpContext()
            => _accessor.HttpContext;
        
        public IEnumerable<Claim> ObterClaims()
            => _accessor.HttpContext.User.Claims;
    }
}
