namespace DistractorProject.Core
{
    public interface IStudyStageEvent : ISerializer
    {
        public bool IsStartEvent { get; set; }
    }
}