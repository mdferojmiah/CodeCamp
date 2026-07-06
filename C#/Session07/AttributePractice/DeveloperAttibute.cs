namespace AttributePractice
{
    public class DeveloperAttribute: Attribute
    {
        public string Name { get;} = string.Empty;
        public string Version { get; set; } = string.Empty;

        public DeveloperAttribute(string name)
        {
            Name =  name;
            Version = "1.0";
        }
    }
}