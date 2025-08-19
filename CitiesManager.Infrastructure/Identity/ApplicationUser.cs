﻿using Microsoft.AspNetCore.Identity;

namespace CitiesManager.Infrastructure.Identity
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public string? Name { get; set; }
    }
}
