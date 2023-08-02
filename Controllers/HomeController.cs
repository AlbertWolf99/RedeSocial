using Microsoft.AspNetCore.Mvc;

namespace RedeSocial.Controllers;

// Este controller do MVC é usado apenas para redirecionar a url raiz para o swagger em servidor de desenvolvimento
public class HomeController : Controller
{
    
    private IHostEnvironment CurrentEnvironment{ get; set; }

    public HomeController(IHostEnvironment env)
    {
        CurrentEnvironment = env;
    }
    
    public ActionResult Index()
    {
        
        if(CurrentEnvironment.IsDevelopment())
            return this.Redirect("/swagger");
        else 
            return this.NotFound();
    }
}