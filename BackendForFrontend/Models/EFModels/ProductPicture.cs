﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BackendForFrontend.Models.EFModels;

public partial class ProductPicture
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Name { get; set; }

    public int DisplayOrder { get; set; }

    public virtual Product Product { get; set; }
}