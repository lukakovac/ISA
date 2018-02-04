using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ISA.Models.ManageViewModels
{
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }

        public List<SelectListItem> Providers { get; set; }
    }
}
