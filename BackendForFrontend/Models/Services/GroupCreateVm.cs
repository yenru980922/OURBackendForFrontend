using BackendForFrontend.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.ViewModels
{
    public class GroupCreateVm
    {
        public string GroupName { get; set; }
        public List<GroupFunctionVm> AllFunctions { get; set; }
        public List<int> SelectedFunctions { get; set; }
    }
}