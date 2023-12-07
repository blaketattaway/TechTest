namespace TechTest.Domain.Entities
{
    public class ResponseObject<T>
    {
        public int Status { get; set; } = 200;
        public string StatusText { get; set; } = "POST Request successful";
        public T? Data { get; set; }
    }
}