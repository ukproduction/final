//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hombay
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Order
    {
        public int Id { get; set; }
        public string Id_Number { get; set; }
        public Nullable<int> Item_Userid { get; set; }
        public Nullable<int> User_id { get; set; }
        public string Product_name { get; set; }
        public string Product_price { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }
}
