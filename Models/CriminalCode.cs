using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CidadeAlta_CodigoPenal.Models
{
    public class CriminalCode
    {   
        [Key]
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Penalty { get; set; }
        public int PrisonTime { get; set; }


        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        [ForeignKey("User")]
        public int CreateUserId { get; set; }
        [ForeignKey("User")]
        public int UpdateUserId { get; set; }

        public bool VerifyValueName(string campo)
        {
            string valor = campo.ToLower();

            if (valor == "id") return true;
            else if(valor == "name") return true;
            else if (valor == "description") return true;
            else if (valor == "penalty") return true;
            else if (valor == "prisontime") return true;
            else if (valor == "status") return true;
            else if (valor == "createdate") return true;
            else if (valor == "updatedate") return true;
            else if (valor == "createuser") return true;
            else if (valor == "updateuser") return true;
            else return false;
        }
    }
}
