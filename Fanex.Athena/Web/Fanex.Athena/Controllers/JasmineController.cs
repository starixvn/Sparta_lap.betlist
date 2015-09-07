using System;
using System.Web.Mvc;

namespace Fanex.Athena.Controllers
{
    public class JasmineController : Controller
    {
        public ViewResult Run()
        {
            return View("SpecRunner");
        }
    }
}
