using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Movies.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly algebramssqlhost_moviesContext _context;
        private readonly IConfiguration _configuration;
        //konstruktor
        public AuthenticateController(algebramssqlhost_moviesContext context, IConfiguration configuration)
        {
            _context = context;
            //Konfiguracija za token
            _configuration = configuration;
        }

        //POST: api/autenticate/login
        [HttpPost]
        [Route("login")]
        public ActionResult Login(LoginModel model)
        {
            var user=_context.AspNetUsers.FirstOrDefault(u=>u.UserName == model.UserName);
            if(user != null)
            {
                //Korisnik postoji ali provjeri i lozinku

                //uspoređujemo lozinku jer je u bazi lozinka heširana...
                var hasher = new PasswordHasher<AspNetUser>();
                //VerifyHashedPassword() -> verificira hash lozinke iz tablice s stringom za prijavu
                var is_valid = hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                if (is_valid == PasswordVerificationResult.Success)
                {
                    //I korisnik i lozinka se poklapaju kreiramo listu klemova
                    var auth_claim = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName)
                    };
                    //generiranje tokena u privatnoj f-iji GetToken()

                    var token = GetToken(auth_claim);


                    return Ok(
                        //radimo anonimni tip podataka
                        new
                        {
                            token=new JwtSecurityTokenHandler().WriteToken(token),
                            expiration=token.ValidTo 
                        }
                        );
                }


                return Unauthorized();

            }
            //Korisnik ne postoji
            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> auth_claims)
        {
            //generiranje asimetričnog ključa
            var auth_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Secret"]));
            var token = new JwtSecurityToken(issuer: _configuration["JWTSettings:ValidIssuer"], audience: _configuration["JWTSettings:ValidAudience"], expires: DateTime.Now.AddHours(3), claims: auth_claims,
                signingCredentials: new SigningCredentials(auth_key, SecurityAlgorithms.HmacSha256));
            return token;
        }


    }
}
