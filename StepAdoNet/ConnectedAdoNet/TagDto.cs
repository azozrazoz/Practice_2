namespace ConnectedAdoNet
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsSystem { get; set; }
        public DateTime CreatedOn { get; }

        public TagDto(string name, bool isSystem)
        {
            Id = Guid.NewGuid();
            Name = name;
            IsSystem = isSystem;
            CreatedOn = DateTime.Now;
        }
    }
}
