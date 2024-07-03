﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BackendForFrontend.Models.EFModels;

public partial class BookProduct
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int ProductId { get; set; }

    public int PublisherId { get; set; }

    public DateTime PublishDate { get; set; }

    public string ISBN { get; set; }

    public virtual Book Book { get; set; }

    public virtual Product Product { get; set; }

    public virtual Bookseller Publisher { get; set; }
}