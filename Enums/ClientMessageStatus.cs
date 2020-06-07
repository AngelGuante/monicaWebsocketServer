namespace monicaWebsocketServer{
    public enum ClientMessageStatus{
        NONE = 0,
        Correct = 1,
        DuplicatedIP = 2,
        IndividualClientStatusReport = 3
    }
}