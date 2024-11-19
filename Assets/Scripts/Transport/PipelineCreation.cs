using Unity.Networking.Transport;

namespace DistractorProject.Transport
{
    public static class PipelineCreation
    {
        public static NetworkPipeline CreatePipeline(ref NetworkDriver driver)
        {
            return driver.CreatePipeline(typeof(FragmentationPipelineStage), typeof(ReliableSequencedPipelineStage));
        }
    }
}