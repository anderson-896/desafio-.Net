//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LocalData
{
    using System;
    using System.Collections.Generic;
    
    public partial class TB_TOKEN_AUTHENTICATION
    {
        public int ID { get; set; }
        public int USER_ID { get; set; }
        public string TOKEN { get; set; }
        public bool IS_VALID { get; set; }
        public System.DateTime CREATE_DATE { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }
    
        public virtual TB_USER TB_USER { get; set; }
    }
}