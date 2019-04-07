﻿using CLERP.Database.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLERP.Database.Entities
{
    /// <summary>
    /// Represents a shelf in a warehouse in the Db
    /// </summary>
    public class Shelf : EntityBase
    {
        /// <summary>
        /// Designation of the shelf e.g. shelf 7A
        /// </summary>
        public string Designation { get; set; }        

        /// <summary>
        /// The warehouse this shelf is inside
        /// </summary>
        public virtual Warehouse Warehouse { get; set; }

        /// <summary>
        /// All compartments inside this shelf
        /// </summary>
        public virtual ICollection<Compartment> Compartments { get; set; }
    }
}
