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
    
    public partial class tbl_cart_item
    {
        public int Id { get; set; }
        public Nullable<int> product_id { get; set; }
        public string Item_title { get; set; }
        public string Item_price { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<int> user_id { get; set; }
    }
}