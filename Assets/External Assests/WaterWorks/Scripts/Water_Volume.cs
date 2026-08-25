using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;

public class Water_Volume : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private Material _material;

        public CustomRenderPass(Material mat)
        {
            _material = mat;
            requiresIntermediateTexture=true;

        }
        class PassData
        {
            public Material material;
            public TextureHandle source;
        }

        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in an performance manner.
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)

        {
            if (_material==null) return;
            UniversalCameraData cameraData=frameData.Get<UniversalCameraData>();
            if (cameraData.cameraType==CameraType.Reflection) return;
            UniversalResourceData resourceData=frameData.Get<UniversalResourceData>();
            TextureHandle sourceTexture =resourceData.activeColorTexture;

            if (!sourceTexture.IsValid()) return;

            RenderTextureDescriptor desc=cameraData.cameraTargetDescriptor;
            desc.depthBufferBits=0;
            TextureHandle tempTexture=UniversalRenderer.CreateRenderGraphTexture(renderGraph,desc,"_TemporaryColourTexture",false);
            using(var builder=renderGraph.AddRasterRenderPass<PassData>("Water_Volume_Effect",out var passData))
            {
                passData.material=_material;
                passData.source=sourceTexture;
                builder.UseTexture(sourceTexture,AccessFlags.Read);
                builder.SetRenderAttachment(tempTexture,0,AccessFlags.Write);
                
                builder.SetRenderFunc((PassData data,RasterGraphContext context)=>
                {
                    Blitter.BlitTexture(context.cmd,data.source,new Vector4(1,1,0,0),data.material,0);

                });
            }
            using(var builder=renderGraph.AddRasterRenderPass<PassData>("Water_Volume_BlitBack",out var passData))
           {
            passData.source=tempTexture;
            builder.UseTexture(tempTexture,AccessFlags.Read);
            builder.SetRenderAttachment(sourceTexture,0,AccessFlags.Write);
            builder.SetRenderFunc((PassData data,RasterGraphContext context)=>
            {
                Blitter.BlitTexture(context.cmd,data.source,new Vector4(1,1,0,0),0.0f,false);
            });
           
           }
        }
    }
    [System.Serializable]
    public class _Settings
    {
        //[HideInInspector]
        public Material material = null;
        public RenderPassEvent renderPass = RenderPassEvent.AfterRenderingSkybox;
    }

    public _Settings settings = new _Settings();

    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        if(settings.material == null)
        {
            settings.material = (Material)Resources.Load("Water_Volume");
        }

        m_ScriptablePass = new CustomRenderPass(settings.material);

        // Configures where the render pass should be injected.
        //m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        m_ScriptablePass.renderPassEvent = settings.renderPass;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (m_ScriptablePass!=null)
        {   
            renderer.EnqueuePass(m_ScriptablePass);
        }
    }
}
