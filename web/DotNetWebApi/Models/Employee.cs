using System.ComponentModel.DataAnnotations;

namespace DotNetWebApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int Age { get; set; }
        public string MobileNo { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime JoinDate { get; set; }


    }
}
