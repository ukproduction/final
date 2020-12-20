using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hombay.Models
{
    public class ReportViewModel
    {
        public string Name { set; get; }
    }
    public class Employee
    {
        public int EmpId { get; set; }
    }
    public class Student
    {
        public string Recevier_user { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
    public class Accounts
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string ExpYear { get; set; }
        public string ExpMonth { get; set; }
        public string Cvc { get; set; }
    }

    public class MessageModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}


namespace Hombay
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class tbl_items
    {
     
        public HttpPostedFileBase ImageFile { get; set; }
    
    }
}

namespace Hombay
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class tbl_new_construction
    {
      
        public HttpPostedFileBase ImageFile { get; set; }
    }
}

namespace Hombay
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class tbl_users
    {
      
        public HttpPostedFileBase ImageFile { get; set; }
    }
}

namespace Hombay
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class User_Bidding
    {
      
        public HttpPostedFileBase ImageFile { get; set; }
       
    }
}

