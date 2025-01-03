using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {

            var username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (username == null) throw new Exception("cannot get username from token");
            return username;
        }
    


    
        public static int GetUserId(this ClaimsPrincipal user)
    {

        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Cannot use username from the token "));
        return userId;
    }
}
}
