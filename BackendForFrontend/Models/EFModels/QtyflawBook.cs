﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BackendForFrontend.Models.EFModels;

public partial class QtyflawBook
{
    public int ID { get; set; }

    public int BookID { get; set; }

    public string Reason { get; set; }

    public DateTime HandlingDate { get; set; }

    public string HandlingMethod { get; set; }

    public string Handler { get; set; }

    public string Status { get; set; }

    public virtual Product Book { get; set; }
}