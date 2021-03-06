﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestingWebAPI.Entity
{
    public class Blog
    {
        public int BlogID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Owner { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public Blog()
        {
            Articles = new HashSet<Article>();
        }
    }
}
