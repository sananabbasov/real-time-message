using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Dtos;
using WebUI.Models;

namespace WebUI.Controllers;

public class HomeController : Controller
{
     private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IHttpContextAccessor _httpContext;

    public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContext, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContext = httpContext;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        return View();
    }
   public IActionResult Login()
    {
        var user = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (user != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

     [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {


        var findUser = await _userManager.FindByEmailAsync(loginDto.Email);
        if (findUser == null)
            return View();


        var result = await _signInManager.PasswordSignInAsync(findUser, loginDto.Password, true, true);
        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        return View();
    }
    //GET, POST
    //AOP

    [HttpGet] // Attribute
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var findUser = await _userManager.FindByEmailAsync(registerDto.Email);

        if (findUser != null)
            return View();

        User newUser = new()
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email,
        };

        IdentityResult result = await _userManager.CreateAsync(newUser, registerDto.Password);


        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, "User");
            return RedirectToAction("Login");
        }


        return View();
    }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
