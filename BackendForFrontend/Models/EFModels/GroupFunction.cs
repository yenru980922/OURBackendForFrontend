﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BackendForFrontend.Models.EFModels;

public partial class GroupFunction
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<GroupPermission> Groups { get; set; } = new List<GroupPermission>();
}