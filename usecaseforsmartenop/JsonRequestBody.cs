using System;
namespace usecaseforsmartenop
{
	public class JsonRequestBody
	{
		public JsonRequestBody()
		{
		}

        public string model { get; set; }
        public PromptItem[] messages { get; set; }
    }
}

