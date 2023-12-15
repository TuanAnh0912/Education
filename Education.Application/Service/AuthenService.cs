using Education.Application.Helpers;
using Education.Application.Interface;
using Education.Core.Interface;
using Education.Core.Model.Core;
using Education.Core.Model.ResponseModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Service
{
    public class AuthenService : IAuthenService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtUtils _jwtUtils;
        private JwtIssuerOptions _jwtIssuerOptions;
        public AuthenService(IUserRepository userRepository, IJwtUtils jwtUtils, IOptions<JwtIssuerOptions> jwtIssuerOptions)
        {
            _userRepository = userRepository;
            _jwtIssuerOptions = jwtIssuerOptions.Value;
            _jwtUtils = jwtUtils;
        }
        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            password = AuthenHelpers.HashPassword(username + password);
            var rsCheckLogin = await _userRepository.CheckLogin(username, password);
            //todo: mesage sai: trường hợp tài khoản tồn tại rồi vẫn trả về "Tài khoản không tồn tại
            if (rsCheckLogin == null)
            {
                return new LoginResponse(false, "Tài khoản hoặc mật khẩu không đúng");
            }

            var token = _jwtUtils.GenerateJwtToken(rsCheckLogin);
            //var refreshToken = await _jwtUtils.GenerateRefreshToken();
            //rsCheckLogin.RefreshToken = refreshToken;
            var condition = $"UserID = '{rsCheckLogin.UserID}'";
           // var resUpdate = await _userRepository.UpdateCustomColumn(rsCheckLogin, new List<string>() { "RefreshToken" }, condition);
            return new LoginResponse(true, "Đăng nhập thành công", "", token);
            //if (resUpdate)
            //{
            //}
            // return new LoginResponse(false, "Tài khoản hoặc mật khẩu không đúng");
        }
        //public async Task<string> GetTokenByRefreshToken(string rfToken)
        //{
        //    var userByRefreshToken = await _userRepository.GetUserByRefreshToken(rfToken);
        //    if (userByRefreshToken == null)
        //    {
        //        return "";
        //    }
        //    return _jwtUtils.GenerateJwtToken(userByRefreshToken) ?? "";
        //}
    }
}
