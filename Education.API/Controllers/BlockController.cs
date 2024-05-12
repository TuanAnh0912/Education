using Education.Application.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.DataModel;
using Education.Core.Model.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.DataValidation;
using System.ComponentModel;
using System.Reflection;

namespace Education.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class BlockController : BaseServiceController<Block>
    {
        public BlockController(IBlockService baseService, IServiceProvider serviceProvider) : base(baseService)
        {
            this.currentType = typeof(Block);
        }
    }

}