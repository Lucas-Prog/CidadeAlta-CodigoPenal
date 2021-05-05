using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CidadeAlta_CodigoPenal.Models
{
    public class ResponseCriminalCode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Penalty { get; set; }
        public int PrisonTime { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        //public ResponseCriminalCode(int id, string name, string description, decimal penalty, int prisonTime, string status, DateTime createDate, DateTime updateDate, string createUser, string updateUser)
        //{
        //    this.Id = id;
        //    this.Name = name;
        //    this.Description = description;
        //    this.Penalty = penalty;
        //    this.PrisonTime = prisonTime;
        //    this.Status = status;
        //    this.CreateDate = createDate;
        //    this.UpdateDate = updateDate;
        //    this.CreateUser = createUser;
        //    this.UpdateUser = updateUser;
        //}
    }
}
