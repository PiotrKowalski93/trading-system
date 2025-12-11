namespace OrderGateway.ApiGrpc.Validators
{
    public class NewOrderValidationResult
    {
        public bool Success { get; }
        public bool Failure { get; }
        public string FailedMessage { get; }

        public NewOrderValidationResult(string FailedMessage = "")
        {
            this.FailedMessage = FailedMessage;

            if(FailedMessage != "")
            {
                Success = false;
                Failure = true;
            }
            else
            {
                Success = true;
                Failure = false;
            }
        }
    }
}
