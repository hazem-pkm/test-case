using System;
using CsvHelper.Configuration.Attributes;

namespace usecaseforsmartenop
{
	public class CsvClass
	{
        public string Questions { get; set; }
        [Name("Related Articles")]
        public string? Articles { get; set; }
        [Name("The Expected Answers")]
        public string Answers { get; set; }
    }

    public class PromptItem
    {
        public string role { get; set; }
        public string content { get; set; }
    }
}

