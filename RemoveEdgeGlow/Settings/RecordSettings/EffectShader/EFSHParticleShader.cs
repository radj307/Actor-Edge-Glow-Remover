namespace RemoveEdgeGlow.SettingsObjects
{
    public class EFSHParticleShader
    {
        public EFSHParticleShader(uint startFrame = 0, uint startFrameVariation = 0, uint endFrame = 0, uint loopStartFrame = 0, uint loopStartVariation = 0, uint frameCount = 0, uint frameCountVariation = 0)
        {
            StartFrame = startFrame;
            StartFrameVariation = startFrameVariation;
            EndFrame = endFrame;
            LoopStartFrame = loopStartFrame;
            LoopStartVariation = loopStartVariation;
            FrameCount = frameCount;
            FrameCountVariation = frameCountVariation;
        }

        public uint StartFrame;
        public uint StartFrameVariation;
        public uint EndFrame;
        public uint LoopStartFrame;
        public uint LoopStartVariation;
        public uint FrameCount;
        public uint FrameCountVariation;
    }
}
