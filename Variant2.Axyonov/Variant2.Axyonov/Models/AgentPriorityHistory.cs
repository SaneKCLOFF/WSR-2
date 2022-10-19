using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Variant2.Axyonov.Models
{
    [Table("AgentPriorityHistory")]
    public partial class AgentPriorityHistory
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("AgentID")]
        public int AgentId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ChangeDate { get; set; }
        public int PriorityValue { get; set; }

        [ForeignKey("AgentId")]
        [InverseProperty("AgentPriorityHistories")]
        public virtual Agent Agent { get; set; } = null!;
    }
}
