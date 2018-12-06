using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Domain.Models.Technology
{
    /// <inheritdoc />
    public class CreateTechnologyModel : Database.Models.Technology
    {
        public CreateTechnologyModel()
        {
            Name = TechnologiesName;
        }

        [NotMapped] 
        public string TechnologiesName { get; set; }
    }
}