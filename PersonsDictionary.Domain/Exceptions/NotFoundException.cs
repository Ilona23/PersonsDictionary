namespace Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public bool ShowMessage { get; set; }

        public NotFoundException(string message, bool showMessage = false) : base(message)
        {
            ShowMessage = showMessage;
        }
    }
}
