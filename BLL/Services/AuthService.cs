using AutoMapper;
using BLL.DTO;
using BLL.DTO.Adding;
using DAL.Entities;
using DAL.UnitsOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BLL.Services
{
    public class AuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.configuration = configuration;
        }


        public async Task<string> Register(WorkerAddingDTO workerRegistrationRequest)
        {
            var workers = await unitOfWork.WorkerRepository.GetAllAsync();

            if (workers.Any(u => u.Email.ToLower() == workerRegistrationRequest.Email.Trim().ToLower()))
            {
                return "Email is taken";
            }

            CreatePasswordHash(workerRegistrationRequest.Password.Trim().ToLower(), out byte[] passwordHash, out byte[] passwordSalt);

            var workerToAdd = mapper.Map<Worker>(workerRegistrationRequest);
            workerToAdd.Email = workerToAdd.Email.Trim().ToLower();
            workerToAdd.PasswordHash = passwordHash;
            workerToAdd.PasswordSalt = passwordSalt;

            await unitOfWork.WorkerRepository.AddAsync(workerToAdd);

            return "User successfully registered";
        }

        public async Task<string> Login(WorkerLoginDTO loginRequest)
        {
            var loggingWorker = await unitOfWork.WorkerRepository.FindByEmailAsync(loginRequest.Email.Trim());

            if (loggingWorker is null || !VerifyPasswordHash(
                loginRequest.Password, loggingWorker.PasswordHash, loggingWorker.PasswordSalt))
            {
                return "Email or password is incorrect";
            }

            var workerToGiveTokenTo = await unitOfWork.WorkerRepository.GetDetailsAsync(loggingWorker.WorkerId);

            return CreateToken(workerToGiveTokenTo);
        }

        public async Task<bool> ChangePassword(WorkerLoginDTO changingPasswordRequest)
        {
            var workerToChangePassword = await unitOfWork.WorkerRepository.FindByEmailAsync(changingPasswordRequest.Email.Trim());

            if (workerToChangePassword is null)
            {
                return false;
            }

            CreatePasswordHash(changingPasswordRequest.Password.Trim().ToLower(), out byte[] passwordHash, out byte[] passwordSalt);
            
            if (workerToChangePassword.PasswordHash == passwordHash && workerToChangePassword.PasswordSalt == passwordSalt)
            {
                return false;
            }

            workerToChangePassword.PasswordHash = passwordHash;
            workerToChangePassword.PasswordSalt = passwordSalt;

            await unitOfWork.WorkerRepository.UpdateAsync(workerToChangePassword);

            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }

        private string CreateToken(Worker worker)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, worker.Position.Name)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:SecretKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
