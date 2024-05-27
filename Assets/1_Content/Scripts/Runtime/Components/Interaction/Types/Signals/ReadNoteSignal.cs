namespace NJN.Runtime.Components
{
    public struct ReadNoteSignal
    {
        public string NoteHeader { get; }
        public string NoteBody { get; }
        
        public ReadNoteSignal(string noteHeader, string noteBody)
        {
            NoteHeader = noteHeader;
            NoteBody = noteBody;
        }
    }
}