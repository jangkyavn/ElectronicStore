﻿using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {

        }
        [StringLength(250)]
        public string Description { set; get; }

        public virtual IEnumerable<ApplicationRoleGroup> ApplicationRoleGroups { get; set; }
    }
}
