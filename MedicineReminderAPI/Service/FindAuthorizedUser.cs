using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MedicineReminderAPI.Models;

namespace MedicineReminderAPI.Service
{
    public interface IFindAuthorizedUser
    {
        User? AuthorizedUser(HttpContext authService, AppApiContext context);
    }

    public class FindAuthorizedUser: IFindAuthorizedUser
    {     
        public User? AuthorizedUser(HttpContext authService, AppApiContext context)
        {                 
            var claim = authService.User.FindFirst("id");
            if (claim == null) return null;
            User user = context.Users.Where(u => u.Id.ToString() == claim.Value).First<User>();
            if (user == null || user.NotUsed == true) return null;
            return user;
        }
        
    }
}
