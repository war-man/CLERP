﻿using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLERP.API.Features.v1.RoleArea.RemoveFromDepartment
{
    public class RoleRemoveFromDepartmentRequest : IRequest
    {
        /// <summary>
        /// Id of the role which should be removed from the department
        /// </summary>
        [JsonProperty("role-id")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// Id of the department to role should be removed from
        /// </summary>
        [JsonProperty("department-id")]
        public Guid DepartmentId { get; set; }
    }
}
