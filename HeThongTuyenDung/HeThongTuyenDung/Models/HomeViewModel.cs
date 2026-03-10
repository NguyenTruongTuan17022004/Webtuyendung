using System.Collections.Generic;

namespace HeThongTuyenDung.Models
{
    public class HomeViewModel
    {
        public List<TinTuyenDung> FeaturedJobs { get; set; } = new List<TinTuyenDung>();
        public List<TinTuyenDung> LatestJobs { get; set; } = new List<TinTuyenDung>();
        public int TotalJobs { get; set; }
        public int TotalCompanies { get; set; }
        public int TotalCandidates { get; set; }
        public int RecentJobsCount { get; set; }
        
        // Properties for candidate dashboard
        public int TotalApplications { get; set; }
        public int PendingApplications { get; set; }
        public int AcceptedApplications { get; set; }
        public int RejectedApplications { get; set; }
        public List<DonUngTuyen> RecentApplications { get; set; } = new List<DonUngTuyen>();
    }
} 