namespace Hombay.models
{
    using System;
    using System.Collections.Generic;

    public partial class SignUp
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }

    }
}
