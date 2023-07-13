using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DCM_Client_Demo.Controllers;

public class AccountController : Controller
{
    public async Task Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        await HttpContext.SignOutAsync("oidc");
    }
    
    
}