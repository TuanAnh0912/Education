using Education.Application.Interface;
using Education.Core.Interface;
using Education.Core.Model.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Application.Service
{
    public class BaseService<T> : IBaseService<T> where T:class
    {
        private readonly IGenericRepository<T> _genericRepository;
        protected Guid _UserID
        {
            get
            {
                var userID = _claimProvider.GetUserID();
                if (userID != Guid.Empty) { __UserID = userID; }; return __UserID;
            }
            set { __UserID = value; }
        }
        private Guid __UserID { get; set; }
        protected string _Email { get { return _claimProvider.GetEmail(); } set {; } }
        protected string _UserName { get { return _claimProvider.GetUserName(); } set {; } }
        private IClaimProvider _claimProvider;
        //private IRolePermisstionRepository _rolePermisstionProvider;
        public BaseService(IServiceProvider serviceProvider)
        {
            _genericRepository = serviceProvider.GetRequiredService<IGenericRepository<T>>();
            _claimProvider = serviceProvider.GetRequiredService<IClaimProvider>();
            //_rolePermisstionProvider = serviceProvider.GetRequiredService<IRolePermisstionRepository>();
        }
        public async virtual Task<ServiceResponse> Add(T entity)
        {
            var resInsert = await _genericRepository.Add(entity);
            if (resInsert == null)
            {
                return new ServiceResponse(false, "Thêm mới thất bại");
            }
            return new ServiceResponse(true, "Thêm mới thành công", resInsert);
        }

        public async virtual Task<ServiceResponse> Delete(object ID)
        {
            var resDel = await _genericRepository.Delete(ID);
            if (!resDel)
            {
                return new ServiceResponse(false, "Xóa thất bại");
            }
            return new ServiceResponse(true, "Xóa thành công");
        }

        public async virtual Task<ServiceResponse> GetAll()
        {
            var datas = await _genericRepository.GetAll();
            if (datas == null)
            {
                return new ServiceResponse(false, "Lấy dữ liệu thất bại");
            }
            return new ServiceResponse(true, "Lấy dữ liệu thành công", datas);
        }

        public async virtual Task<ServiceResponse> GetById(object ID)
        {
            var resGetByID = await _genericRepository.GetById(ID);
            if (resGetByID == null)
            {
                return new ServiceResponse(false, "Lấy dữ liệu thất bại");
            }
            return new ServiceResponse(true, "Lấy dữ liệu thành công", resGetByID);
        }

        public async virtual Task<ServiceResponse> Update(T entity)
        {
            var resUpdate = await _genericRepository.Update(entity);
            if (!resUpdate)
            {
                return new ServiceResponse(false, "cập nhật dữ liệu thất bại");
            }
            return new ServiceResponse(true, "cập nhật dữ liệu thành công");
        }

        public Task<bool> CheckRoleAccess(List<string> listRole)
        {
            throw new NotImplementedException();
        }

        //public async Task<bool> CheckRoleAccess(List<string> listRole)
        //{
        //    if (_UserID == Guid.Empty) return false;
        //    var lstRolePermisstion = await _rolePermisstionProvider.GetRolePermisstionsByUserID(_UserID.ToString());
        //    if (lstRolePermisstion == null || lstRolePermisstion.Count == 0) return false;
        //    var lstRoleOwner = lstRolePermisstion.Select(x => x.PermisstionName);
        //    var countPermisstion = 0;
        //    foreach (var item in listRole)
        //    {
        //        if (lstRoleOwner.Contains(item))
        //        {
        //            countPermisstion++;
        //            if (countPermisstion == listRole.Count)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
    }
}
