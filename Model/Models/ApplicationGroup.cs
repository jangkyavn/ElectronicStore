﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Models
{
    [Table("ApplicationGroups")]
    public class ApplicationGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [StringLength(250)]
        public string Name { set; get; }

        [StringLength(250)]
        public string Description { set; get; }

        public virtual IEnumerable<ApplicationRoleGroup> ApplicationRoleGroups { get; set; }
        public virtual IEnumerable<ApplicationUserGroup> ApplicationUserGroups { get; set; }
    }
}
