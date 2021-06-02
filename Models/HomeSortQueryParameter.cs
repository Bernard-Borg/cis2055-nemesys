using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class HomeSortQueryParameter
    {
        public int StatusId { get; set; }
        public string SortString { get; set; }
    }
}