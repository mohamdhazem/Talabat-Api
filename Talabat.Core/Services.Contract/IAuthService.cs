﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Services.Contract
{
    public interface IAuthService
    {
        Task<string> CreateToken(AppUser user);
    }
}
