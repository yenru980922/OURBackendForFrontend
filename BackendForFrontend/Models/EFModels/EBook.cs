﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BackendForFrontend.Models.EFModels;

public partial class EBook
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string FileLink { get; set; }

    public string Sample { get; set; }

    public virtual ICollection<EBooksPermission> EBooksPermissions { get; set; } = new List<EBooksPermission>();

    public virtual Product Product { get; set; }
}