using Education.Application.Interface;
using Education.Core.Model.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Education.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseServiceController<T> :  ControllerBase where T : class
    {
        public readonly IBaseService<T> _baseService;
        private Type? _currentType;
        protected Type currentType
        {
            get
            {
                if (_currentType == null)
                {
                    throw new NotImplementedException("chưa gán modal type");
                };
                return _currentType;
            }
            set
            {
                _currentType = value;
            }
        }
        public BaseServiceController(IBaseService<T> baseService)
        {
            _baseService = baseService;
        }
        [HttpPost("add")]
        //[Authorize]
        public async Task<ServiceResponse> Add([FromBody] T data)
        {
            string permissoinName = currentType.GetCustomAttribute<TableEducationAttribute>()?.PermissionName ?? "";
            var listRole = new List<string>() { $"{permissoinName}.Add" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _baseService.Add(data);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpPut("update")]
        //[Authorize]
        public async Task<ServiceResponse> Update([FromBody] T data)
        {
            string tableName = currentType.GetCustomAttribute<TableEducationAttribute>()?.TableName ?? "";
            var listRole = new List<string>() { $"{tableName}.Update" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _baseService.Update(data);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpDelete("delete/{id}")]
        //[Authorize]
        public async Task<ServiceResponse> Delete([FromRoute] int id) 
        {
            string tableName = currentType.GetCustomAttribute<TableEducationAttribute>()?.TableName ?? "";
            var listRole = new List<string>() { $"{tableName}.Delete" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _baseService.Delete(id);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpGet("get/{id}")]
        public async Task<ServiceResponse> GetByID([FromRoute] object id)
        {
            string tableName = currentType.GetCustomAttribute<TableEducationAttribute>()?.TableName ?? "";
            var listRole = new List<string>() { $"{tableName}.View" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _baseService.GetById(id);
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }
        [HttpGet("get-all")]
        public async Task<ServiceResponse> GetAll()
        {
            string permisstionName = currentType.GetCustomAttribute<TableEducationAttribute>()?.PermissionName ?? "";
            var listRole = new List<string>() { $"{permisstionName}.View" };
            var checkPermisstion = await _baseService.CheckRoleAccess(listRole);
            if (checkPermisstion)
            {
                return await _baseService.GetAll();
            }
            return new ServiceResponse(false, "Bạn không có quyền");
        }

    }
}