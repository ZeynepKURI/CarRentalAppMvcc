using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalAppMvc.Models
{
	public class VehicleWorkingTime
    {
        [Key]
        public int Id { get; set; }
        public float ActiveWorkTime { get; set; }
        public float MaintenanceTime { get; set; }
        public float IdleTime { get; set; }



    [ForeignKey("Vehicle")]        //Foreign Key (Yabancı Anahtar), bir veritabanında iki tablo arasındaki ilişkiyi tanımlamak için kullanılan bir sütundur.
        public int VehicleId { get; set; }
        public  virtual Vehicle Vehicle { get; set; }                    //virtual, Entity Framework'te lazy loading mekanizmasını etkinleştirmek için kullanılır.
                                                                 //Lazy loading, ilişkili verilerin yalnızca ihtiyaç duyulduğunda yüklenmesini sağlar.


    }
}

