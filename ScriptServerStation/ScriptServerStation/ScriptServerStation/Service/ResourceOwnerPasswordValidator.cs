using DataBaseController;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ScriptServerStation.Service
{
    public class ResourceOwnerPasswordValidator :BaseService, IResourceOwnerPasswordValidator
    {
        public ResourceOwnerPasswordValidator(DataBaseContext DataBaseContext) : base(DataBaseContext)
        {
            
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            if (this.DataBaseContext.Users.Count(x => x.Account == context.UserName && x.Password == context.Password)>0)
            {
                DataBaseController.Entitys.User user = this.DataBaseContext.Users.First(x => x.Account == context.UserName && x.Password == context.Password);
                context.Result = new GrantValidationResult(
                    subject: context.UserName,
                    authenticationMethod: "custom",
                    claims: GetUserClaims(user));
            }
            else
            {
                
                 //验证失败
                 context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
        }
        //可以根据需要设置相应的Claim
        private Claim[] GetUserClaims(DataBaseController.Entitys.User user)
        {
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                new Claim(JwtClaimTypes.Name, user.Account),
                new Claim(JwtClaimTypes.Email, user.Email==null?"":user.Email),
                new Claim(JwtClaimTypes.PhoneNumber,user.Phone==null?"":user.Phone),
                new Claim("Level", user.Level.ToString())
            };
        }
    }
}
