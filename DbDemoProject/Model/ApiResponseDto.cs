namespace DbDemoProject.Model
{
    public class ApiResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public string? Tokan { get; set; }

    }
}
