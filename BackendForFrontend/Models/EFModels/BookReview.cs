﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BackendForFrontend.Models.EFModels;

public partial class BookReview
{
    public int ReviewID { get; set; }

    public int MemberID { get; set; }

    public DateTime ReviewTime { get; set; }

    public string Content { get; set; }

    public string Rating { get; set; }

    public bool IsSpoiler { get; set; }

    public virtual Member Member { get; set; }
}