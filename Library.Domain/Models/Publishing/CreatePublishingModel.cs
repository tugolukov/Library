namespace Library.Domain.Models.Publishing
{
    /// <inheritdoc />
    public class CreatePublishingModel : Database.Models.Publishing
    {
        public CreatePublishingModel()
        {
            Name = PublishingName;
        }

        public string PublishingName { get; set; }
    }
}