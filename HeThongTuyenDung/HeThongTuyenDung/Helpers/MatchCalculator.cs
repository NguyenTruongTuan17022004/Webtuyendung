using HeThongTuyenDung.Models;
using System;
using System.Linq;

namespace HeThongTuyenDung.Helpers
{
    public static class MatchCalculator
    {
        /// <summary>
        /// Calculates the match score (0-100) between a job posting and a candidate profile.
        /// </summary>
        public static int CalculateMatchScore(TinTuyenDung job, HoSoUngVien candidate)
        {
            if (job == null || candidate == null) return 0;

            int score = 0;

            // 1. Skill Match (Max 50 points)
            if (job.KyNangTuyenDungs != null && job.KyNangTuyenDungs.Any())
            {
                var jobSkills = job.KyNangTuyenDungs.Select(k => k.MaKyNang).ToList();
                if (candidate.KyNangUngViens != null && candidate.KyNangUngViens.Any())
                {
                    var candidateSkills = candidate.KyNangUngViens.Select(k => k.MaKyNang).ToList();
                    int matchedSkills = candidateSkills.Count(s => jobSkills.Contains(s));
                    
                    double skillPercentage = (double)matchedSkills / jobSkills.Count;
                    score += (int)Math.Round(skillPercentage * 50);
                }
            }
            else
            {
                // If job has no specific skills required, give full points for this section
                // or you could give 0, depending on business logic. Let's give prorated points.
                score += 50; 
            }

            // 2. Location Match (Max 20 points)
            if (!string.IsNullOrEmpty(candidate.DiaDiemMongMuon))
            {
                bool isLocationMatch = false;
                string candidateLocationLower = candidate.DiaDiemMongMuon.ToLower();

                if (!string.IsNullOrEmpty(job.DiaDiem) && candidateLocationLower.Contains(job.DiaDiem.ToLower()))
                {
                    isLocationMatch = true;
                }
                else if (!string.IsNullOrEmpty(job.ThanhPho) && candidateLocationLower.Contains(job.ThanhPho.ToLower()))
                {
                    isLocationMatch = true;
                }

                if (isLocationMatch)
                {
                    score += 20;
                }
            }

            // 3. Experience Match (Max 20 points)
            if (job.KinhNghiemTu.HasValue)
            {
                if (candidate.SoNamKinhNghiem.HasValue && candidate.SoNamKinhNghiem.Value >= job.KinhNghiemTu.Value)
                {
                    score += 20;
                }
                else if (candidate.SoNamKinhNghiem.HasValue && candidate.SoNamKinhNghiem.Value > 0 && job.KinhNghiemTu.Value > 0)
                {
                    // Partial points if they have some experience but less than required
                    double xpRatio = (double)candidate.SoNamKinhNghiem.Value / job.KinhNghiemTu.Value;
                    score += (int)Math.Round(xpRatio * 20);
                }
            }
            else
            {
                // No minimum experience required
                score += 20;
            }

            // 4. Salary Match (Max 10 points)
            if (candidate.MucLuongMongMuon.HasValue && job.LuongTu.HasValue)
            {
                // If candidate wants less than or equal to max salary, or within the range
                if (job.LuongDen.HasValue)
                {
                    if (candidate.MucLuongMongMuon.Value <= job.LuongDen.Value)
                    {
                        score += 10;
                    }
                }
                else
                {
                    // No max salary specified, just check if they are okay with the starting salary
                    // Usually candidates want more, but if they want less or around the starting point, it's a match
                     if (candidate.MucLuongMongMuon.Value <= job.LuongTu.Value * 1.5m) // Arbitrary tolerance
                     {
                         score += 10;
                     }
                }
            }
            else
            {
                // Unspecified salary matching
                score += 10;
            }

            // Cap at 100 just in case
            return Math.Min(100, Math.Max(0, score));
        }
    }
}
