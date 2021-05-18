using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.API.Settings
{
    public class Key
    {
        public const string AuthCacheKey = "appUser";
        public const string AuthHeaderKey = "Authorization";
        public const string JWTPrefixKey = "Bearer";
        public const string JWTUserIdKey = "userId";
    }
}
