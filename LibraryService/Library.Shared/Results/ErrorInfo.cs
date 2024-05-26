namespace Library.Shared.Results
{
    public class ValidationError
    { 
        public string Identifier { get; set; } = null!;
        public string Description { get; set; } = null!;
        
        public ValidationError(string description, string identifier)
        {
            Description = description;
            Identifier = identifier;
        }
    }
}