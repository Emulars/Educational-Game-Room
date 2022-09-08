namespace UEBlockly
{
    public enum ErrorCode
    {
        NoError,
        NullValue,
        NameHasSpaces,
        FirstElementIsDigit,    //to check string first char
        NoValidName,
        EmptyString,
        NameAlreadyUsed,    //controllabile solo da chi ha il dictionary, quindi solo da chi poi esegue il codice
                            // non è possibilie quindi inviare questo errore con SendMessage
                            // in quanto viene scoperto solo da chi riceve i messaggi

        NotDroppedValue,    //valore passato da un altro blocco mancante
        ArgumentTypeNotValid,
    }
}