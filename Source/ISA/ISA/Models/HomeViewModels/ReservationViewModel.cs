using System;

namespace ISA.Models.HomeViewModels
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        public CinemaViewModel Theatre { get; set; }
        public ProjectionViewModel Projection { get; set; }
    }


    public class ProjectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public string Description { get; set; }
    }
    public class CinemaViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
    }
}
