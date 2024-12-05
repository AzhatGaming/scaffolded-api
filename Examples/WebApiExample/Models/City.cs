using ScaffoldedApi.QueryFilter;
using System.ComponentModel.DataAnnotations;

namespace WebApiExample.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        [Filter(FilterType.String)]
        public string Name { get; set; } = string.Empty;
        [Filter(FilterType.String)]
        public string Slogan { get; set; } = string.Empty;
        [Filter(FilterType.DateRange)]
        public DateTime FoundedDate { get; set; }
    }
}
