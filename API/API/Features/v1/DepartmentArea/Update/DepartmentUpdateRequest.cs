﻿using MediatR;
using Newtonsoft.Json;
using System;

namespace CLERP.API.Features.v1.DepartmentArea.Update
{
    public class DepartmentUpdateRequest : IRequest<DepartmentResponse>
    {
        [JsonIgnore]
        public Guid DepartmentId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
