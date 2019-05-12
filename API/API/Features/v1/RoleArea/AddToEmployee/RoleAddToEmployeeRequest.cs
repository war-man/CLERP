﻿using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLERP.API.Features.v1.RoleArea.AddToEmployee
{
    public class RoleAddToEmployeeRequest : IRequest<bool>
    {
        /// <summary>
        /// Id of the role which should be added to the employee
        /// </summary>
        [JsonProperty("roled-id")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// Id of the employee whicht the role should be added to
        /// </summary>
        [JsonProperty("employee-id")]
        public Guid EmployeeId { get; set; }
    }
}