using System;

namespace AzureDevopsWork.Services.Models.Views
{
    public class Config
    {
        public string uri { get; set; }

        public string projeto { get; set; }

        public string access_token { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(uri))
                return false;

            if (string.IsNullOrEmpty(projeto))
                return false;

            if (string.IsNullOrEmpty(access_token))
                return false;

            return true;
        } 
    }
}
