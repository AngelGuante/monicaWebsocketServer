public class ExceptionDTO{
    public string Ip { get; set; }
    public string Time { get; } = System.DateTime.Now.ToString("dd/MM/yyyy h:mm tt");
    public string Message { get; set; }
}