using FileHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models
{
    [DelimitedRecord(";")]
    public class Member
    {
        public string MemberID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MemberOf { get; set; }
        
        public string DiscountCode { get; set; }
    }

    public class DiscountCode {
        public long DiscountCodeID { get; set; }
        public string DiscountHash { get; set; }
        public Boolean Used { get; set; }

    }

    public class DiscountContext : DbContext { 
        public DbSet<DiscountCode> DiscountCodes {get; set;}
        public DbSet<Member> Members { get; set; }


    }
}
