using System;
using AWSApplication.Model;
using Microsoft.AspNetCore.Mvc;

namespace AWSApplication.Controllers
{
	[ApiController]
	[Route("api/controller")]
	public class AWSConteroller:ControllerBase
	{
		private AWSManager manager;
		public AWSConteroller()
		{
			manager = new AWSManager();
		}
		[HttpPost]
		[Route("DownloadFile")]
		public void DownloadFile( string name)
		{
			this.manager.Download(name);
		}
        [HttpPost]
        [Route("UploadFile")]
        public void UploadFile(IFormFile file)
        {

			this.manager.Upload(file);
		}
        [HttpPost]
        [Route("DeleteFile")]
        public void DeleteFile(string fileName)
        {
			this.manager.Delete(fileName);
		}
        [HttpGet]
        [Route("GetListFile")]
        public Task<List<FileModel>> GetListFile()
        {
            return this.manager.List();
        }
    }
}

