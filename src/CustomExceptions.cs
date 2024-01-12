namespace CustomExceptions;

abstract class CustomException : ApplicationException {
    protected string _messageDetails = string.Empty;
    public override string Message => $"\nError Message: {_messageDetails}";

    public CustomException(){}
    public CustomException(string message){
        _messageDetails = message;
    }
}

class SerializationException : CustomException {
    public SerializationException() : base() {}
    public SerializationException(string message) : base(message) {}
}

class DeserializationException : CustomException {
    public DeserializationException() : base() {}
    public DeserializationException(string message) : base(message) {}
}

class JsonReadException : CustomException {
    public JsonReadException() : base() {}
    public JsonReadException(string message) : base(message) {}
}

class JsonWriteException: CustomException {
    public JsonWriteException() : base() {}
    public JsonWriteException(string message) : base(message) {}
}