using System;
namespace AWSApplication.Model
{
	public class FileModel
	{
		public string NameFile { get; set; }
		public string LastModifay { get; set; }
		public string SizeFile { get; set; }
		public FileModel(string name,string lastMod,string size)
		{
			this.NameFile = name;
			this.LastModifay = lastMod;
			this.SizeFile = size;
		}
	}
}

