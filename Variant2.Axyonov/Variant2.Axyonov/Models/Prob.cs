using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Variant2.Axyonov.Models
{
    [Keyless]
    [Table("Prob")]
    public partial class Prob
    {
        [StringLength(50)]
        public string Продукция { get; set; } = null!;
        [Column("Наимование материала")]
        [StringLength(50)]
        public string НаимованиеМатериала { get; set; } = null!;
        [Column("Необходимое количество материала")]
        [StringLength(50)]
        public string НеобходимоеКоличествоМатериала { get; set; } = null!;
    }
}
