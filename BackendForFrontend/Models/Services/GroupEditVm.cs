using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.ViewModels
{
    public class GroupEditVm
    {
        public int GroupId { get; set; }
        public List<GroupFunctionVm> CanChooseFunctions { get; set; }
        public List<GroupFunctionVm> AllFunctions { get; set; }
        public List<int> SelectedFunctions { get; set; } // 保持不變

        public GroupEditVm()
        {
            CanChooseFunctions = new List<GroupFunctionVm>(); // 注意類型更改
            AllFunctions = new List<GroupFunctionVm>(); // 注意類型更改
            SelectedFunctions = new List<int>(); // 初始化列表
        }
    }
}
