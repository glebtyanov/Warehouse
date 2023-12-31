﻿using AutoMapper;
using BLL.DTO;
using BLL.DTO.Adding;
using BLL.Exceptions;
using DAL.Entities;
using DAL.Enum;
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


        public async void Register(WorkerAddingDTO workerRegistrationRequest)
        {
            var workers = await unitOfWork.WorkerRepository.GetAllAsync();

            if (workers.Any(u => u.Email.ToLower() == workerRegistrationRequest.Email.Trim().ToLower()))
            {
                throw new AlreadyExistsException("Email is taken");
            }

            CreatePasswordHash(workerRegistrationRequest.Password.Trim().ToLower(), out byte[] passwordHash, out byte[] passwordSalt);

            var workerToAdd = mapper.Map<Worker>(workerRegistrationRequest);

            workerToAdd.Email = workerToAdd.Email.Trim().ToLower();
            workerToAdd.PasswordHash = passwordHash;
            workerToAdd.PasswordSalt = passwordSalt;
            workerToAdd.PositionId = (int)Enums.Positions.Regular;
            workerToAdd.HireDate = DateTime.Now;

            await unitOfWork.WorkerRepository.AddAsync(workerToAdd);

            return;
        }

        public async Task<string> Login(WorkerLoginDTO loginRequest)
        {
            var loggingWorker = await unitOfWork.WorkerRepository.FindByEmailAsync(loginRequest.Email.Trim());

            if (loggingWorker is null || !VerifyPasswordHash(
                loginRequest.Password, loggingWorker.PasswordHash, loggingWorker.PasswordSalt))
            {
                throw new NotFoundException("User with given email and password not found");
            }

            var workerToGiveTokenTo = await unitOfWork.WorkerRepository.GetDetailsAsync(loggingWorker.WorkerId);

            return CreateToken(workerToGiveTokenTo);
        }

        public async void ChangePassword(WorkerLoginDTO changingPasswordRequest)
        {
            var workerToChangePassword = await unitOfWork.WorkerRepository.FindByEmailAsync(changingPasswordRequest.Email.Trim());

            if (workerToChangePassword is null)
            {
                throw new NotFoundException("Worker with given email not found");
            }

            CreatePasswordHash(changingPasswordRequest.Password.Trim().ToLower(), out byte[] passwordHash, out byte[] passwordSalt);

            if (workerToChangePassword.PasswordHash == passwordHash && workerToChangePassword.PasswordSalt == passwordSalt)
            {
                throw new Exception();
            }

            workerToChangePassword.PasswordHash = passwordHash;
            workerToChangePassword.PasswordSalt = passwordSalt;

            await unitOfWork.WorkerRepository.UpdateAsync(workerToChangePassword);

            return;
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
                new Claim(ClaimTypes.Role, worker.Position.Name),
                new Claim("id", worker.WorkerId.ToString())
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
