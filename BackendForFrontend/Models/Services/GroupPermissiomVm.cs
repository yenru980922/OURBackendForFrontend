using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.ViewModels
{
    public class GroupPermissionsVm : GroupFunctionVm
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<GroupFunctionVm> AvailableFunctions { get; set; } // 所有可分配的功能
        public List<int> SelectedFunctions { get; set; } // 已分配的功能ID列表
    }



}