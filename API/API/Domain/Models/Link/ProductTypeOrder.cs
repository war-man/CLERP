﻿using CLERP.API.Domain.Models.Abstract;
using System;

namespace CLERP.API.Domain.Models.Link
{
    /// <summary>
    /// Link table for the many to mayn relation between <see cref="ProductType"/> and <see cref="Order"/>
    /// </summary>
    public class ProductTypeOrder : LinkEntityBase
    {
        /// <summary>
        /// Foreign key to the mapped product-type
        /// </summary>
        public Guid ProductTypeGuid { get; set; }

        /// <summary>
        /// Foreign key to the mapped order
        /// </summary>
        public Guid OrderGuid { get; set; }

        /// <summary>
        /// Link to the mapped product-type
        /// </summary>
        public virtual ProductType ProductType { get; set; }

        /// <summary>
        /// Link to the mapped order
        /// </summary>
        public virtual Order Order { get; set; }
    }
}
