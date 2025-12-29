using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KaufmannPro.Web.Pages.Mandanten
{
    [Authorize]
    public class MandantenModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
