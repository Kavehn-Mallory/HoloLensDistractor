using Unity.Collections;

namespace DistractorProject.Core
{
    public interface ISerializer
    {
        public void Serialize(ref DataStreamWriter dataStreamWriter);
        
        public void Deserialize(ref DataStreamReader dataStreamReader);
    }
}