//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DbRepository.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task_Algotithm
    {
        public int TaskAlgorithmId { get; set; }
        public int TaskId { get; set; }
        public int SubGroup { get; set; }
        public int BlockNumber { get; set; }
        public string Condition { get; set; }
    
        public virtual Task Task { get; set; }
    }
}
